﻿using Blogzaur.Domain.Interfaces;
using Blogzaur.Infrastructure.Persistence;
using Blogzaur.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blogzaur.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BlogzaurDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BlogzaurDb")));

            services.AddScoped<IBlogEntryRepository, BlogEntryRepository>();
        }
    }
}
