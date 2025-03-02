using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Build.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoviesApi.Helpers;
using MoviesApi.Models;
using MoviesApi.Services;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//get connection string from app setting
var connection_string=builder.Configuration.GetConnectionString("DefultConnection");
//add connection string 
builder.Services.AddDbContext<ApplicationDbContext>(
    options=>options.UseSqlServer(connection_string)
    );

builder.Services.AddControllers();

builder.Configuration.GetSection("JWT");
builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddTransient<IGenresService,GenresService>();
builder.Services.AddTransient<IMoviesService,MoviesService>();

builder.Services.AddAutoMapper(typeof(Program));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
                         .AllowAnyOrigin() // ?? ???? ??? ???? (???????? ???)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                       
                        );
});
//Edit in swaggerGen,add Authorize,edit title,add description,service and terms,contact
builder.Services.AddSwaggerGen(options=>
{
    options.SwaggerDoc("v1",new OpenApiInfo
        {
        Version="v1",
        Title="TestApi",
        TermsOfService=new Uri("https://www.google.com"),
        Contact=new OpenApiContact
        {
            Name="Dina mohamed",
            Email="dinm5862@gmail.com"
            
        }
         });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:7201/api/Account/ExternalLogin"),
                TokenUrl = new Uri("https://oauth2.googleapis.com/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "User authentication" },
                    { "profile", "User profile information" },
                    { "email", "User email address" }
                }
            }
        }
    });


    ///////////////////////

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new List<string> { "openid", "profile", "email" }
        }
    });

});

builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;
       
        
        
    }
    
    ).AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.CallbackPath = new PathString("/ExternalLoginCallback"); // يجب أن يتطابق مع redirect_uri
        options.UsePkce=true;
        
    })
    //.AddFacebook(options =>
    //{
    //    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    //    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    //})


    .AddJwtBearer(options =>
    {
        
        options.SaveToken=true;
        options.RequireHttpsMetadata=true;
        options.TokenValidationParameters=new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience=true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey=true,
            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
        options.Events=new JwtBearerEvents
        {
            OnMessageReceived = context=>{
                context.Token = context.Request.Cookies["AuthToken"] ;
                return Task.CompletedTask;
            }
        };
    } );
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseFileServer();

//Add cors

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
