using System;
using Microsoft.Extensions.DependencyInjection;
using AZWebAppCDB1.Business.Services.Interfaces;
using AZWebAppCDB1.Business.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AZWebAppCDB1.DataAccess.DbContext;
using Microsoft.AspNetCore.Builder;
using AZWebAppCDB1.DataAccess.Repositories.CDRepositories.Interfaces;
using AZWebAppCDB1.DataAccess.Repositories.CDRepositories.Implementations;

namespace AZWebAppCDB1.Business.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services
                .AddTransient<IUserService, UserService>()
                .AddTransient<IPostService, PostService>()
                .AddTransient<ICommentService, CommentService>()

                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IPostRepository, PostRepository>()
                .AddScoped<ICommentRepository, CommentRepository>();
        }

        public static void MigrateDataBase(this IApplicationBuilder app, IConfiguration configuration)
        {
            AZWebApp1DbContext.GetContext().Migrate(configuration);
        }
    }
}
