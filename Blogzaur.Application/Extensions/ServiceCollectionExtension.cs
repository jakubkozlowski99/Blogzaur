using AutoMapper;
using Blogzaur.Application.ApplicationUser;
using Blogzaur.Application.BlogEntry.Commands.CreateBlogEntry;
using Blogzaur.Application.Comment.Commands.CreateComment;
using Blogzaur.Application.Mappings;
using Blogzaur.Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogzaur.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();
            services.AddMediatR(typeof(CreateBlogEntryCommand));

            services.AddScoped(provider => new MapperConfiguration(cfg => 
                {
                    var scope = provider.CreateScope();
                    var userContext = scope.ServiceProvider.GetRequiredService<IUserContext>();

                    cfg.AddProfile(new BlogEntryMappingProfile(userContext));
                    cfg.AddProfile(new CommentMappingProfile(userContext));
                }).CreateMapper());

            services.AddValidatorsFromAssemblyContaining<CreateBlogEntryCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<CreateCommentCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
        }
    }
}
