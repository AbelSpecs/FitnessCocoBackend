using InsightCore.Application.Interface.Persistence;
using InsightCore.Application.Interface.Presentation;
using InsightCore.Application.UseCases.Students.Commands.SendMotivationEmailCommand;
using InsightCore.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InsightCore.Application.Tests
{
    public class SendMotivationEmailHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IStudentsRepository> _mockStudentsRepo;
        private readonly Mock<IUsersRepository> _mockUsersRepo;
        private readonly Mock<IEmailService> _mockEmailService;

        public SendMotivationEmailHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockStudentsRepo = new Mock<IStudentsRepository>();
            _mockUsersRepo = new Mock<IUsersRepository>();
            _mockUnitOfWork.Setup(u => u.Students).Returns(_mockStudentsRepo.Object);
            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUsersRepo.Object);
            _mockEmailService = new Mock<IEmailService>();
        }

        [Fact]
        public async Task SendMotivationEmail_WhenStudentExists_SendsEmailSuccessfully()
        {
            // Arrange
            var student = new Student
            {
                Id = 1,
                UserId = 100,
                GymId = 2
            };

            var user = new User
            {
                Id = 100,
                FirstName = "Lucas",
                LastName = "Silva",
                UserName = "lsilva",
                Email = "lucas@example.com",
                Password = "pwd",
                Birthdate = new DateTime(1998, 2, 2)
            };

            _mockStudentsRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);
            _mockUsersRepo.Setup(r => r.GetByIdAsync(100)).ReturnsAsync(user);

            var handler = new SendMotivationEmailHandler(
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                NullLogger<SendMotivationEmailHandler>.Instance);

            var command = new SendMotivationEmailCommand
            {
                StudentId = 1,
                Message = "¡Vamos Lucas, no pierdas tu racha hoy!",
                CoachName = "Coach Alex"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _mockEmailService.Verify(e => e.SendMotivationEmailAsync(
                "lucas@example.com",
                "Lucas Silva",
                "¡Vamos Lucas, no pierdas tu racha hoy!",
                "Coach Alex"), Times.Once);
        }

        [Fact]
        public async Task SendMotivationEmail_WhenStudentNotFound_ReturnsFailure()
        {
            // Arrange
            _mockStudentsRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Student)null!);

            var handler = new SendMotivationEmailHandler(
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                NullLogger<SendMotivationEmailHandler>.Instance);

            var command = new SendMotivationEmailCommand
            {
                StudentId = 999,
                Message = "¡Entrenemos hoy!"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            _mockEmailService.Verify(e => e.SendMotivationEmailAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
