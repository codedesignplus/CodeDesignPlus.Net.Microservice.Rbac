
using CodeDesignPlus.Net.Microservice.Rbac.Domain.Entities;
using MongoDB.Driver.Linq;

namespace CodeDesignPlus.Net.Microservice.Rbac.Infrastructure.Repositories;

public class RbacRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<RbacRepository> logger)
    : RepositoryBase(serviceProvider, mongoOptions, logger), IRbacRepository
{
    public async Task<List<RbacPermissionEntity>> GetPermissionsByMicroserviceAsync(string microservice, CancellationToken cancellationToken)
    {
        var collection = this.GetCollection<RbacAggregate>();

        var filter = Builders<RbacAggregate>.Filter.And(
            Builders<RbacAggregate>.Filter.Eq(x => x.IsActive, true),
            Builders<RbacAggregate>.Filter.ElemMatch(x => x.Permissions, p => p.Resource.Service == microservice)
        );
        
        var projection = Builders<RbacAggregate>.Projection.Expression(rbac => 
            rbac.Permissions.Where(p => p.Resource.Service == microservice).ToList()
        );

        var permissions = await collection
            .Find(filter)
            .Project(projection)
            .FirstOrDefaultAsync(cancellationToken);

        return permissions ?? [];
    }

    public Task<bool> HasActiveRbacAsync(CancellationToken cancellationToken)
    {
        var collection = this.GetCollection<RbacAggregate>();

        var filter = Builders<RbacAggregate>.Filter.Eq(x => x.IsActive, true);

        return collection.Find(filter).AnyAsync(cancellationToken);
    }

    public Task<bool> HasActiveRbacAsync(Guid id, CancellationToken cancellationToken)
    {
        var collection = this.GetCollection<RbacAggregate>();

        var filter = Builders<RbacAggregate>.Filter.And(
            Builders<RbacAggregate>.Filter.Eq(x => x.IsActive, true),
            Builders<RbacAggregate>.Filter.Ne(x => x.Id, id)
        );

        return collection.Find(filter).AnyAsync(cancellationToken);
    }
}