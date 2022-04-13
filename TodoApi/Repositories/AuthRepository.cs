using TodoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Repositories
{
    public interface IAuthRepository : IRepository<Token, Guid> {}
    
    public class AuthRepository : IAuthRepository {
        private readonly TodoContext _context;

        public AuthRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<Token> CreateAsync(Token entity)
        {
            var token = _context.Tokens.Add(entity);
            await _context.SaveChangesAsync();

            return token.Entity;
        }

        public async Task<IEnumerable<Token>> GetAsync()
        {
            //return await _context.Todos.ToListAsync();
            throw new NotImplementedException();
        }

        public async Task<Token?> FindAsync(params object[] keyValues)
        {
            //return await _context.Todos.FindAsync(keyValues);
            throw new NotImplementedException();
        }

        public async Task<Token?> UpdateAsync(Guid index, Token entity)
        {
            // try 
            // {
            //     entity.Id = index;

            //     var todo = _context.Update(entity);
            //     await _context.SaveChangesAsync();

            //     return todo.Entity;

            // }
            // catch (DbUpdateConcurrencyException)
            // {
            //     return null;
            // }
            throw new NotImplementedException();
        }

        public async Task<Token?> DeleteAsync(Guid index)
        {
            // var todo = await _context.Todos.FindAsync(index);
            // if (todo == null)
            // {
            //     return null;
            // }

            // _context.Todos.Remove(todo);
            // await _context.SaveChangesAsync();

            // return todo;
            throw new NotImplementedException();
        }
    }
}