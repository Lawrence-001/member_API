using AutoMapper;
using MemberTestAPI.Dtos;
using MemberTestAPI.Models;

namespace MemberTestAPI.Profiles
{
    public class MemberRegistrationProfile : Profile
    {
        public MemberRegistrationProfile()
        {
            CreateMap<CreateMemberRegistrationDto, MemberRegistration>();
            CreateMap<MemberRegistration, MemberRegistrationDto>();

        }
    }
}
