using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Dtos.Author;
using BookStoreApp.API.Dtos.Book;
using BookStoreApp.API.Dtos.User;

namespace BookStoreApp.API.Configurations
{
   public class MapperConfig : Profile
   {
      public MapperConfig()
      {
         CreateMap<AuthorCreateDto, Author>().ReverseMap();
         CreateMap<Author, AuthorReadOnlyDto>().ReverseMap();
         CreateMap<Author, AuthorUpdateDto>().ReverseMap();
         CreateMap<Book, BookReadOnlyDto>().ForMember(b => b.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
            .ReverseMap();
         CreateMap<Book, BookCreateDto>().ReverseMap();
         CreateMap<Book, BookUpdateDto>().ReverseMap();
         CreateMap<Book, BookDetailsDto>().ForMember(b => b.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
            .ReverseMap();
         CreateMap<UserDto, ApiUser>().ForMember(u => u.UserName, d => d.MapFrom(map => map.Email)).ReverseMap();
      }
   }
}
