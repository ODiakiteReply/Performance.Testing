using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Performance.Testing.DataAccess.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Performance.Testing.Business.Services.Interfaces;
using Performance.Testing.Business.Services.Implementations;
using Performance.Testing.DataAccess.Repositories.CDRepositories.Interfaces;
using Performance.Testing.DataAccess.Repositories.CDRepositories.Implementations;

namespace Performance.Testing.Business.Extensions
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
