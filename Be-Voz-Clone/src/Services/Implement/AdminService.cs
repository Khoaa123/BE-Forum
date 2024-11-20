using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Services.Common;
using Be_Voz_Clone.src.Services.DTO.Account;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Core.Helper;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AdminService(AppDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<BaseResponse> BanUserAsync(string userId)
        {
            var response = new BaseResponse();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.LockoutEnd = DateTimeOffset.MaxValue;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                response.AddMessage("User banned successfully");
            }
            else
            {
                response.AddError("Failed to ban user");
            }

            return response;
        }

        public async Task<AccountListObjectResponse> GetAllUserAsync(int pageNumber, int pageSize)
        {
            var response = new AccountListObjectResponse();

            var users = await _userManager.Users.ToListAsync();

            var filteredUsers = new List<ApplicationUser>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(Roles.User) || roles.Contains(Roles.Mod))
                {
                    filteredUsers.Add(user);
                }
            }

            var totalUsers = filteredUsers.Count;
            var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

            var usersPerPage = filteredUsers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            response.Data = _mapper.Map<List<AccountResponse>>(usersPerPage);
            response.TotalPages = totalPages;

            if (usersPerPage.Count == 0)
            {
                response.AddMessage("No users found");
            }
            else
            {
                response.AddMessage("Users retrieved successfully");
            }

            return response;
        }

        public async Task<BaseResponse> UnbanUserAsync(string userId)
        {
            var response = new BaseResponse();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found!");
            }

            user.LockoutEnd = null;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                response.AddMessage("User unbanned successfully");
            }
            else
            {
                response.AddError("Failed to unban user");
            }

            return response;
        }
    }
}