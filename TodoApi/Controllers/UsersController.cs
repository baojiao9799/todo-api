using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Controllers
{
    [Route("/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController
        (
            IUserRepository repo
        )
        {
            _repo = repo;
        }

        // POST: /users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(LoginData userInfo)
        {
            var user = new User(userInfo.Username, userInfo.Password);
            PasswordUtil.HashUserPassword(user);

            try
            {
                var createdUser = await _repo.CreateAsync(user);

                return CreatedAtAction("GetUser", new { id = createdUser.Id }, createdUser);
            }
            catch (DbUpdateException)
            {
                // TODO: add body
                return Conflict();
            }
        }

        // GET: /users
        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _repo.GetAsync();

            var usersDTO = users.Select(user => user);
            
            return users;
        }

        // GET: /users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _repo.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        // PUT: users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ResetPassword(Guid id, [FromBody] ResetPasswordModel resetPasswordInfo)
        {
            var userToUpdate = await _repo.FindAsync(id);

            if (!PasswordUtil.IsPasswordCorrect(userToUpdate, resetPasswordInfo.OldPassword))
            {
                return BadRequest();
            }

            userToUpdate.Password = resetPasswordInfo.NewPassword;
            PasswordUtil.HashUserPassword(userToUpdate);

            var updatedUser = await _repo.UpdateAsync(id, userToUpdate);

            if (updatedUser == null)
            {
                return NotFound();
            }

            return NoContent();
        }


        // DELETE: users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
           var deletedUser = await _repo.DeleteAsync(id);

           if (deletedUser == null)
           {
               return NotFound();
           }

           return NoContent();
        }
    }
}
