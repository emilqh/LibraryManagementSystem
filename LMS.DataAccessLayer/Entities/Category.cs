using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.DataAccessLayer.Entities
{
    //- Id (int), - Name(string), - Description(string)
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
