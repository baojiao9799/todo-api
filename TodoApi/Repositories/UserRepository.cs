using TodoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Repositories
{
    public interface IUserRepository : IRepository<User, Guid> {}
    
    public class UserRepository : IUserRepository {
        private readonly TodoContext _context;

        public UserRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User entity)
        {
            var user = _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return user.Entity;
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> FindAsync(params object[] keyValues)
        {
            return await _context.Users.FindAsync(keyValues);
        }

        public async Task<User?> UpdateAsync(Guid index, User entity)
        {
            try 
            {
                entity.Id = index;

                var user = _context.Update(entity);
                await _context.SaveChangesAsync();

                return user.Entity;

            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
        }

        public async Task<User?> DeleteAsync(Guid index)
        {
            var user = await _context.Users.FindAsync(index);
            if (user == null)
            {
                return null;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}