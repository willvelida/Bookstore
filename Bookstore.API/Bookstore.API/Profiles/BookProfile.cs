using AutoMapper;

namespace Bookstore.API.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Models.BookForUpdateDTO, Models.Book>();
        }
    }
}
