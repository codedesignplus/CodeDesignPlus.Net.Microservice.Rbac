namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.CreateRbac;

[DtoGenerator]
public record CreateRbacCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<CreateRbacCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
