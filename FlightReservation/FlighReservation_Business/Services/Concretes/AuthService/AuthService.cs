using AutoMapper;
using FlighReservation_Business.Services.Abstracts.IAuthService;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_Core.Entities.Concrete.Auth;
using FlightReservation_Entities.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace FlighReservation_Business.Services.Concretes.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly TokenOption _tokenOption;
        private readonly Abstracts.IAuthService.IEmailSender _emailSender;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration config, IOptions<TokenOption> tokenOption, Abstracts.IAuthService.IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _config = config;
            _tokenOption = tokenOption.Value;
            _emailSender = emailSender;
        }


        public async Task<IResult> AddAdmin(RegisterDTO register)
        {

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));


            var admin = new AppUser
            {
                UserName = register.Email,
                Email = register.Email,
                FullName = register.FullName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(admin, register.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ErrorResult(errors);
            }


            await _userManager.AddToRoleAsync(admin, "Admin");


            var token = await _userManager.GenerateEmailConfirmationTokenAsync(admin);
            var confirmationLink = $"https://localhost:7038/api/auth/confirm-email?userId={admin.Id}&token={HttpUtility.UrlEncode(token)}";
            await _emailSender.SendEmailAsync(admin.Email, "Confirm your email", $"Click to confirm: {confirmationLink}");

            return new SuccessResult("Admin uğurla əlavə edildi və təsdiq e-poçtu göndərildi.");
        }

        public async Task<IResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
                return new ErrorResult("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) 
                return new ErrorResult("Email confirmation failed");

            return new SuccessResult("Email confirmed successfully");
        }

        public async Task<IResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new ErrorResult("Bu email ilə istifadəçi tapılmadı.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return new ErrorResult("Email təsdiqlənməyib.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = $"https://localhost:7038/reset-password?email={email}&token={HttpUtility.UrlEncode(token)}";

            await _emailSender.SendEmailAsync(email, "Şifrəni sıfırla",
                $"Şifrənizi sıfırlamaq üçün link: {resetLink}");

            return new SuccessResult("Şifrə sıfırlama linki email ünvanınıza göndərildi.");
        }

        public async Task<IDataResult<AuthResponseDto>> Login(LoginDto login)
        {
            AppUser user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null)
            {
                return new ErrorDataResult<AuthResponseDto>("Istifadeci tapilmadii");
            }
            if (!await _userManager.CheckPasswordAsync(user, login.Password))
                return new ErrorDataResult<AuthResponseDto>("Şifrə səhvdir.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return new ErrorDataResult<AuthResponseDto>("Email təsdiqlənməyib.");

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOption.SecurityKey));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            JwtHeader header = new JwtHeader(signingCredentials);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("FullName",user.FullName)
            };
            foreach (var userRole in await _userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            JwtPayload payload = new JwtPayload(audience: _tokenOption.Audience, issuer: _tokenOption.Issuer, claims: claims, expires: DateTime.UtcNow.AddMinutes(_tokenOption.AccessTokenExpiration), notBefore: DateTime.UtcNow);
            JwtSecurityToken token = new JwtSecurityToken(header, payload);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string jwt = tokenHandler.WriteToken(token);
            AuthResponseDto dto = new AuthResponseDto();
            dto.Token = jwt;
            dto.Expiration = DateTime.UtcNow.AddMinutes(_tokenOption.AccessTokenExpiration);

            return new SuccessDataResult<AuthResponseDto>(dto, "fsf");
        }

        public async Task<IResult> Register(RegisterDTO register)
        {
            var user = _mapper.Map<AppUser>(register);
            var resultUser = await _userManager.CreateAsync(user, register.Password);
            if (!resultUser.Succeeded)
            {
                return new ErrorResult("Not register");
            }
            string roleName = register.Role.ToString();

            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));

            var resultRole = await _userManager.AddToRoleAsync(user, roleName);
            if (!resultRole.Succeeded)
            {
                return new ErrorResult("Role not added");
            }

            return new SuccessResult("Succeeded register");
        }

        public async Task<IResult> ResetPassword(ResetPasswordDTO reset)
        {
            var user = await _userManager.FindByEmailAsync(reset.Email);
            if (user == null)
                return new ErrorResult("Bu email ilə istifadəçi tapılmadı.");

            var result = await _userManager.ResetPasswordAsync(user, reset.Token, reset.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ErrorResult($"Şifrə sıfırlanmadı: {errors}");
            }

            return new SuccessResult("Şifrəniz uğurla sıfırlandı.");
        }
    }
}
