using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoviesApi.Models;
using MoviesApi.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//get connection string from app setting
var connection_string=builder.Configuration.GetConnectionString("DefultConnection");
//add connection string 
builder.Services.AddDbContext<ApplicationDbContext>(
    options=>options.UseSqlServer(connection_string)
    );

builder.Services.AddControllers();
builder.Services.AddTransient<IGenresService,GenresService>();
builder.Services.AddTransient<IMoviesService,MoviesService>();
builder.Services.AddAutoMapper(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
    options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme

        {
        Name="Authorization",
        Type=SecuritySchemeType.ApiKey,
        Scheme="Bearer",
        BearerFormat="JWT",
        In=ParameterLocation.Header,
        Description="Enter your JWT key",
        });
});
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Add cors
app.UseCors(c=>c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
