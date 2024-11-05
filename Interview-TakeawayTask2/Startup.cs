using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using InterviewTakeawayTask2.Repositories;

namespace InterviewTakeawayTask2
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Register UserRepository as a singleton service
            //makes sure that only one instance of UserRepository is created and shared across all requests, guess in the real service we would be interacting with a database.
            services.AddSingleton<UserRepository>();

            // Add controller services
            services.AddControllers();

            // Register Swagger generator. Is fairly straightforward, just setting up the swagger documentation for the API. Could add some further documentation for the endpoints on swagger (Markup with attributes etc).
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "User API",
                    Version = "v1",
                    Description = "A simple API to manage users"
                });
            });
        }

        //Pretty straightforward startup, just setting up swagger, enabling/mapping routing and controllers, and setting up a singleton service.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
                c.RoutePrefix = string.Empty;  // This makes Swagger UI available at the app's root
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
