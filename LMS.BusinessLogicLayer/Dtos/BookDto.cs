using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.BusinessLogicLayer.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public string? ISBN { get; set; }
        public int PublishedYear { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class CreateBookDto
    {
        public required string Title { get; set; }
        public required string Author { get; set; }
        public string? ISBN { get; set; }
        public int PublishedYear { get; set; }
        public int CategoryId { get; set; }
    }

    public class UpdateBookDto
    {
        public required string Title { get; set; }
        public required string Author { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
