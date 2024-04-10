using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Dtos.Author;

namespace BookStoreApp.API.Configurations
{
   public class MapperConfig : Profile
   {
      public MapperConfig()
      {
         CreateMap<AuthorCreateDto, Author>().ReverseMap();
         CreateMap<Author, AuthorReadOnlyDto>().ReverseMap();
         CreateMap<Author, AuthorUpdateDto>().ReverseMap();
      }
   }
}
