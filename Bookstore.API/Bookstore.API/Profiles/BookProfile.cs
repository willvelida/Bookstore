using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

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
