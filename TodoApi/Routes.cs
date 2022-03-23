using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi
{
    public static class RouteExtensions
    {
        public static void MapRoutes(this WebApplication app)
        {
            app.MapPost("/todos", async (Todo todo, ITodoRepository repo) => 
            {
                var createdTodo = await repo.CreateAsync(todo);

                return Results.Created($"/todos/{createdTodo.Id}", createdTodo);
            });

            app.MapGet("/todos", async (ITodoRepository repo) =>
            {
                var todos = await repo.FetchAsync();

                return Results.Ok(todos);
            });

            app.MapPut("/todos/{id}", async (Guid id, Todo inputTodo, ITodoRepository repo) =>
            {
                var updatedTodo = await repo.UpdateAsync(id, inputTodo);

                if (updatedTodo is null) return Results.NotFound();

                return Results.NoContent();
            });

            app.MapDelete("/todos/{id}", async (Guid id, ITodoRepository repo) =>
            {
                var deletedTodo = await repo.DeleteAsync(id);

                if (deletedTodo is null) return Results.NotFound();

                return Results.NoContent();
            });
        }
    }
}