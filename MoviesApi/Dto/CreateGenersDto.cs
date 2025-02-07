using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dto
{
    public class CreateGenersDto
    {
        [MaxLength(100)]
        public string Name { get;set;}
    }
}
