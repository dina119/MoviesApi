using MoviesApi.Dto;
using MoviesApi.Models;


namespace MoviesApi.Services
{
    public interface IWatchListServices
    {
        Task<WatchList>AddToWatchList(WatchList watchList);
        Task<List<WatchListDto>>GetWatchList();
        Task<WatchList> CheckMovieExist(AddToWatchListDto dto);
        Task<WatchList>  RemoveFromList(WatchList watchList);
    }
}
