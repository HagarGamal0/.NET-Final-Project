using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PickGo_backend.DTOs.Courier;
using PickGo_backend.DTOs.Supplier;
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
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<User> userManager, IMapper mapper, UnitOfWork unitOfWork, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        // -------------------- Registration --------------------

        [HttpPost("Register/Supplier")]
        public async Task<IActionResult> RegisterSupplier(SupplierRegisterDTO dto)
        {
            // 1. Create Identity user
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Address = dto.Address,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // 2. Add role if not exists
            if (!await _userManager.IsInRoleAsync(user, "Supplier"))
                await _userManager.AddToRoleAsync(user, "Supplier");

            // 3. Create Supplier with FK to User
            var supplier = new Supplier
            {
                UserId = user.Id,
                ShopName = dto.ShopName,
                IsDeleted = false
            };
            await _unitOfWork.SupplierRepo.AddAsync(supplier);
            await _unitOfWork.SaveAsync();

            return Ok(new { userId = user.Id, role = "Supplier" });
        }

        [HttpPost("Register/Courier")]
        public async Task<IActionResult> RegisterCourier([FromBody] CourierRegisterDTO dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Address = dto.Address,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _userManager.IsInRoleAsync(user, "Courier"))
                await _userManager.AddToRoleAsync(user, "Courier");

            var courier = new Courier
            {
                UserId = user.Id,
                VehicleType = dto.VehicleType,
                LicenseNumber = dto.LicenseNumber,
                MaxWeight = dto.MaxWeight,
                IsAvailable = true,
                IsOnline = false,
                Rating = 0,
                Status = string.IsNullOrEmpty(dto.Status) ? "Pending" : dto.Status

            };

            await _unitOfWork.CourierRepo.AddAsync(courier);
            await _unitOfWork.SaveAsync();

            return Ok(new { userId = user.Id, role = "Courier" });
        }


        // POST: api/Auth/Register/Admin
        [HttpPost("Register/Admin")]
        public async Task<IActionResult> RegisterAdmin(UserRegisterDTO dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,

            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
                await _userManager.AddToRoleAsync(user, "Admin");

            return Ok(new { userId = user.Id, role = "Admin" });
        }







        // -------------------- Login --------------------

        [HttpPost("Login/Supplier")]
        public async Task<IActionResult> LoginSupplier(UserLoginDTO dto)
        {
            return await LoginUser(dto, "Supplier");
        }

        [HttpPost("Login/Courier")]
        public async Task<IActionResult> LoginCourier(UserLoginDTO dto)
        {
            return await LoginUser(dto, "Courier");
        }

        [HttpPost("Login/Admin")]
        public async Task<IActionResult> LoginAdmin(UserLoginDTO dto)
        {
            return await LoginUser(dto, "Admin");
        }

        private async Task<IActionResult> LoginUser(UserLoginDTO dto, string role)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName)
                       ?? await _userManager.FindByEmailAsync(dto.UserName);

            if (user == null)
                return Unauthorized("User not found.");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid password.");

            if (!await _userManager.IsInRoleAsync(user, role))
                return Unauthorized($"User is not a {role}.");

            if (role == "Courier")
            {
                var courier = (await _unitOfWork.CourierRepo.GetAllAsync())
                              .FirstOrDefault(c => c.UserId == user.Id);

                if (courier == null) return Unauthorized("Courier record not found.");
                if (courier.Status != "Approved")
                    return Unauthorized($"Your registration is {courier.Status}. Admin approval required.");
            }

            // JWT creation
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
new Claim(ClaimTypes.Role, role)  
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                role
            });
        }
    }
}