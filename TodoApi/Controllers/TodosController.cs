#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Controllers
{
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
            var createdTodo = await _repo.CreateAsync(todo);

            return CreatedAtAction("GetTodo", new { id = createdTodo.Id }, createdTodo);
        }

        // GET: /todos
        [HttpGet]
        public async Task<IEnumerable<Todo>> GetTodos([FromQuery] string sortBy, [FromQuery] string order, [FromQuery] Status? status)
        {
            var todos = await _repo.GetAsync();

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

            if (status != null) {
                todos = todos.Where(todo => todo.Status == status);
            }
            
            return todos;
        }

        // GET: /todos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(Guid id)
        {
            var todo = await _repo.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }


        // PUT: todos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(Guid id, Todo todo)
        {
            var updatedTodo = await _repo.UpdateAsync(id, todo);

            if (updatedTodo == null)
            {
                return NotFound();
            }

            return NoContent();
        }


        // DELETE: todos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
           var deletedTodo = await _repo.DeleteAsync(id);

           if (deletedTodo == null)
           {
               return NotFound();
           }

           return NoContent();
        }
    }
}
