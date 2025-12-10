using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using PickGo_backend.DTOs.Request;
using PickGo_backend.DTOs.Package;
using PickGo_backend.Models;
using System.Security.Claims;

namespace PickGo_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Supplier")] // supplier = business owner
    public class RequestController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RequestController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // --------------------------------------------------------
        // CREATE Request
        // --------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateRequest(RequestCreateDTO dto)
        {
            var supplierId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var request = _mapper.Map<Request>(dto);
            request.SupplierId = supplierId;
            request.CreatedAt = DateTime.UtcNow;
            request.Status = RequestStatus.Pending;

            await _unitOfWork.RequestRepo.AddAsync(request);
            await _unitOfWork.SaveAsync();

            return Ok(_mapper.Map<RequestReadDTO>(request));
        }

        // --------------------------------------------------------
        // GET all Requests for logged-in Supplier
        // --------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll(
            string? search,
            string? sortBy = "date",
            string sortDir = "desc")
        {
            var supplierId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var requests = await _unitOfWork.RequestRepo.GetBySupplierAsync(supplierId);

            // Filtering
            if (!string.IsNullOrEmpty(search))
                requests = requests.Where(r =>
                    r.Source.Contains(search) ||
                    r.Packages.Any(p => p.Description.Contains(search))
                ).ToList();

            // Sorting
            requests = sortBy switch
            {
                "status" => (sortDir == "asc"
                        ? requests.OrderBy(r => r.Status)
                        : requests.OrderByDescending(r => r.Status)).ToList(),

                _ => (sortDir == "asc"
                        ? requests.OrderBy(r => r.CreatedAt)
                        : requests.OrderByDescending(r => r.CreatedAt)).ToList()
            };

            return Ok(_mapper.Map<List<RequestReadDTO>>(requests));
        }

        // --------------------------------------------------------
        // GET request details
        // --------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var request = await _unitOfWork.RequestRepo.GetFullRequestAsync(id);
            if (request == null) return NotFound();

            return Ok(_mapper.Map<RequestReadDTO>(request));
        }

        // --------------------------------------------------------
        // UPDATE Request
        // --------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RequestUpdateDTO dto)
        {
            var request = await _unitOfWork.RequestRepo.GetAsync(id);
            if (request == null) return NotFound();

            _mapper.Map(dto, request);

            await _unitOfWork.SaveAsync();
            return Ok(_mapper.Map<RequestReadDTO>(request));
        }

        // --------------------------------------------------------
        // DELETE Request
        // --------------------------------------------------------
        [HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    var request = await _unitOfWork.RequestRepo.GetByIdAsync(id);
    if (request == null)
        return NotFound();

    _unitOfWork.RequestRepo.Delete(request);
    await _unitOfWork.SaveAsync();

    return Ok(new { message = "Request deleted" });
}


        // --------------------------------------------------------
        // ASSIGN Rider
        // --------------------------------------------------------
        [HttpPut("{requestId}/assign/{courierId}")]
        public async Task<IActionResult> AssignRider(int requestId, int courierId)
        {
            var request = await _unitOfWork.RequestRepo.GetAsync(requestId);
            if (request == null) return NotFound("Request not found");

            if (request.Status != RequestStatus.Pending)
                return BadRequest("Only pending requests can be assigned.");

            var courier = await _unitOfWork.CourierRepo.GetAsync(courierId);
            if (courier == null || !courier.IsAvailable)
                return BadRequest("Courier unavailable");

            // Assign logic
            foreach (var pkg in request.Packages)
                pkg.CourierID = courierId;

            request.Status = RequestStatus.Assigned;

            await _unitOfWork.SaveAsync();
            return Ok(new { message = "Rider assigned successfully" });
        }
    }
}
