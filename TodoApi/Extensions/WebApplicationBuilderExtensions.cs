using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            var configManager = builder.Configuration;
            var config = new ApplicationConfig();
            configManager.Bind("Application", config);
            builder.Services.AddScoped<ApplicationConfig>(provider => config);

            builder.Services.AddDbContext<TodoContext>();
            builder.Services.AddScoped<ITodoRepository, TodoRepository>();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}