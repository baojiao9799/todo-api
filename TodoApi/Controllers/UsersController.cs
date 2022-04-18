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
        public async Task<ActionResult<User>> CreateUser(ApiPayload<AuthData> userInfoPayload)
        {
            var userInfo = userInfoPayload.Data;
            var user = new User(userInfo.Username, userInfo.Password);
            PasswordUtil.HashUserPassword(user);

            try
            {
                var createdUser = await _repo.CreateAsync(user);

                return CreatedAtAction("GetUser", new { id = createdUser.Id }, createdUser);
            }
            catch (DbUpdateException)
            {
                return Conflict
                (
                    new ApiResponse<SuccessMeta, Empty>
                    {
                        Meta = new SuccessMeta
                        {
                            Success = false,
                            Msg = "username already exists"
                        },
                        Data = new Empty {}
                    }
                );
            }
        }

        // GET: /users
        [HttpGet]
        public async Task<ApiResponse<FetchUsersMeta, IEnumerable<User>>> GetUsers()
        {
            var users = await _repo.GetAsync();
            
            return new ApiResponse<FetchUsersMeta, IEnumerable<User>>
            {
                Meta = new FetchUsersMeta
                {
                    UserCount = users.Count()
                },
                Data = users
            };
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
        public async Task<IActionResult> ResetPassword(Guid id, [FromBody] ApiResponse<ResetPasswordModel> resetPasswordPayload)
        {
            // Get user for session
            var userId = HttpContext.User.Identity?.Name;
            if (userId == null)
            {
                return Unauthorized();
            }

            var userToUpdate = await _repo.FindAsync(id);

            // Throw 404 Not Found if user not found
            if (userToUpdate == null)
            {
                return NotFound();
            }

            // Throw 403 Forbidden if user IDs don't match
            if (id.ToString() != userId)
            {
                return Forbid();
            }

            // Throw 400 Bad Request if old password incorrect
            var resetPasswordInfo = resetPasswordPayload.Data;
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
