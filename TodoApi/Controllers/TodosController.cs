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
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return Ok(await _repo.GetAsync());
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

            return Ok(todo);
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
