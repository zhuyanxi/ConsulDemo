using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Hosting;

namespace ConsumerWebApi
{
    public static class ConsulExtension
    {
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, ConsulOption option)
        {
            var consulClient = new ConsulClient(t =>
            {
                t.Address = new Uri("http://localhost:8500");
            });

            var healthCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{option.IP}:{option.Port}/api/HealthCheck",//健康检查地址
                Timeout = TimeSpan.FromSeconds(50)
            };

            var registConsul = new AgentServiceRegistration()
            {
                //Checks = new[] { healthCheck },
                Check = healthCheck,
                ID = Guid.NewGuid().ToString(),
                Name = option.ServiceName,
                Address = option.IP,
                Port = option.Port,
                Tags = new[] { $"urlprefix-/{option.ServiceName}" },
            };

            consulClient.Agent.ServiceRegister(registConsul).Wait();
            lifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registConsul.ID).Wait();
            });

            return app;
        }
    }
}