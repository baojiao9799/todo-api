using TodoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Repositories {
    public interface ITodoRepository : IRepository<Todo, Guid> {}

    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDb _db;
        private DbSet<Todo> todos => _db.Todos;

        public TodoRepository(
            TodoDb db
        )
        {
            _db = db;
        }

        public async Task<Todo> CreateAsync(Todo entity) 
        {
            var todo = todos.Add(entity);
            await _db.SaveChangesAsync();

            return todo.Entity;
        }

        public async Task<List<Todo>> FetchAsync()
        {
            return await todos.ToListAsync();
        }

        public async Task<Todo?> UpdateAsync(Guid id, Todo inputTodo) {
            var todo = await todos.FindAsync(id);

            if (todo is null) return null;

            todo.Title = inputTodo.Title;
            todo.Status = inputTodo.Status;
            todo.DueDate = inputTodo.DueDate;

            await _db.SaveChangesAsync();

            return todo;
        }

        public async Task<Todo?> DeleteAsync(Guid id) {
            var todo = await todos.FindAsync(id);

            if (todo is null) return null;

            var removedTodo = todos.Remove(todo);
            
            await _db.SaveChangesAsync();
            
            return removedTodo.Entity;
        }
    }
}