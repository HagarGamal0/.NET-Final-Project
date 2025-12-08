using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Security.Claims;
using PickGo_backend;                     
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

        [HttpPost]
        public async Task<IActionResult> CreateRequest(RequestCreateDTO dto)
        {
            var request = _mapper.Map<Request>(dto);

            request.CreatedAt = DateTime.UtcNow;
            request.Status = "Pending";
            request.SupplierId = 0;
            request.Customer = null;
            request.Packages = null;

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
