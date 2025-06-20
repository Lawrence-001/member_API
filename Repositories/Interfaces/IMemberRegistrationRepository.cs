using MemberTestAPI.Models;

namespace MemberTestAPI.Repositories.Interfaces
{
    public interface IMemberRegistrationRepository
    {
        Task<MemberRegistration> CreateRegistration(MemberRegistration idpregistration);
        Task<MemberRegistration> UpdateRegistration(int RegistrationId, MemberRegistration idpregistration);
        void DeleteRegistration(int RegistrationId);
        Task<MemberRegistration?> GetRegistrationById(int RegistrationId);
        Task<IEnumerable<MemberRegistration>> GetAllRegistrations();
    }
}
