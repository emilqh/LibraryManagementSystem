using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.DataAccessLayer.Entities
{
    //- Id (int), - Title(string), - Author(string), - ISBN(string)
    //- PublishedYear(int), - CategoryId(int), - IsAvailable(bool)


    public class Book
    {       
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public string? ISBN { get; set; }
        public int PublishedYear { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
