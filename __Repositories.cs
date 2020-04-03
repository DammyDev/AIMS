using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectAPI.Data;
using ProjectAPI.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProjectAPI.Repositories
{
    #region Services
    public interface ISolutionRepository
    {
        bool Get(int Id, out Solution solution);
        PagedList<Solution_> GetAll(PagingParameters parameters, HttpContext httpContext);
        Task<int> AddAsync(Solution solution);
        Solution Update(Solution changes);
        void Delete(int Id);
    }

    public interface IApplicationRepository
    {
        bool Get(int Id, out Application application);
        PagedList<Application> GetAll(PagingParameters parameters, HttpContext httpContext);
        Task<int> AddAsync(Application application);
        Application Update(Application changes);
        void Delete(int Id);
    }

    public interface IDatabaseRepository
    {
        bool Get(int Id, out Database database);
        PagedList<Database> GetAll(PagingParameters parameters, HttpContext httpContext);
        Task<int> AddAsync(Database database);
        Database Update(Database changes);
        void Delete(int Id);
    }    

    public interface IServerRepository
    {
        bool Get(int Id, out Server server);
        PagedList<Server> GetAll(PagingParameters parameters, HttpContext httpContext);
        Task<int> AddAsync(Server server);
        Server Update(Server server);
        void Delete(int Id);
    }
    #endregion

    #region Implementations
    public class SQLSolutionRepository : ISolutionRepository
    {
        private readonly AppDbContext context;
        public SQLSolutionRepository(AppDbContext context)
        {
            this.context = context;
        }

        
        public async Task<int> AddAsync(Solution solution)
        {
            context.Solution.Add(solution);
            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected;
        }

        public void Delete(int Id)
        {
            Solution solution = context.Solution.Find(Id);
            if (solution != null)
            {
                //context.Solution.Remove(solution);
                //context.SaveChanges();
                solution.Status = "Disabled";
            }
            Update(solution);
        }

        public bool Get(int id, out Solution solution)
        {
            solution = context.Solution.Find(id);
            return (solution != null);
        }

        public PagedList<Solution_> GetAll([FromQuery]PagingParameters parameters, HttpContext httpContext)
        {

            // Return List of Solutions  
            var source = (from solution in context.Solution.OrderBy(a => a.Id) select solution).AsQueryable().Where(x => x.Status=="Enabled");

            return PagedList<Solution_>.ToPagedList(source, parameters.PageNumber, parameters.PageSize, httpContext);
        }

        public Solution Update(Solution changes)
        {
            var solution = context.Solution.Attach(changes);
            solution.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return changes;
        }
    }
    public class SQLApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext context;
        public SQLApplicationRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Application application)
        {
            context.Applications.Add(application);
            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected;
        }

        public void Delete(int Id)
        {
            Application application = context.Applications.Find(Id);
            if (application != null)
            {
                //context.Applications.Remove(application);
                //context.SaveChanges();
                application.Status = "Disabled";
            }
            Update(application);
        }

        public bool Get(int id, out Application application)
        {
            application = context.Applications.Find(id);
            return (application != null);
        }

        public PagedList<Application> GetAll([FromQuery]PagingParameters parameters, HttpContext httpContext)
        {
            var source = (from solution in context.Applications.OrderBy(a => a.Id) select solution).AsQueryable().Where(x => x.Status == "Enabled");

            return PagedList<Application>.ToPagedList(source, parameters.PageNumber, parameters.PageSize, httpContext);
        }

        public Application Update(Application changes)
        {
            var Application = context.Applications.Attach(changes);
            Application.State = EntityState.Modified;
            context.SaveChanges();
            return changes;
        }
    }
    public class SQLDatabaseRepository : IDatabaseRepository
    {
        private readonly AppDbContext context;
        public SQLDatabaseRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Database database)
        {
            context.Databases.Add(database);
            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected;
        }

        public void Delete(int Id)
        {
            Database database = context.Databases.Find(Id);
            if (database != null)
            {
                // context.Databases.Remove(database);
                //context.SaveChanges();
                database.Status = "Disabled";
            }
            Update(database);
        }

        public bool Get(int id, out Database database)
        {
            database = context.Databases.Find(id);
            return (database != null);
        }

        public PagedList<Database> GetAll([FromQuery]PagingParameters parameters, HttpContext httpContext)
        {
            var source = (from solution in context.Databases.OrderBy(a => a.Id) select solution).AsQueryable().Where(x => x.Status == "Enabled");

            return PagedList<Database>.ToPagedList(source, parameters.PageNumber, parameters.PageSize, httpContext);
        }

        public Database Update(Database changes)
        {
            var Database = context.Databases.Attach(changes);
            Database.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return changes;
        }
    }
    public class SQLServerInfoRepository : IServerRepository
    {
        private readonly AppDbContext context;
        public SQLServerInfoRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Server server)
        {
            context.ServerInfo.Add(server);
            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected;
        }

        public void Delete(int Id)
        {
            Server server = context.ServerInfo.Find(Id);
            if (server != null)
            {
                //context.ServerInfo.Remove(server);
                //context.SaveChanges();
                server.Status = "Disabled";
            }
            Update(server);
        }

        public bool Get(int id, out Server server)
        {
            server = context.ServerInfo.Find(id);
            return (server != null);
        }

        public PagedList<Server> GetAll([FromQuery]PagingParameters parameters, HttpContext httpContext)
        {
            var source = (from solution in context.ServerInfo.OrderBy(a => a.Id) select solution).AsQueryable().Where(x => x.Status == "Enabled");

            return PagedList<Server>.ToPagedList(source, parameters.PageNumber, parameters.PageSize, httpContext);
        }

        public Server Update(Server changes)
        {
            var Database = context.ServerInfo.Attach(changes);
            Database.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return changes;
        }
    }
    #endregion

}
