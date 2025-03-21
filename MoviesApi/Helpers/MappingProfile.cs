﻿using AutoMapper;
using MoviesApi.Dto;
using MoviesApi.Models;

namespace MoviesApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie,MovieDetailsDto>();
            CreateMap<RateDto,Rate>();
            CreateMap<AddToWatchListDto,WatchList>();
            CreateMap<CreateMoviesDto,Movie>()
                .ForMember(src=>src.PosterUrl,opt=>opt.Ignore());
        }
    }
}
