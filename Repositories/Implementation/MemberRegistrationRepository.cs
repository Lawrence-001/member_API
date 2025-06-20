using MemberTestAPI.Data;
using MemberTestAPI.Models;
using MemberTestAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemberTestAPI.Repositories.Implementation
{
    public class MemberRegistrationRepository : IMemberRegistrationRepository
    {
        private readonly AppDbContext _context;

        public MemberRegistrationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<MemberRegistration> CreateRegistration(MemberRegistration memberRegistration)
        {
            _context.MemberRegistrations.Add(memberRegistration);
            await _context.SaveChangesAsync();
            return memberRegistration;
        }

        public void DeleteRegistration(int id)
        {
            var recordToDelete = _context.MemberRegistrations.FirstOrDefaultAsync(x => x.RegistrationId == id);
            if (recordToDelete != null)
            {
                _context.Remove(recordToDelete);
                _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MemberRegistration>> GetAllRegistrations()
        {
            var registrations = await _context.MemberRegistrations.ToListAsync();
            return registrations;
        }

        public async Task<MemberRegistration?> GetRegistrationById(int registrationId)
        {
            var reg = await _context.MemberRegistrations.FirstOrDefaultAsync(x => x.RegistrationId == registrationId);
            return reg;
        }

        public async Task<MemberRegistration> UpdateRegistration(int registrationId, MemberRegistration idpregistration)
        {
            var reg_update = await _context.MemberRegistrations.FirstOrDefaultAsync(x => x.RegistrationId == registrationId);
            if (reg_update != null)
            {
                _context.Update(reg_update);
                await _context.SaveChangesAsync();
                return reg_update;
            }
            return null;
        }
    }
}
