using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tarefa> Tarefas { get; set; }
        private readonly ILogger<AppDbContext> _logger;
        public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger)
        : base(options)
        {
            _logger = logger;

            try
            {
                Database.EnsureCreated();
                _logger.LogInformation("Banco de dados verificado/criado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Erro ao verificar/criar banco de dados.");
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarefa>().ToTable("Tarefa");
            modelBuilder.Entity<Tarefa>().HasKey(t => t.Id);
            base.OnModelCreating(modelBuilder);
        }

    }
}
