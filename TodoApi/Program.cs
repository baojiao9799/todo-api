using Microsoft.EntityFrameworkCore;
using TodoApi.Repositories;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("Todos"));
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.MapRoutes();

app.Run();