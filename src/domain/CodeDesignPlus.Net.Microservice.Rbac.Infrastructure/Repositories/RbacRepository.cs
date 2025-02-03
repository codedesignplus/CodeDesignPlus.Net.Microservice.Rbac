namespace CodeDesignPlus.Net.Microservice.Rbac.Infrastructure.Repositories;

public class RbacRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<RbacRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IRbacRepository
{
   
}