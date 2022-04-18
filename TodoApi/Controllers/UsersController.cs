using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<IActionResult> ResetPassword(Guid id, [FromBody] ResetPasswordModel resetPasswordInfo)
        {
            // Get user for session
            var userId = HttpContext.User.Identity?.Name;
            if (userId == null)
            {
                return Unauthorized();
            }

            // Throw 403 Forbidden if user IDs don't match
            if (id.ToString() != userId)
            {
                return Forbid();
            }

            var userToUpdate = await _repo.FindAsync(id);

            // Throw 400 Bad Request if old password incorrect
            if (!PasswordUtil.IsPasswordCorrect(userToUpdate, resetPasswordInfo.OldPassword))
            {
                return BadRequest
                (
                    new ApiResponse<CreateSessionMeta, Empty> 
                    { 
                        Meta = new CreateSessionMeta 
                        { 
                            Success = false, 
                            Message = "Incorrect old password"
                        },
                        Data = new Empty {}
                    }
                );
            }

            // Throw 404 Not Found if user not found
            if (userToUpdate == null)
            {
                return NotFound();
            }

            // Update password
            userToUpdate.Password = resetPasswordInfo.NewPassword;
            PasswordUtil.HashUserPassword(userToUpdate);

            var updatedUser = await _repo.UpdateAsync(id, userToUpdate);

            return NoContent();
        }


        // DELETE: users/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
             // Get user for session
            var userId = HttpContext.User.Identity?.Name;
            if (userId == null)
            {
                return Unauthorized();
            }

            // Throw 403 Forbidden if user IDs don't match
            if (id.ToString() != userId)
            {
                return Forbid();
            }

            // Delete user
            var deletedUser = await _repo.DeleteAsync(id);

            // Throw 404 Not Found if user not found
            if (deletedUser == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
