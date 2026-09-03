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

        /// <summary>
        /// Servir de forma privada la imagen de perfil de un usuario desde R2 a través de la API.
        /// Devuelve el stream del objeto y soporta range requests para reproducción/streaming eficiente.
        /// </summary>
        [HttpGet("users/{userId}/profile")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserProfilePicture([FromRoute] int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(user.ProfilePictureKey))
                return NotFound();

            var fileKey = user.ProfilePictureKey!;
            try
            {
                var (stream, contentType, _) = await _storageService.GetObjectStreamAsync(fileKey);
                return File(stream, contentType ?? "application/octet-stream", enableRangeProcessing: true);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Endpoint genérico para servir cualquier archivo almacenado por key.
        /// Útil para vídeos de ejercicios, banners por trainer u otros recursos que ya guardan la key en la DB.
        /// </summary>
        [HttpGet("serve")]
        [AllowAnonymous]
        public async Task<IActionResult> Serve([FromQuery] string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return BadRequest("key is required");

            try
            {
                // Para vídeos grandes/streaming, devolver URL pre-firmada para que el cliente
                // gestione streaming y range requests directamente contra el proveedor (R2).
                    var ext = System.IO.Path.GetExtension(key)?.ToLowerInvariant() ?? string.Empty;
                    var mediaExtensions = new[] { ".mp4", ".mov", ".webm", ".mkv", ".ogg", ".avi" };

                    if (mediaExtensions.Contains(ext))
                    {
                        var url = await _storageService.GeneratePresignedDownloadUrl(key, TimeSpan.FromSeconds(600));
                        return Ok(new { downloadUrl = url });
                    }

                    var (stream, contentType, _) = await _storageService.GetObjectStreamAsync(key);
                    return File(stream, contentType ?? "application/octet-stream", enableRangeProcessing: true);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Servir de forma privada la imagen de banner de un usuario desde R2 a través de la API.
        /// </summary>
        [HttpGet("users/{userId}/banner")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserBannerPicture([FromRoute] int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(user.BannerPictureKey))
                return NotFound();

            var fileKey = user.BannerPictureKey!;
            try
            {
                var (stream, contentType, _) = await _storageService.GetObjectStreamAsync(fileKey);
                return File(stream, contentType ?? "application/octet-stream", enableRangeProcessing: true);
            }
            catch
            {
                return NotFound();
            }
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
            //if (!int.TryParse(_currentUser.UserId, out var currentUserId))
            //    return Forbid();

            var coach = await _unitOfWork.Coaches.GetByIdAsync(trainerId);
            if (coach == null)
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
