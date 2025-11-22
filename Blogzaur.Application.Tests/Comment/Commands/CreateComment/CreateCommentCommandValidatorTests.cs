using Xunit;
using FluentValidation.TestHelper;

namespace Blogzaur.Application.Comment.Commands.CreateComment.Tests
{
    public class CreateCommentCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
        {
            //arrange
            var command = new CreateCommentCommand
            {
                BlogEntryId = 1,
                Content = "This is a valid comment content."
            };
            var validator = new CreateCommentCommandValidator();

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validate_WithInvalidCommand_ShouldHaveValidationErrors()
        {
            //arrange
            var command = new CreateCommentCommand
            {
                BlogEntryId = 1,
                Content = "" // Invalid: Empty content
            };
            var validator = new CreateCommentCommandValidator();

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldHaveValidationErrorFor(c => c.Content);
        }
    }
}