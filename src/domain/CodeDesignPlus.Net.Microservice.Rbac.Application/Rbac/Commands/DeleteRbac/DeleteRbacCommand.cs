namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.DeleteRbac;

[DtoGenerator]
public record DeleteRbacCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteRbacCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
