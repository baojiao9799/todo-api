using TodoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Repositories
{
    public interface ITodoRepository : IRepository<Todo, Guid> {}
    
    public class TodoRepository : ITodoRepository {
        private readonly TodoContext _context;

        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<Todo> CreateAsync(Todo entity)
        {
            var todo = _context.Todos.Add(entity);
            await _context.SaveChangesAsync();

            return todo.Entity;
        }

        public async Task<IEnumerable<Todo>> GetAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo?> FindAsync(params object[] keyValues)
        {
            return await _context.Todos.FindAsync(keyValues);
        }

        public async Task<Todo?> UpdateAsync(Guid index, Todo entity)
        {
            try 
            {
                entity.Id = index;

                var todo = _context.Update(entity);
                await _context.SaveChangesAsync();

                return todo.Entity;

            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
        }

        public async Task<Todo?> DeleteAsync(Guid index)
        {
            var todo = await _context.Todos.FindAsync(index);
            if (todo == null)
            {
                return null;
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return todo;
        }
    }
}