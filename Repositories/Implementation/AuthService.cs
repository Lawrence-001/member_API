using MemberTestAPI.Dtos;
using MemberTestAPI.Models;
using MemberTestAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MemberTestAPI.Repositories.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailSender emailSender, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task Register(RegisterDto registerDto)
        {
            var emailExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (emailExists is not null)
            {
                throw new Exception($"{registerDto.Email} already exists.");
            }

            //foreach (var role in registerDto.Roles)
            //{
            //    if (!await _roleManager.RoleExistsAsync(role))
            //    {
            //        var roleResult = await _roleManager.CreateAsync(new IdentityRole(role.));
            //        if (!roleResult.Succeeded)
            //        {
            //            throw new Exception($"Failed to create role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            //        }
            //        await _userManager.AddToRoleAsync(user, role);
            //    }
            //}

            foreach (var role in registerDto.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    throw new Exception($"Role '{role}' does not exist, create it first");
                }
            }

            var user = new ApplicationUser
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception($"Registration failed {string.Join(", ", result.Errors.Select(x => x.Description))}");
            }

            foreach (var role in registerDto.Roles.Distinct())
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            try
            {
                var baseUrl = _configuration["BaseUrl"];
                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    _logger.LogError("BaseUrl is not configured in appsettings.json!");
                    throw new Exception("Server configuration error. Please contact support.");
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = Uri.EscapeDataString(token);

                var confirmUrl = $"{baseUrl}/confirm-email?userId={user.Id}&token={encodedToken}";
                var body = $@"
        <p>Hello {user.FullName},</p>
        <p>Please confirm your email by clicking <a href='{confirmUrl}'>here</a>.</p>";

                await _emailSender.SendEmail(user.Email, "Please confirm your email", body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate email confirmation token or send email");

                // Rollback user creation if email sending fails
                await _userManager.DeleteAsync(user);

                throw new Exception($"Registration failed: {ex.Message}");

            }

            //var token = await GenerateJwtToken(user);
            //return new AuthResponse
            //{
            //    Email = user.Email,
            //    FullName = user.FullName,
            //    Token = token
            //};
        }

        //if (!result.Succeeded)
        //{

        //    // Rollback user creation if email sending fails
        //    await _userManager.DeleteAsync(user);
        //    throw new Exception($"Registration failed {string.Join(", ", result.Errors.Select(x => x.Description))}");
        //}
        //var token = await GenerateJwtToken(user);
        //return new AuthResponse
        //{
        //    Email = user.Email,
        //    FullName = user.FullName,
        //    Token = token
        //};


        public async Task<AuthResponse> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new Exception($"User with {loginDto.Email} does not exist.");
            }
            var userLogin = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
            if (userLogin.Succeeded)
            {
                var token = await GenerateJwtToken(user);
                return new AuthResponse
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    Token = token
                };
            }
            throw new Exception("Invalid credentials");
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var secret = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);

            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("FullName", user.FullName)
        };

            // Add roles as claims
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Add any custom claims
            claims.AddRange(userClaims);

            var creds = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public async Task ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new Exception("Email confirmation failed.");
            }
        }
    }
}
