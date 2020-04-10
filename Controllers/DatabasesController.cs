using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AIMS.Data;
using AIMS.Pagination;
using AIMS.Repositories;

namespace AIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabasesController : ControllerBase
    {
        private readonly IDatabaseRepository repository;

        public DatabasesController(IDatabaseRepository _repository)
        {
            repository = _repository;
        }

        [HttpGet]
        public IEnumerable<Database> Get([FromQuery] PagingParameters parameters)
        {
            return repository.GetAll(parameters, HttpContext);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (!repository.Get(id, out var database))
            {
                return NotFound();
            }
            return Ok(database);
        }

        // ADD: api/databases
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Database database)
        {
            await repository.AddAsync(database);
            return CreatedAtAction(nameof(GetById), new { id = database.Id }, database);
        }

        // DELETE: api/databases/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (!repository.Get(id, out var database))
            {
                return NotFound();
            }
            repository.Delete(id);
            return Ok(database);
        }

        // UPDATE: api/databases/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, Database database)
        {
            if (id != database.Id)
            {
                return BadRequest();
            }
            repository.Update(database);
            return Ok(database);
        }
    }
}
