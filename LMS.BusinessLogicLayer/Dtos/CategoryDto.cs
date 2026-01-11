using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.BusinessLogicLayer.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }

    public class CreateCategoryDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateCategoryDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
