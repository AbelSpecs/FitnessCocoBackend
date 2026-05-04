using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InsightCore.Application.UseCases.Students.Commands.CreateStudentCommand;

namespace InsightCore.Application.IntegrationTest
{
    [TestClass]
    public class CreateStudentCommandValidatorTests
    {
        [TestMethod]
        public void Validator_WhenStudentIsNull_ShouldHaveError()
        {
            var validator = new CreateStudentCommandValidator();
            var command = new CreateStudentCommand { Student = null };
            var validationResult = validator.Validate(command);
            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationResult.Errors.Any(e => e.PropertyName == "Student"));
        }
    }
}
