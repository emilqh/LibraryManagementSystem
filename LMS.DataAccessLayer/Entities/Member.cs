using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.DataAccessLayer.Entities
{
    //- Id (int), - FullName(string), - Email(string), - PhoneNumber(string)
    //- MembershipDate(DateTime), - IsActive(bool)
    public class Member
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime MembershipDate { get; set; }
        public bool IsActive { get; set; }
    }
}
