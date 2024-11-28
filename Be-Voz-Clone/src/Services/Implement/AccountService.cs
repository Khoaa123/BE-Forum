using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Services.DTO.Account;
using Be_Voz_Clone.src.Services.DTO.User;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Be_Voz_Clone.src.Services.Implement;

public class AccountService : IAccountService
{
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private SignInManager<ApplicationUser> _signInManager;

    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, AppDbContext context,
        ICloudinaryService cloudinaryService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _mapper = mapper;
        _context = context;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<TokenObjectResponse> GetRefreshTokenAsync(TokenRequest request)
    {
        var response = new TokenObjectResponse();
        var principal = GetPrincipalFromExpiredToken(request.AccessToken);
        var username = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);
        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new NotFoundException("Invalid access token or refresh token!");
        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id),
            new("DisplayName", user.DisplayName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        foreach (var role in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, role));
        var newAccessToken = GenerateToken(authClaims);
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        response.AddMessage("Token refreshed successfully");

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
        if (user == null) throw new NotFoundException("Invalid Username");

        // Kiểm tra xem người dùng có bị ban hay không
        if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.Now)
        {
            throw new ForbiddenException("User account is banned.");
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            throw new NotFoundException("Invalid Password");

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
        new(ClaimTypes.Name, request.UserName),
        new(ClaimTypes.NameIdentifier, user.Id),
        new("DisplayName", user.DisplayName),
        new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        foreach (var role in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, role));

        response.AddMessage("Login success");
        response.Data = new TokenResponse
        {
            AccessToken = GenerateToken(authClaims),
            RefreshToken = GenerateRefreshToken()
        };
        var _RefreshTokenValidityInDays = Convert.ToInt64(_configuration["JWT:RefreshTokenValidityInDays"]);
        user.RefreshToken = response.Data.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_RefreshTokenValidityInDays);
        await _userManager.UpdateAsync(user);
        return response;
    }

    public async Task<RegisterObjectResponse> RegisterAsync(AccountRegisterRequest request, string role)
    {
        var response = new RegisterObjectResponse();
        var existingUser = await _userManager.FindByNameAsync(request.UserName);
        if (existingUser != null) throw new BadRequestException("User already exists!");
        var user = _mapper.Map<ApplicationUser>(request);
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync(role)) await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(user, role);

            response.AddMessage("Account created!");
            response.Data = _mapper.Map<AccountRegisterResponse>(user);
        }
        else
        {
            var errorDescription = result.Errors.FirstOrDefault()?.Description ?? "Unknown error";

            response.AddError($"Account creation failed: {errorDescription}");
        }

        return response;
    }

    public async Task UpdateBadgesAsync()
    {
        var users = await _context.Users.ToListAsync();
        foreach (var user in users) user.Badge = CalculateBadge(user.ReactionScore);
        await _context.SaveChangesAsync();
    }

    public async Task<string> UploadAvatarUrlAsync(string userId, IFormFile file)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new NotFoundException("User not found!");
        var avatarUrl = await _cloudinaryService.UploadAvatarAsync(file, "Voz-Avatars", userId);
        user.AvatarUrl = avatarUrl;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errorDescription = result.Errors.FirstOrDefault()?.Description ?? "Unknown error";
            throw new BadRequestException($"Failed to update avatar URL: {errorDescription}");
        }

        return avatarUrl;
    }

    public async Task<UserObjectResponse> GetUserAsync(string userId)
    {
        var response = new UserObjectResponse();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new NotFoundException("User not found!");

        response.AddMessage("Get user success");
        response.Data = _mapper.Map<UserResponse>(user);
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
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
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

    private string CalculateBadge(int reactionScore)
    {
        if (reactionScore < 0)
            return "Sắt";
        if (reactionScore < 10)
            return "Đồng";
        if (reactionScore < 50)
            return "Bạc";
        if (reactionScore < 100)
            return "Vàng";
        if (reactionScore < 200)
            return "Bạch Kim";
        if (reactionScore < 300)
            return "Kim Cương";
        if (reactionScore < 500)
            return "Cao Thủ";
        return "Thách Đấu";
    }
}