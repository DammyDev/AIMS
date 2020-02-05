using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjectAPI.Data;
using ProjectAPI.Enums;
using ProjectAPI.Repositories;
using System.Configuration;
using System;
using ProjectAPI.Pagination;
using System.IO;

namespace ProjectAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionsController : ControllerBase
    {
        private readonly ISolutionRepository solutionRepository;
        private readonly IApplicationRepository applicationRepository;
        private readonly IDatabaseRepository databaseRepository;
        private readonly IConfiguration config;

        public SolutionsController(IConfiguration iConfig, ISolutionRepository _solutionRepository, IApplicationRepository _applicationRepository, IDatabaseRepository _databaseRepository)
        {
            solutionRepository = _solutionRepository;
            applicationRepository = _applicationRepository;
            databaseRepository = _databaseRepository;
            config = iConfig;
        }

        // GETALL: api/solutions/5
        [HttpGet]
        public IEnumerable<Solution_> Get([FromQuery] PagingParameters parameters)
        {
            return solutionRepository.GetAll(parameters, HttpContext);
        }

        // GET: api/solutions/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            if (!solutionRepository.Get(id, out var solution))
            {
                return NotFound();
            }
            return Ok(solution);
        }

        // ADD: api/solutions
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Solution solution)
        {
            var fileStream = new MemoryStream(solution.ProjectDocument)
            {
                Position = 0  //this must be reset always
            };

            await solutionRepository.AddAsync(solution);
            return CreatedAtAction(nameof(GetById), new { id = solution.Id }, solution);
        }

        // DELETE: api/solutions/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (!solutionRepository.Get(id, out var solution))
            {
                return NotFound();
            }
            solutionRepository.Delete(id);
            return Ok(solution);
        }

        // UPDATE: api/solutions/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, Solution solution)
        {
            if (id != solution.Id)
            {
                return BadRequest();
            }
            solutionRepository.Update(solution);
            return Ok(solution);
        }

        #region  CustomMethods

        // GET: api/solutions/5
        [HttpGet("GetApps/{id}")]
        public IEnumerable<Application> GetApplications(int id)
        {
            if (!solutionRepository.Get(id, out var solution))
            {
                //return NotFound();
            }

            var query = $"select b.* from Solution a, Applications b, Solution_Application c where a.Id = c.SolutionId  and b.Id = c.ApplicationId  and a.Id = '{id.ToString()}'";
                       
            return ReadDatabase1(query);
        }
        // GET: api/solutions/5
        [HttpGet("GetDbs/{id}")]
        public IEnumerable<Database> GetDatabases(int id)
        {
            if (!solutionRepository.Get(id, out var solution))
            {
                //return NotFound();
            }

            var query = $"select b.* from Solution a, Databases b, Solution_Database c where a.Id = c.SolutionId  and b.Id = c.DatabaseId  and a.Id = '{id.ToString()}'";

            return ReadDatabase2(query);
        }
        // ADD a new application to solution: api/solution/1
        [HttpPost("AddNewApp/{id}")]
        public async Task<IActionResult> AddNewApplicationAsync([FromRoute] int id, [FromBody]Application app,  bool isAppInternal)
        {
            if (solutionRepository.Get(id, out Solution solution))
            {
                await applicationRepository.AddAsync(app);

                string query = $"INSERT INTO Solution_Application (SolutionId,ApplicationId,DateCreated,IsInternal) " +
                    $"VALUES('{solution.Id.ToString()}', '{app.Id.ToString()}', GETDATE(), '{isAppInternal}')";

                UpdateDatabase(query);
                //return CreatedAtAction(nameof(GetById), new { id = solution.Id }, solution);
                return CreatedAtAction(nameof(GetById), new { id = app.Id }, app);
            }
            return NotFound();
        }

        // ADD an existing application to solution: api/solution/1
        [HttpPost("AddExistingApp/")]
        public IActionResult AddExistingApplication(SolutionApplication SAModel)
        {
            if (solutionRepository.Get(SAModel.SolutionId, out Solution solution))
            {
                if (applicationRepository.Get(SAModel.ApplicationId, out Application app))
                {
                    string query = $"INSERT INTO Solution_Application (SolutionId,ApplicationId,DateCreated,IsInternal) " +
                        $"VALUES('{solution.Id.ToString()}', '{app.Id.ToString()}', GETDATE(), '{SAModel.IsInternal}')";
                    UpdateDatabase(query);
                    return CreatedAtAction(nameof(GetById), new { id = app.Id }, app);
                }
            }
            return NotFound();
        }

        //ADD a new database to solution: api/solution/1
        [HttpPost("AddNewDb/{id}")]
        public async Task<IActionResult> AddNewDatabaseAsync([FromRoute] int id, [FromBody] Database database)
        {
            if (solutionRepository.Get(id, out Solution solution))
            {
                await databaseRepository.AddAsync(database);
                string query = $"INSERT INTO Solution_Database (SolutionId,DatabaseId,DateCreated) " +
                    $"VALUES('{solution.Id.ToString()}', '{database.Id.ToString()}', GETDATE())";
                UpdateDatabase(query);
                return CreatedAtAction(nameof(GetById), new { id = database.Id }, database);
            }
            return NotFound();
        }

        // ADD an existing database to solution: api/solution/1
        [HttpPost("AddExistingDb/")]
        public IActionResult AddExistingDatabase(SolutionDatabase SDModel)
        {
            if (solutionRepository.Get(SDModel.SolutionId, out Solution solution))
            {
                if (databaseRepository.Get(SDModel.DatabaseId, out Database database))
                {
                    string query = $"INSERT INTO Solution_Database (SolutionId,DatabaseId,DateCreated) " +
                        $"VALUES('{solution.Id.ToString()}', '{database.Id.ToString()}', GETDATE())";
                    UpdateDatabase(query);
                    return CreatedAtAction(nameof(GetById), new { id = database.Id }, database);
                }
            }
            return NotFound();
        }
        #endregion

        #region Helper Methods
        public static void UpdateDatabase(string query)
        {
            string connStr = "Server=172.27.4.135;Database=dbAppManager2;uid=batappuser;password=bat*987User$2;";
            //string connStr = "Server=(localdb)\\MSSQLLocalDB;Database=dbAccounts;Trusted_Connection=True;MultipleActiveResultSets=true";
            using SqlConnection con = new SqlConnection(connStr);
            con.Open();
            using SqlCommand command = new SqlCommand(query, con);
            using SqlDataReader reader = command.ExecuteReader();
        }

        private IEnumerable<Application> ReadDatabase1(string query)
        {
            string connStr = "Server=172.27.4.135;Database=dbAppManager2;uid=batappuser;password=bat*987User$2;";
            //string connStr = "Server=(localdb)\\MSSQLLocalDB;Database=dbAccounts;Trusted_Connection=True;MultipleActiveResultSets=true";
            using SqlConnection con = new SqlConnection(connStr);
            con.Open();
            using SqlCommand command = new SqlCommand(query, con);
            using SqlDataReader reader = command.ExecuteReader();

            List<Application> output = new List<Application>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Application newApp = new Application()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Type = reader.GetString(3),
                        Language = reader.GetString(4),
                        FullPath = reader.GetString(5),
                        DateCreated = reader.GetDateTime(6)
                    };
                    output.Add(newApp);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();
            return output;
        }
        private IEnumerable<Database> ReadDatabase2(string query)
        {
            string connStr = "Server=172.27.4.135;Database=dbAppManager2;uid=batappuser;password=bat*987User$2;";
            //string connStr = "Server=(localdb)\\MSSQLLocalDB;Database=dbAccounts;Trusted_Connection=True;MultipleActiveResultSets=true";
            using SqlConnection con = new SqlConnection(connStr);
            con.Open();
            using SqlCommand command = new SqlCommand(query, con);
            using SqlDataReader reader = command.ExecuteReader();

            List<Database> output = new List<Database>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Database newDb = new Database()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Engine = reader.GetString(2),
                        ServerName = reader.GetString(3),
                        UserName = reader.GetString(4),
                        Password = reader.GetString(5),
                        DateCreated = reader.GetDateTime(6)
                    };
                    output.Add(newDb);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();
            return output;
        }
        #endregion
    }
}