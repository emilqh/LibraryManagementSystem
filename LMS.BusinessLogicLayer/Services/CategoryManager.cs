using LMS.BusinessLogicLayer.Dtos;
using LMS.BusinessLogicLayer.Services.Contracts;
using LMS.DataAccessLayer;
using LMS.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace LMS.BusinessLogicLayer.Services
{
    public class CategoryManager : ICategoryService
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryManager()
        {
            _categoryRepository = new CategoryRepository();
        }

        public List<CategoryDto> GetCategories()
        {
            List<Category> categories = _categoryRepository.GetAll();
            List<CategoryDto> result = new List<CategoryDto>();

            foreach (Category category in categories)
            {
                result.Add(MapToDto(category));
            }

            return result;
        }

        public CategoryDto GetCategoryById(int id)
        {
            Category category = _categoryRepository.GetById(id);

            if (category is null)
            {
                return null;
            }

            return MapToDto(category);
        }

        public void AddCategory(CreateCategoryDto createCategoryDto)
        {
            //[Id:5][Name:25][Description:50]

            if (string.IsNullOrWhiteSpace(createCategoryDto.Name))
                throw new Exception("Category name is required.");

            if (createCategoryDto.Name.Length > 25)
                throw new Exception("Category name cannot exceed 25 characters.");

            if (createCategoryDto.Description != null && createCategoryDto.Description.Length > 50)
                throw new Exception("Category description cannot exceed 50 characters.");

            _categoryRepository.Add(new Category
            {
                Name = createCategoryDto.Name.Trim(),
                Description = createCategoryDto.Description?.Trim()
            });
        }

        public void UpdateCategory(int id, UpdateCategoryDto updateCategoryDto)
        {
            if (updateCategoryDto is null)
                throw new ArgumentNullException(nameof(updateCategoryDto));

            if (string.IsNullOrWhiteSpace(updateCategoryDto.Name))
                throw new Exception("Category name is required.");

            if (updateCategoryDto.Name.Length > 25)
                throw new Exception("Category name cannot exceed 25 characters.");

            if (updateCategoryDto.Description != null &&
                updateCategoryDto.Description.Length > 50)
                throw new Exception("Category description cannot exceed 50 characters.");

            Category category = _categoryRepository.GetById(id);

            if (category is null)
            {
                return;
            }

            category.Name = updateCategoryDto.Name.Trim();
            category.Description = updateCategoryDto.Description?.Trim();

            _categoryRepository.Update(category);
        }

        public void DeleteCategory(int id)
        {
            Category category = _categoryRepository.GetById(id);

            if (category is null)
            {
                return;
            }

            _categoryRepository.Delete(id);
        }

        public List<CategoryDto> SearchCategory(string keyword)
        {
            List<CategoryDto> result = new List<CategoryDto>();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return result;
            }

            List<Category> categories = _categoryRepository.Search(keyword);

            foreach (Category category in categories)
            {
                result.Add(MapToDto(category));
            }

            return result;
        }

        //One centralized mapping method to use in other methods
        private CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
