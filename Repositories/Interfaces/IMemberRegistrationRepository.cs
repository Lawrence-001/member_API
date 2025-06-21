using MemberTestAPI.Models;

namespace MemberTestAPI.Repositories.Interfaces
{
    public interface IMemberRegistrationRepository
    {
        Task<MemberRegistration> CreateRegistration(MemberRegistration memberRegistration);
        Task<MemberRegistration> UpdateRegistration(int RegistrationId, MemberRegistration memberRegistration);
        Task<bool> DeleteRegistration(int registrationId);
        Task<MemberRegistration> GetRegistrationById(int registrationId);
        Task<IEnumerable<MemberRegistration>> GetAllRegistrations();
    }
}
