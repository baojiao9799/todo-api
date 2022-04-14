using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace TodoApi.Controllers
{
    [Authorize]
    [Route("/todos")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoRepository _repo;

        public TodosController
        (
            ITodoRepository repo
        )
        {
            _repo = repo;
        }

        // POST: /todos
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            // Get user for current session
            var guid = new Guid();
            var userId = HttpContext.User.Identity?.Name;
            Guid.TryParse(userId, out guid);
            if (guid == Guid.Empty)
            {
                return Unauthorized();
            }

            // Create todo for user
            todo.UserId = guid;
            var createdTodo = await _repo.CreateAsync(todo);

            return CreatedAtAction("GetTodo", new { id = createdTodo.Id }, createdTodo);
        }

        // GET: /todos
        [HttpGet]
        public async Task<ActionResult<List<Todo>>> GetTodos([FromQuery] string? sortBy, [FromQuery] string? order, [FromQuery] Status? status)
        {
            // Get user for session
            var userId = HttpContext.User.Identity?.Name;
            if (userId == null)
            {
                return Unauthorized();
            }

            // Get todos for user
            var todos = await _repo.GetAsync();
            todos = todos.Where(todo => todo.UserId.ToString() == userId);

            // Sort results
            switch (sortBy)
            {   
                case "due_date":
                    if (order == "desc") {
                        todos = todos.OrderByDescending(todo => todo.DueDate);
                    } else {
                        todos = todos.OrderBy(todo => todo.DueDate);
                    }
                    break;
                case "status":
                    if (order == "desc") 
                    {
                        todos = todos.OrderByDescending(todo => todo.Status);
                    } 
                    else 
                    {
                        todos = todos.OrderBy(todo => todo.Status);
                    }
                    break;
                case "title":
                case "name":
                    if (order == "desc")
                    {
                        todos = todos.OrderByDescending(todo => todo.Title);
                    }
                    else
                    {
                        todos = todos.OrderBy(todo => todo.Title);
                    }
                    break;
                case "creation_date":
                default:
                    if (order == "desc")
                    {
                        todos = todos.OrderByDescending(todo => todo.CreationDate);
                    }
                    else
                    {
                        todos = todos.OrderBy(todo => todo.CreationDate);
                    }
                    break;
            }

            // Filter results
            if (status != null) {
                todos = todos.Where(todo => todo.Status == status);
            }
            
            return todos.ToList();
        }

        // GET: /todos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(Guid id)
        {
            // Get user for current session
            var userId = HttpContext.User.Identity?.Name;
            if (userId == null)
            {
                return Unauthorized();
            }

            // Get todo
            var todo = await _repo.FindAsync(id);

            // Throw 404 Not Found if todo can't be found
            if (todo == null)
            {
                return NotFound();
            }

            // Throw 403 Forbidden if todo doesn't belong to user
            if (todo.UserId.ToString() != userId)
            {
                return Forbid();
            }

            return todo;
        }


        // PUT: todos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(Guid id, Todo todo)
        {
            // Get user for current session
            var userId = HttpContext.User.Identity?.Name;
            if (userId == null)
            {
                return Unauthorized();
            }

            // Get todo
            var todoToUpdate = await _repo.FindAsync(id);

            // Throw 404 Not Found if todo can't be found
            if (todoToUpdate == null)
            {
                return NotFound();
            }

            // Throw 403 Forbidden if todo doesn't belong to user
            if (todo.UserId.ToString() != userId)
            {
                return Forbid();
            }

            // Update todo
            var updatedTodo = await _repo.UpdateAsync(id, todo);

            return NoContent();
        }


        // DELETE: todos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            // Get user for current session
            var userId = HttpContext.User.Identity?.Name;
            if (userId == null)
            {
                return Unauthorized();
            }

            // Get todo
            var todo = await _repo.FindAsync(id);

            // Throw 404 Not Found if todo can't be found
            if (todo == null)
            {
                return NotFound();
            }

            // Throw 403 Forbidden if todo doesn't belong to user
            if (todo.UserId.ToString() != userId)
            {
                return Forbid();
            }

            // Delete todo
            var deletedTodo = await _repo.DeleteAsync(id);

            return NoContent();
        }
    }
}
