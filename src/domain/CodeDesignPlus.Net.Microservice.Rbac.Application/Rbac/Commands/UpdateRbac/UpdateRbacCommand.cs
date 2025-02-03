namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Rbac.Commands.UpdateRbac;

[DtoGenerator]
public record UpdateRbacCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<UpdateRbacCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
