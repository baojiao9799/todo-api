#nullable disable
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Utils;
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
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            PasswordUtil.HashUserPassword(user);

            var createdUser = await _repo.CreateAsync(user);

            return CreatedAtAction("GetUser", new { id = createdUser.Id }, UserDTO.UserToDTO(createdUser));
        }

        // GET: /users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _repo.GetAsync();

            var usersDTO = users.Select(user => UserDTO.UserToDTO(user));

            return Ok(usersDTO);
        }

        // GET: /users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            var user = await _repo.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(UserDTO.UserToDTO(user));
        }


        // PUT: users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ResetPassword(Guid id, ResetPasswordModel resetPasswordInfo)
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
