using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace TodoApi.Models
{
    public class TodoContext : DbContext
    {
        private readonly ApplicationConfig _applicationConfig;
        private string? _connectionString => _applicationConfig.ConnectionString;
        public TodoContext(
            DbContextOptions<TodoContext> options,
            ApplicationConfig applicationConfig
        )
            : base(options)
        {
            _applicationConfig = applicationConfig;
        }

        public DbSet<Todo> Todos { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Token> Tokens { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseNpgsql(_connectionString)
                    .EnableSensitiveDataLogging()
                    .UseSnakeCaseNamingConvention();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Status>("completion_status");
        }
           
    }
}