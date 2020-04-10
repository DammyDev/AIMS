using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AIMS.Data;
using AIMS.Repositories.SQL;
using Microsoft.OpenApi.Models;
using Serilog;
using AIMS.Repositories;

namespace AIMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("PagingHeaders");
                    });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AppManager", Version = "v1" });
            });
            services.AddScoped<ISolutionRepository, SQLSolutionRepository>();
            services.AddScoped<IApplicationRepository, SQLApplicationRepository>();
            services.AddScoped<IDatabaseRepository, SQLDatabaseRepository>();
            services.AddScoped<IServerRepository, SQLServerRepository>();
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")
                //options => options.UseSqlServer(Cyber_Ark.GetConnectionString()
                ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
            #if DEBUG
                // For Debug in Kestrel
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppManager API V1");
            #else
                // To deploy on IIS
                c.SwaggerEndpoint("/AppManager/swagger/v1/swagger.json", "AppManager API V1");
            #endif
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging(); //serilog

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }
}
