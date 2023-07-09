using AutoMapper;
using BookStore.Models;
using BookStore.Models.Dto;

namespace BookStore
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Books, BooksDto>().ReverseMap();
        }
    }
}
