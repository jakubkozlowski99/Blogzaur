using Xunit;
using Blogzaur.Application.BlogEntry.Commands.EditBlogEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;

namespace Blogzaur.Application.BlogEntry.Commands.EditBlogEntry.Tests
{
    public class EditBlogEntryCommandValidatorTests
    {
        [Fact()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
        {
            //arrange
            var command = new EditBlogEntryCommand
            {
                Title = "Updated Title",
                Content = "This is updated content with more than ten characters.",
                Description = "This is an updated description.",
                CategoryIds = new List<int> { 1, 2, 3 }
            };
            var validator = new EditBlogEntryCommandValidator();

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validate_WithInvalidCommand_ShouldHaveValidationErrors()
        {
            //arrange
            var command = new EditBlogEntryCommand
            {
                Title = "",
                Content = "",
                CategoryIds = new List<int>()
            };
            var validator = new EditBlogEntryCommandValidator();

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldHaveValidationErrorFor(c => c.Title);
            result.ShouldHaveValidationErrorFor(c => c.Content);
            result.ShouldHaveValidationErrorFor(c => c.CategoryIds);
        }
    }
}