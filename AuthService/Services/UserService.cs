using AuthService.Data;
using AuthService.Models;
using AuthService.Models.Dtos;
using AuthService.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services
{
    public class UserService : IUser
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwt _jwtService;

        public UserService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager , IJwt jwtService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;

        }
        public async Task<bool> AssignUserRoles(string Email, string RoleName)
        {
            var user = await _context.ApplicationUsers.Where(x => x.Email.ToLower() == Email.ToLower()).FirstOrDefaultAsync();
            //does user exist
            if(user == null)
            {
                return false;
            }
            else
            {
                //does the role exist
                if(!_roleManager.RoleExistsAsync(RoleName).GetAwaiter().GetResult())
                {
                   await _roleManager.CreateAsync(new IdentityRole(RoleName));
                }
                //assign the user the role
                await _userManager.AddToRoleAsync(user, RoleName);
                return true;
            }
        }

        public async Task<ApplicationUser> GetUserById(string Id)
        {
            return await _context.ApplicationUsers.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<LoginResponseDto> LoginUser(LoginRequestDto loginRequestDto)
        {
            var user = await _context.ApplicationUsers.Where(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower()).FirstOrDefaultAsync();
            //compare hashed password with plain text
            var isValid = _userManager.CheckPasswordAsync(user, loginRequestDto.Password).GetAwaiter().GetResult();

            if (!isValid || user == null)
            {
                return new LoginResponseDto();
            }
            var loggeduser = _mapper.Map<UserResponseDto>(user);
            //to get roles for  specific user
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);  // token requires user of type application user
            var response = new LoginResponseDto()
            {
                User = loggeduser,
                //Token = "Coming soon.."
                Token = token
            };
            return response;
        }
       

        public async Task<string> RegisterUser(RegisterUserDto userDto)
        {
            try {
                var user = _mapper.Map<ApplicationUser>(userDto);
                //create user
                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                {
                    return string.Empty;
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception ex) {
                return ex.Message;
            }
            
        }

        
        //to assign a user a role after a successfull registration
        //public async Task<string> RegisterUser(RegisterUserDto userDto)
        //{
        //    try
        //    {
        //        var user = _mapper.Map<ApplicationUser>(userDto);
        //        //create user
        //        var result = await _userManager.CreateAsync(user, userDto.Password);
        //        if (result.Succeeded)
        //        {
        //            //does the role exist
        //            if (!_roleManager.RoleExistsAsync(userDto.Role).GetAwaiter().GetResult())
        //            {
        //                await _roleManager.CreateAsync(new IdentityRole(userDto.Role));
        //            }
        //            //assign the user the role
        //            await _userManager.AddToRoleAsync(user, userDto.Role); 
        //            return string.Empty;
        //        }
        //        else
        //        {
        //            return result.Errors.FirstOrDefault().Description;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }

        //}

    }
}
