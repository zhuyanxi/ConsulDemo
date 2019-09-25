using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ConsumerWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IHostApplicationLifetime lifetime,
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //var address = Configuration.GetValue<string>("urls");
            var address = Configuration["urls"];
            var logger = loggerFactory.CreateLogger("TestInfo");

            var addArr = address.Split(':');
            var Port = Convert.ToInt32(addArr[2]);
            var IP = addArr[1].Substring(2);

            ConsulOption consulOption = new ConsulOption()
            {
                ServiceName = Configuration["servicename"],
                IP = IP,
                Port = Port,
                ConsulAddress = Configuration["consuladdress"]
            };

            logger.LogWarning($"Consul {consulOption.ServiceName} at {consulOption.ConsulAddress}");
            logger.LogWarning($"Consul {consulOption.ServiceName}'s local service at {address}");

            app.UseConsul(lifetime, consulOption);
        }
    }
}