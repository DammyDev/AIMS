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
    public class ServersController : ControllerBase
    {
        private readonly IServerRepository repository;

        public ServersController(IServerRepository _repository)
        {
            repository = _repository;
        }

        [HttpGet]
        public IEnumerable<Server> Get([FromQuery] PagingParameters parameters)
        {
            return repository.GetAll(parameters, HttpContext);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (!repository.Get(id, out var serverInformation))
            {
                return NotFound();
            }
            return Ok(serverInformation);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Server server)
        {
            await repository.AddAsync(server);
            return CreatedAtAction(nameof(GetById), new { id = server.Id }, server);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (!repository.Get(id, out var serverInformation))
            {
                return NotFound();
            }
            repository.Delete(id);
            return Ok(serverInformation);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Server server)
        {
            if (id != server.Id)
            {
                return BadRequest();
            }
            repository.Update(server);
            return Ok(server);
        }
    }
}