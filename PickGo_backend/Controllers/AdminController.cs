using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PickGo_backend.DTOs;
using PickGo_backend.DTOs.Admin;
using PickGo_backend.Models;
using PickGo_backend.Models.Enums;

namespace PickGo_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public AdminController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // -------------------- Get all pending Couriers --------------------
        [HttpGet("PendingCouriers")]
        public async Task<IActionResult> GetPendingCouriers()
        {
            var pendingCouriers = (await _unitOfWork.CourierRepo.GetAllAsync())
                                  .Where(c => c.Status == CourierStatus.Pending)
                                  .Select(c => new
                                  {
                                      id = c.Id,
                                      name = c.User.UserName, 
                                      email = c.User.Email, 
                                      phone = c.User.PhoneNumber,
                                      passWord =c.User.PasswordHash,
                                      vehicleType = c.VehicleType.ToString(), 
                                      licenseNumber = c.LicenseNumber,
                                      maxWeight = c.MaxWeight,
                                      photoUrl = c.PhotoUrl,
                                      licensePhotoFront = c.LicensePhotoFront,
                                      licensePhotoBack = c.LicensePhotoBack,
                                      appliedAt = c.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss"), 
                                      address = c.User.Address,
                                      birthDate = c.User.BirthDate,
                                      gender = c.User.Gender.ToString() 
                                  })
                                  .ToList();

            if (!pendingCouriers.Any())
                return NotFound(new { message = "No pending couriers found." });

            return Ok(pendingCouriers);
        }

        // -------------------- Approve Courier --------------------
        [HttpPost("ApproveCourier/{courierId}")]
        public async Task<IActionResult> ApproveCourier(int courierId)
        {
            var courier = await _unitOfWork.CourierRepo.GetByIdAsync(courierId);
            if (courier == null) return NotFound("Courier not found.");

            courier.Status = CourierStatus.Approved;
            _unitOfWork.CourierRepo.Update(courier);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Courier approved successfully." });
        }

        // -------------------- Reject Courier --------------------
        [HttpPost("RejectCourier/{courierId}")]
        public async Task<IActionResult> RejectCourier(int courierId, [FromBody] RejectCourierDto dto)
        {
            var courier = await _unitOfWork.CourierRepo.GetByIdAsync(courierId);
            if (courier == null) return NotFound("Courier not found.");

            courier.Status = CourierStatus.Rejected;
            courier.RejectionReason = dto.Reason;

            _unitOfWork.CourierRepo.Update(courier);
            await _unitOfWork.SaveAsync();

            // TODO: Notify courier about rejection and reason
            // await _notificationService.SendAsync(courier.UserId, $"Your account was rejected: {dto.Reason}");

            return Ok(new { message = "Courier rejected successfully." });
        }
        // -------------------- Get Online Couriers --------------------
        [HttpGet("OnlineCouriers")]
        public async Task<IActionResult> GetOnlineCouriers()
        {
            var onlineCouriers = (await _unitOfWork.CourierRepo.GetAllAsync())
                                 .Where(c => c.IsOnline && c.Status == CourierStatus.Approved)
                                 .ToList();

            return Ok(onlineCouriers);
        }



       

        [HttpGet("DashboardStats")]
        public async Task<IActionResult> GetStats()
        {
            var packages = await _unitOfWork.PackageRepo.GetAllAsync();

            var couriers = await _unitOfWork.CourierRepo.GetAllAsync();

            var invoices = await _unitOfWork.InvoiceRepo.GetAllAsync();

            var stats = new AdminDashboardStatsDto
            {
                TotalOrders = packages.Count(),
                FailedDeliveries = packages.Count(p => p.Status == PackageStatus.Failed),
                ActiveCouriers = couriers.Count(c => c.IsOnline && c.Status == CourierStatus.Approved),
                TotalRevenue = invoices.Sum(i => i.Cost)
            };

            return Ok(stats);
        }



        // -------------------- Manage Users --------------------
        [HttpPost("BlockUser/{userId}")]
        public async Task<IActionResult> BlockUser(string userId)
        {
            var user = await _unitOfWork.UserRepo.GetByIdAsync(userId);
            if (user == null) return NotFound(new { message = "User not found." });

            user.LockoutEnd = DateTimeOffset.MaxValue; // ASP.NET Identity way to block
            _unitOfWork.UserRepo.Update(user);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "User blocked successfully." });
        }

        [HttpPost("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _unitOfWork.UserRepo.GetByIdAsync(userId);
            if (user == null) return NotFound(new { message = "User not found." });

            _unitOfWork.UserRepo.Delete(user);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "User deleted successfully." });
        }

        [HttpGet("SearchUsers")]
        public async Task<IActionResult> SearchUsers(string? email, string? phone)
        {
            var users = await _unitOfWork.UserRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(email))
                users = users.Where(u => u.Email != null && u.Email.Contains(email, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(phone))
                users = users.Where(u => u.PhoneNumber != null && u.PhoneNumber.Contains(phone));

            var result = users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                u.PhoneNumber,
                IsBlocked = u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.Now
            }).ToList();

            if (!result.Any())
                return NotFound(new { message = "No users found." });

            return Ok(result);
        }



        [HttpGet("Disputes")]
        public async Task<IActionResult> GetDisputes()
        {
            var disputes = await _unitOfWork.DisputeRepo.GetAllAsync();
            if (!disputes.Any())
                return NotFound(new { message = "No disputes found." });

            var result = disputes.Select(d => new
            {
                d.Id,
                d.PackageId,
                d.Description,
                d.Status,
                d.DisputeType,
                d.CreatedAt
            });

            return Ok(result);
        }

        [HttpGet("Dispute/{id}")]
        public async Task<IActionResult> GetDisputeDetails(int id)
        {
            var dispute = await _unitOfWork.DisputeRepo.GetDisputeDetailsAsync(id);
            if (dispute == null) return NotFound(new { message = "Dispute not found." });

            return Ok(new
            {
                dispute.Id,
                dispute.PackageId,
                dispute.Description,
                dispute.Status,
                dispute.DisputeType,
                dispute.ResolutionNotes,
                ProofImages = dispute.ProofImages.Select(p => p.ImageUrl),
                StatusHistory = dispute.StatusHistory.Select(s => new { s.Status, s.ChangedAt }),
                dispute.CreatedAt
            });
        }

        [HttpPost("ResolveDispute/{disputeId}")]
        public async Task<IActionResult> ResolveDispute(int disputeId, [FromBody] ResolveDisputeDto dto)
        {
            var dispute = await _unitOfWork.DisputeRepo.GetByIdAsync(disputeId);
            if (dispute == null) return NotFound(new { message = "Dispute not found." });

            dispute.Status = dto.Status;
            dispute.ResolutionNotes = dto.Notes;

            // Add status history
            dispute.StatusHistory.Add(new DisputeStatusHistory
            {
                Status = dto.Status,
                ChangedAt = DateTime.UtcNow
            });

            _unitOfWork.DisputeRepo.Update(dispute);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Dispute resolved successfully." });
        }


    }
}

