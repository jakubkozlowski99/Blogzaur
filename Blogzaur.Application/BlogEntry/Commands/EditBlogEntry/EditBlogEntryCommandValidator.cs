using Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Commands.EditBlogEntry
{
    public class EditBlogEntryCommandValidator : AbstractValidator<EditBlogEntryCommand>
    {
        public EditBlogEntryCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(10).WithMessage("Content must be at least 10 characters long.");

            RuleFor(x => x.Description)
                .MaximumLength(400).WithMessage("Description must not exceed 400 characters.");

            RuleFor(x => x.CategoryIds)
                .NotEmpty().WithMessage("At least one category must be selected.");
        }
    }
}
