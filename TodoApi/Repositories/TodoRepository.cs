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

        public async Task<Todo> UpdateAsync(Guid id, Todo inputTodo) {
            inputTodo.Id = id;

            var todo = todos.Update(inputTodo);

            await _db.SaveChangesAsync();

            return todo.Entity;
        }

        public async Task<Todo> DeleteAsync(Guid id) {
            var todo = todos.Remove(new Todo { Id = id });
            
            await _db.SaveChangesAsync();
            
            return todo.Entity;
        }
    }
}