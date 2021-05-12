using AutoMapper;
using Book_Store.Models;
using Book_Store.ViewModel;

namespace Book_Store.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorModel, GetAuthorViewModel>();
            CreateMap<PostAuthorViewModel, AuthorModel>();
        }
    }
}
