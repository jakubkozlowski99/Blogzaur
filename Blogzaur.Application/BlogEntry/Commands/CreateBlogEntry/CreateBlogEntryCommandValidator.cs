using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry
{
    public class CreateBlogEntryCommandValidator : AbstractValidator<CreateBlogEntryCommand>
    {
        public CreateBlogEntryCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(50).WithMessage("Title must not exceed 50 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(10).WithMessage("Content must be at least 10 characters long.");

            RuleFor(x => x.Description)
                .MaximumLength(400).WithMessage("Description must not exceed 400 characters.");
        }
    }
}
