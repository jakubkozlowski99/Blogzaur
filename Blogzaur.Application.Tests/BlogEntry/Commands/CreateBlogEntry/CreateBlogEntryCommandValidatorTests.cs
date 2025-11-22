using Xunit;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry.Tests
{
    public class CreateBlogEntryCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
        {
            //arrange
            var command = new CreateBlogEntryCommand
            {
                Title = "Valid Title",
                Content = "This is a valid content with more than ten characters.",
                Description = "This is a valid description.",
                CategoryIds = new List<int> { 1, 2 }
            };
            var validator = new CreateBlogEntryCommandValidator();
            //act
            var result = validator.TestValidate(command);
            //assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validate_WithInvalidCommand_ShouldHaveValidationErrors()
        {
            //arrange
            var command = new CreateBlogEntryCommand
            {
                Title = "",
                Content = "",
                CategoryIds = new List<int>()
            };
            var validator = new CreateBlogEntryCommandValidator();
            //act
            var result = validator.TestValidate(command);
            //assert
            result.ShouldHaveValidationErrorFor(c => c.Title);
            result.ShouldHaveValidationErrorFor(c => c.Content);
            result.ShouldHaveValidationErrorFor(c => c.CategoryIds);
        }
    }
}