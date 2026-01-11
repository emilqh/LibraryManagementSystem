using LMS.BusinessLogicLayer.Dtos;

namespace LMS.BusinessLogicLayer.Services.Contracts
{
    public interface ICategoryService
    {
        List<CategoryDto> GetCategories();
        CategoryDto GetCategoryById(int id);
        void AddCategory(CreateCategoryDto createCategoryDto);
        void UpdateCategory(int id, UpdateCategoryDto updateCategoryDto);
        void DeleteCategory(int id);
        List<CategoryDto> SearchCategory(string keyword);
    }


}
