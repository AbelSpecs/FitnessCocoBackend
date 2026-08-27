using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.Presentation;
using InsightCore.Application.UseCases.Users.Commands.ForgotPasswordCommand;
using InsightCore.Application.UseCases.Users.Commands.ResetPasswordCommand;
using InsightCore.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InsightCore.Application.Tests
{
    public class PasswordResetHandlersTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUsersRepository> _mockUsersRepo;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly IConfiguration _configuration;

        public PasswordResetHandlersTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUsersRepo = new Mock<IUsersRepository>();
            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUsersRepo.Object);
            _mockEmailService = new Mock<IEmailService>();

            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["AppSettings:FrontendUrl"]).Returns("https://pyrosfit.com");
            _configuration = mockConfig.Object;
        }

        [Fact]
        public async Task ForgotPassword_WhenUserExists_SetsTokenAndSendsEmail()
        {
            // Arrange
            var user = new User
            {
                Id = 10,
                FirstName = "Carlos",
                LastName = "Gomez",
                UserName = "cgomez",
                Email = "carlos@example.com",
                Password = "hashedpassword",
                Birthdate = new DateTime(1990, 1, 1)
            };

            _mockUsersRepo.Setup(r => r.GetByEmailAsync("carlos@example.com"))
                .ReturnsAsync(user);
            _mockUsersRepo.Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            var handler = new ForgotPasswordHandler(
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _configuration,
                NullLogger<ForgotPasswordHandler>.Instance);

            var command = new ForgotPasswordCommand { Email = "carlos@example.com" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(user.PasswordResetToken);
            Assert.True(user.PasswordResetTokenExpiry > DateTime.UtcNow);
            _mockEmailService.Verify(e => e.SendPasswordResetEmailAsync(user.Email, It.Is<string>(link => link.Contains("reset-password?code=")), user.FirstName), Times.Once);
        }

        [Fact]
        public async Task ForgotPassword_WhenUserDoesNotExist_ReturnsSuccessWithoutSendingEmail()
        {
            // Arrange
            _mockUsersRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            var handler = new ForgotPasswordHandler(
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _configuration,
                NullLogger<ForgotPasswordHandler>.Instance);

            var command = new ForgotPasswordCommand { Email = "nonexistent@example.com" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _mockEmailService.Verify(e => e.SendPasswordResetEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ResetPassword_WithValidOpaqueCode_UpdatesPasswordAndClearsToken()
        {
            // Arrange
            const string plainToken = "test-token-123456";
            var user = new User
            {
                Id = 15,
                FirstName = "Maria",
                LastName = "Perez",
                UserName = "mperez",
                Email = "maria@example.com",
                Password = "oldhashedpassword",
                Birthdate = new DateTime(1995, 5, 5),
                PasswordResetToken = plainToken,
                PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1)
            };

            _mockUsersRepo.Setup(r => r.GetByIdAsync(15))
                .ReturnsAsync(user);
            _mockUsersRepo.Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Create opaque code: Base64Url("15:test-token-123456")
            var raw = $"15:{plainToken}";
            var opaqueCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(raw)).Replace("+", "-").Replace("/", "_").TrimEnd('=');

            var handler = new ResetPasswordHandler(
                _mockUnitOfWork.Object,
                NullLogger<ResetPasswordHandler>.Instance);

            var command = new ResetPasswordCommand
            {
                Code = opaqueCode,
                NewPassword = "NewSecurePassword123!"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(user.PasswordResetToken);
            Assert.Null(user.PasswordResetTokenExpiry);
            Assert.True(user.CheckPassword("NewSecurePassword123!"));
        }

        [Fact]
        public async Task ResetPassword_WithExpiredToken_ReturnsFailure()
        {
            // Arrange
            const string plainToken = "expired-token";
            var user = new User
            {
                Id = 15,
                FirstName = "Maria",
                LastName = "Perez",
                UserName = "mperez",
                Email = "maria@example.com",
                Password = "oldhashedpassword",
                Birthdate = new DateTime(1995, 5, 5),
                PasswordResetToken = plainToken,
                PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(-1) // Expired
            };

            _mockUsersRepo.Setup(r => r.GetByIdAsync(15)).ReturnsAsync(user);

            var raw = $"15:{plainToken}";
            var opaqueCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(raw)).Replace("+", "-").Replace("/", "_").TrimEnd('=');

            var handler = new ResetPasswordHandler(
                _mockUnitOfWork.Object,
                NullLogger<ResetPasswordHandler>.Instance);

            var command = new ResetPasswordCommand
            {
                Code = opaqueCode,
                NewPassword = "NewSecurePassword123!"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("expirado", result.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
