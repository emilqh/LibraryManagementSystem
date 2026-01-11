using LMS.BusinessLogicLayer.Dtos;
using LMS.BusinessLogicLayer.Services.Contracts;
using LMS.DataAccessLayer;
using LMS.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.BusinessLogicLayer.Services
{
    public class MemberManager : IMemberService
    {
        private readonly MemberRepository _memberRepository;

        public MemberManager()
        {
            _memberRepository = new MemberRepository();
        }

        public List<MemberDto> GetMembers()
        {
            return _memberRepository
                .GetAll()
                .Select(MapToDto)
                .ToList();
        }

        public MemberDto? GetMemberById(int id)
        {
            Member member = _memberRepository.GetById(id);
            return member == null ? null : MapToDto(member);
        }

        public void AddMember(CreateMemberDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new Exception("Full name is required.");

            Member member = new Member
            {
                FullName = dto.FullName.Trim(),
                Email = dto.Email?.Trim(),
                PhoneNumber = dto.PhoneNumber?.Trim(),
                MembershipDate = DateTime.Now,
                IsActive = true
            };

            _memberRepository.Add(member);
        }

        public void UpdateMember(int id, UpdateMemberDto dto)
        {
            Member member = _memberRepository.GetById(id);
            if (member == null)
                throw new Exception("Member not found.");

            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new Exception("Full name is required.");

            member.FullName = dto.FullName.Trim();
            member.Email = dto.Email?.Trim();
            member.PhoneNumber = dto.PhoneNumber?.Trim();
            member.IsActive = dto.IsActive;

            _memberRepository.Update(member);
        }

        public void DeleteMember(int id)
        {
            Member member = _memberRepository.GetById(id);
            if (member == null)
                throw new Exception("Member not found.");

            _memberRepository.Delete(id);
        }

        public List<MemberDto> SearchMember(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<MemberDto>();

            return _memberRepository
                .Search(keyword)
                .Select(MapToDto)
                .ToList();
        }

        //One centralized mapping method to use in other methods
        private MemberDto MapToDto(Member member)
        {
            return new MemberDto
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                MembershipDate = member.MembershipDate,
                IsActive = member.IsActive
            };
        }
    }
}
