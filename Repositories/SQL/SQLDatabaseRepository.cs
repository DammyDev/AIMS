using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Data;
using ProjectAPI.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAPI.Repositories.SQL
{
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
}
