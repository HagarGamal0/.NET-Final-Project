using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PickGo_backend;
using PickGo_backend.DTOs.User;
using PickGo_backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PickGo_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration configuration;

        public AuthController(UserManager<User> userManager, IMapper mapper, UnitOfWork unitOfWork , IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        // GET: /api/Auth/Register
        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var rolesList = await _unitOfWork.RoleRepo.GetAllAsync();

            var roles = rolesList.Select(r => r.Name).ToList();

            return Ok(new { roles });
        }

        // POST: /api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(userRegisterDTO);

            var result = await _userManager.CreateAsync(user, userRegisterDTO.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

                return BadRequest(ModelState);
            }

            if (!string.IsNullOrEmpty(userRegisterDTO.Role))
            {
                var roles = await _unitOfWork.RoleRepo.GetAllAsync();
                if (!roles.Any(r => r.Name == userRegisterDTO.Role))
                {
                    await _unitOfWork.RoleRepo.AddAsync(new IdentityRole(userRegisterDTO.Role));
                }

                await _userManager.AddToRoleAsync(user, userRegisterDTO.Role);
            }


            if (userRegisterDTO.Role == "Supplier")
            {
                await _unitOfWork.SupplierRepo.AddAsync(new Supplier
                {
                    UserId = user.Id,
                });
            }

            _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = "User registered successfully",
                userId = user.Id,
                role = userRegisterDTO.Role
            });
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userFromRequest)
        {
            //check
            var userFromDb = await _userManager.FindByNameAsync(userFromRequest.UserName);
            if (userFromDb == null)
                userFromDb = await _userManager.FindByEmailAsync(userFromRequest.UserName);

            if (userFromDb == null)
                return Unauthorized("User not found.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(userFromDb, userFromRequest.Password);
            if (!isPasswordValid)
                return Unauthorized("Invalid password.");

            // Get user roles
            var roles = await _userManager.GetRolesAsync(userFromDb);







            // Create JWT token
            var claims = new List<Claim>
    {
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, userFromDb.Id),
            new Claim(ClaimTypes.Name, userFromDb.UserName),
             new Claim(ClaimTypes.Email, userFromDb.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
