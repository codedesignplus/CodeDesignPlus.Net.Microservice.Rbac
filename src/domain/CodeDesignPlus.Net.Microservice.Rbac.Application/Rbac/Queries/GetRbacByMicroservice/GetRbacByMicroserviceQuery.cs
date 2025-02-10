namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Queries.GetRbacByMicroservice;

public record GetRbacByMicroserviceQuery(string Microservice) : IRequest<List<RbacResourceDto>>;

public class Validator : AbstractValidator<GetRbacByMicroserviceQuery>
{
    public Validator()
    {
        this.RuleFor(x => x.Microservice).NotNull().NotEmpty();
    }
}
