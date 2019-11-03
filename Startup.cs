using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkipTakeRepro.Models;

namespace SkipTakeRepro
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<MyDbContext>(optionsBuilder => {
                string connectionString = Configuration.GetConnectionString("Default");
                optionsBuilder.UseSqlite(connectionString);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    using var dbContext = context.RequestServices.GetService<MyDbContext>();

                    //When Skip and/or Take is used, the owned types CurrentPrice and FullPrice are null
                    var courseList1 = await dbContext.Courses
                    .Include(course => course.CurrentPrice)
                    .Select(course => CourseViewModel.FromEntity(course))
                    .Skip(0).Take(3)
                    .AsNoTracking()
                    .ToListAsync();
                    await context.Response.WriteAsync($"See? The CurrentPrice is null for these entites fetched with Skip/Take\r\n");
                    foreach (var course in courseList1)
                    {
                        await context.Response.WriteAsync($"Price: {(course.CurrentPrice?.ToString() ?? "null")}\r\n");
                    }

                    //No problem when Skip/Take are omitted
                    var courseList2 = await dbContext.Courses
                    .Include(course => course.CurrentPrice)
                    .Select(course => CourseViewModel.FromEntity(course))
                    .AsNoTracking()
                    .ToListAsync();
                    await context.Response.WriteAsync($"No problem whatsoever if Skip/Take are omitted\r\n");
                    foreach (var course in courseList2)
                    {
                        await context.Response.WriteAsync($"Price: {course.CurrentPrice}\r\n");
                    }
                });
            });
        }
    }
}
