using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProjectAPI.Data;
using ProjectAPI.Pagination;
using ProjectAPI.Repositories;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationRepository repository;
        private readonly IServerRepository serverRepository;
        private readonly IConfiguration config;

        public ApplicationsController(IConfiguration iConfig, IApplicationRepository repository, IServerRepository _serverRepository)
        {
            this.repository = repository;
            serverRepository = _serverRepository;
            config = iConfig;
        }

        [HttpGet]
        public IEnumerable<Application> Get([FromQuery] PagingParameters parameters)
        {
            return repository.GetAll(parameters, HttpContext);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            if (!repository.Get(id, out var application))
            {
                return NotFound();
            }
            return Ok(application);
        }

        // ADD: api/applications
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Application app)
        {
            await repository.AddAsync(app);
            return CreatedAtAction(nameof(GetById), new { id = app.Id }, app);
        }

        // DELETE: api/applications/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (!repository.Get(id, out var app))
            {
                return NotFound();
            }
            repository.Delete(id);
            return Ok(app);
        }

        // UPDATE: api/applications/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, Application app)
        {
            if (id != app.Id)
            {
                return BadRequest();
            }
            repository.Update(app);
            return Ok(app);
        }

        // GET: api/applications/5
        [HttpGet("GetServers/{id}")]
        public IEnumerable<Server> GetApplications(int id)
        {
            var query = $"select b.* from Applications a, ServerInfo b, Application_ServerInfo c where a.Id = c.ApplicationId  and b.Id = c.ServerInformationId  and a.Id = '{id.ToString()}'";
            return ReadDatabase3(query);
        }

        // ADD a new serverInfo to application: api/application/1
        [HttpPost("AddNewServer/{id}")]
        public async Task<IActionResult> AddNewServerAsync([FromRoute] int id, [FromBody] Server server)
            {
                if (repository.Get(id, out Application app))
                {
                    await serverRepository.AddAsync(server);

                    string query = $"INSERT INTO Application_ServerInfo (ApplicationId,ServerInformationId,FolderPath,DateDeployed,DateCreated) " +
                        $"VALUES('{app.Id.ToString()}', '{server.Id.ToString()}', GETDATE())";

                QueryDB(query);
                    return CreatedAtAction(nameof(GetById), new { id = server.Id }, server);
                }
                return NotFound();
            }

        // ADD an existing serverInfo to application: api/application/1
        [HttpPost("AddExistingServer/")]
        public IActionResult AddExistingServer(ApplicationServer ASModel)
        {
            if (repository.Get(ASModel.ApplicationId, out Application app))
            {
                if (serverRepository.Get(ASModel.ServerInformationId, out Server server))
                {
                    string query = $"INSERT INTO Application_ServerInfo (ApplicationId,ServerInformationId,FolderPath,DateDeployed,DateCreated) " +
                        $"VALUES('{app.Id.ToString()}', '{server.Id.ToString()}', '{ASModel.FolderPath}', '{ASModel.DateDeployed}', GETDATE())";
                    QueryDB(query);
                    return CreatedAtAction(nameof(GetById), new { id = server.Id }, server);
                }
            }
            return NotFound();
        }

        // REMOVE an existing server in an application: api/application/removeserver
        [HttpPost("RemoveServer/")]
        public IActionResult RemoveServer(ApplicationServer ASModel)
        {
            if (repository.Get(ASModel.ApplicationId, out Application app))
            {
                if (serverRepository.Get(ASModel.ServerInformationId, out Server server))
                {
                    string query = $"DELETE FROM Application_ServerInfo WHERE ApplicationId={ASModel.ApplicationId} AND ServerInformationId={ASModel.ServerInformationId}";
                    QueryDB(query);
                    return Ok();
                }
                return NotFound();
            }
            return NotFound();
        }

        private IEnumerable<Server> ReadDatabase3(string query)
        {
            //string connStr = Cyber_Ark.GetConnectionString();
            string connStr = config.GetConnectionString("DefaultConnection");
            using SqlConnection con = new SqlConnection(connStr);
            con.Open();
            using SqlCommand command = new SqlCommand(query, con);
            using SqlDataReader reader = command.ExecuteReader();

            List<Server> output = new List<Server>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Server newServer = new Server()
                    {
                        Id = reader.GetInt32(0),
                        FullName = reader.GetString(3),
                        IPAddress = reader.GetString(1),
                        Type = reader.GetString(5),
                        OperatingSystem = reader.GetString(4),
                        DateCreated = reader.GetDateTime(2)
                    };
                    output.Add(newServer);
                }
            }
            else
            {
                //Console.WriteLine("No rows found.");
            }
            reader.Close();
            return output;
        }

        private void QueryDB(string query)
        {
            // string connStr = Cyber_Ark.GetConnectionString();
            string connStr = config.GetConnectionString("DefaultConnection");
            using SqlConnection con = new SqlConnection(connStr);
            con.Open();
            using SqlCommand command = new SqlCommand(query, con);
            using SqlDataReader reader = command.ExecuteReader();
        }
    }
}