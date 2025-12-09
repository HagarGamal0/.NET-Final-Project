using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PickGo_backend.DTOs.Request;
using PickGo_backend.Models;

namespace PickGo_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RequestController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRequest(RequestCreateDTO dto)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return Unauthorized("Invalid User ID");
            }
            var user = await _unitOfWork.UserRepo.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            Console.WriteLine(user);
            // AutoMapper maps the DTO to Request model correctly
            var request = _mapper.Map<Request>(dto);

            request.CreatedAt = DateTime.UtcNow;
            request.Status = "Pending";

            // Get logged-in user ID
            request.UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            // ❗ DO NOT assign Supplier, Customer, or Packages — they no longer exist in the model
            // ❗ Request.Packages remains null until packages are added later

            await _unitOfWork.RequestRepo.AddAsync(request);
            await _unitOfWork.SaveAsync();

            return Ok(new 
            {
                Message = "Request created successfully",
                Request = request
            });
        }
    }
}
