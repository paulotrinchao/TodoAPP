using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        private static void InjectRepositories(IServiceCollection services)
        {
            services.AddTransient<ITarefaRepository, TarefaRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }               

        public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

             InjectRepositories(services);

            return services;
        }

        public static IServiceCollection AddDBImemory(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("todoDB"));
            InjectRepositories(services);
            return services;
        }
    }
}
