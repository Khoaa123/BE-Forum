using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Services.DTO.Account;
using Be_Voz_Clone.src.Shared.Core.Enums;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Be_Voz_Clone.src.Services.Implement
{
    public class AccountService : IAccountService
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private IMapper _mapper;
        private AppDbContext _context;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _context = context;
        }
        public async Task<TokenObjectResponse> GetRefreshTokenAsync(TokenRequest request)
        {
            TokenObjectResponse response = new();

            var principal = GetPrincipalFromExpiredToken(request.AccessToken);

            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new NotFoundException("Invalid access token or refresh token!");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim("DisplayName", user.DisplayName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var newAccessToken = GenerateToken(authClaims);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            response.StatusCode = ResponseCode.OK;
            response.Message = "Success";
            response.Data = new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return response;
        }

        public async Task<TokenObjectResponse> LoginAsync(AccountLoginRequest request)
        {
            var response = new TokenObjectResponse();

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new NotFoundException("Invalid Username");
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new NotFoundException("Invalid Password");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("DisplayName", user.DisplayName),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            response.StatusCode = ResponseCode.OK;
            response.Message = "Success";
            response.Data = new TokenResponse
            {
                AccessToken = GenerateToken(authClaims),
                RefreshToken = GenerateRefreshToken(),
            };

            var _RefreshTokenValidityInDays = Convert.ToInt64(_configuration["JWT:RefreshTokenValidityInDays"]);
            user.RefreshToken = response.Data.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_RefreshTokenValidityInDays);
            await _userManager.UpdateAsync(user);

            return response;
        }

        public async Task<RegisterObjectResponse> RegisterAsync(AccountRegisterRequest request, string role)
        {
            RegisterObjectResponse response = new();

            var existingUser = await _userManager.FindByNameAsync(request.UserName);

            if (existingUser != null)
            {
                throw new BadRequestException("User already exists!");
            }

            var user = _mapper.Map<ApplicationUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                response.StatusCode = ResponseCode.CREATED;
                response.Message = "Account created!";
                response.Data = _mapper.Map<AccountRegisterResponse>(user);
            }
            else
            {
                var errorDescription = result.Errors.FirstOrDefault()?.Description ?? "Unknown error";
                response.StatusCode = ResponseCode.BADREQUEST;
                response.Message = $"Account creation failed: {errorDescription}";
            }

            return response;
        }


        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var secret = _configuration["JWT:Secret"] ?? "";
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var expirationTimeUtc = DateTime.UtcNow.AddHours(1);
            var localTimeZone = TimeZoneInfo.Local;
            var expirationTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(expirationTimeUtc, localTimeZone);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = expirationTimeInLocalTimeZone,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        private ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var secret = _configuration["JWT:Secret"] ?? "";
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            var range = RandomNumberGenerator.Create();
            range.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
