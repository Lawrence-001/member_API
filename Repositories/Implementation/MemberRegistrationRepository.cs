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

        public async Task<bool> DeleteRegistration(int id)
        {
            var recordToDelete = _context.MemberRegistrations.FirstOrDefaultAsync(x => x.RegistrationId == id);
            if (recordToDelete != null)
            {
                _context.Remove(recordToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new Exception($"Registration with Id {id} does not exist.");
        }

        public async Task<IEnumerable<MemberRegistration>> GetAllRegistrations()
        {
            var registrations = await _context.MemberRegistrations.ToListAsync();
            return registrations;
        }

        public async Task<MemberRegistration> GetRegistrationById(int registrationId)
        {
            var reg = await _context.MemberRegistrations.FirstOrDefaultAsync(x => x.RegistrationId == registrationId);
            if (reg != null)
            {
                return reg;
            }
            throw new Exception($"Registration with Id {registrationId} doe not exist.");
        }

        public async Task<MemberRegistration> UpdateRegistration(int registrationId, MemberRegistration memberRegistration)
        {
            var reg_update = await _context.MemberRegistrations.FirstOrDefaultAsync(x => x.RegistrationId == registrationId);
            if (reg_update != null)
            {
                reg_update.FullName = memberRegistration.FullName;
                reg_update.NationalId = memberRegistration.NationalId;
                reg_update.DOB = memberRegistration.DOB;
                reg_update.Gender = memberRegistration.Gender;
                reg_update.Region = memberRegistration.Region;
                reg_update.District = memberRegistration.District;

                await _context.SaveChangesAsync();
                return reg_update;
            }
            throw new Exception($"Registration with Id {registrationId} does not exist.");
        }
    }
}
