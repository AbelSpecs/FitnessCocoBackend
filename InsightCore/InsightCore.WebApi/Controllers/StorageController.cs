using InsightCore.Application.Interface.Integration;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.Presentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public StorageController(IStorageService storageService, IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _storageService = storageService;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        [HttpPost("presign/profile")]
        [AllowAnonymous]
        public async Task<IActionResult> PresignProfile([FromQuery] int userId, [FromQuery] string fileName, [FromQuery] string contentType, [FromQuery] int expiresInSeconds = 300)
        {
            var ext = System.IO.Path.GetExtension(fileName);
            var key = $"perfiles/usuarios/user-{userId}{ext}";
            var url = await _storageService.GeneratePresignedUploadUrl(key, contentType, TimeSpan.FromSeconds(expiresInSeconds));
            return Ok(new { uploadUrl = url, key });
        }

        [HttpPost("presign/banner")]
        [AllowAnonymous]
        public async Task<IActionResult> PresignBanner([FromQuery] int trainerId, [FromQuery] string fileName, [FromQuery] string contentType, [FromQuery] int expiresInSeconds = 300)
        {
            var ext = System.IO.Path.GetExtension(fileName);
            var key = $"banners/trainers/trainer-{trainerId}{ext}";
            var url = await _storageService.GeneratePresignedUploadUrl(key, contentType, TimeSpan.FromSeconds(expiresInSeconds));
            return Ok(new { uploadUrl = url, key });
        }

        [HttpPost("presign/video")]
        [AllowAnonymous]
        public async Task<IActionResult> PresignVideo([FromQuery] int trainerId, [FromQuery] int exerciseId, [FromQuery] string fileName, [FromQuery] string contentType, [FromQuery] int expiresInSeconds = 600)
        {
            // Validar que el entrenador autenticado sea el propietario del trainerId
            if (!int.TryParse(_currentUser.UserId, out var currentUserId))
                return Forbid();

            var coach = await _unitOfWork.Coaches.GetByIdAsync(trainerId);
            if (coach == null || coach.UserId != currentUserId)
                return Forbid();

            var ext = System.IO.Path.GetExtension(fileName);
            var key = $"videos/trainers/{trainerId}/ejercicios/{exerciseId}{ext}";
            var url = await _storageService.GeneratePresignedUploadUrl(key, contentType, TimeSpan.FromSeconds(expiresInSeconds));
            return Ok(new { uploadUrl = url, key });
        }

        [HttpGet("download")]
        [AllowAnonymous]
        public async Task<IActionResult> Download([FromQuery] string key, [FromQuery] int expiresInSeconds = 300)
        {
            var url = await _storageService.GeneratePresignedDownloadUrl(key, TimeSpan.FromSeconds(expiresInSeconds));
            return Ok(new { downloadUrl = url });
        }
    }
}
