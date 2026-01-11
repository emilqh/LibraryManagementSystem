using LMS.BusinessLogicLayer.Dtos;

namespace LMS.BusinessLogicLayer.Services.Contracts
{
    public interface IMemberService
    {
        List<MemberDto> GetMembers();
        MemberDto GetMemberById(int id);
        void AddMember(CreateMemberDto createMemberDto);
        void UpdateMember(int id, UpdateMemberDto updateMemberDto);
        void DeleteMember(int id);
        List<MemberDto> SearchMember(string keyword);
    }
}
