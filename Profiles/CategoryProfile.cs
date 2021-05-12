using AutoMapper;
using Book_Store.Models;
using Book_Store.ViewModel;

namespace Book_Store.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryModel, GetCategoryViewModel>();
            CreateMap<PostCategoryViewModel, CategoryModel>();
        }
    }
}
