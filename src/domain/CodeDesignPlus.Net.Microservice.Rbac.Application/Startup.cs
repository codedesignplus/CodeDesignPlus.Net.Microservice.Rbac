using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application
{
    public class Startup : IStartup
    {
        public void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            MapsterConfigRbac.Configure();
        }
    }
}
