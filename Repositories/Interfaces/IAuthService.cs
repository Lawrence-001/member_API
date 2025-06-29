using MemberTestAPI.Dtos;
using MemberTestAPI.Models;

namespace MemberTestAPI.Repositories.Interfaces
{
    public interface IAuthService
    {
        //Task<AuthResponse> Register(RegisterDto registerDto);
        Task Register(RegisterDto registerDto);
        Task<AuthResponse> Login(LoginDto loginDto);
        Task ConfirmEmail(string userId, string token);
    }
}
