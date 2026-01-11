namespace LMS.BusinessLogicLayer.Dtos
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime MembershipDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateMemberDto
    {
        public required string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class UpdateMemberDto
    {
        public required string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
