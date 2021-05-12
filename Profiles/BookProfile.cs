using AutoMapper;
using Book_Store.Models;
using Book_Store.ViewModel;
using System.Collections.Generic;

namespace Book_Store.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookModel, GetBookViewModel>();
            CreateMap<PostBookViewModel, BookModel>();
        }
    }
}
