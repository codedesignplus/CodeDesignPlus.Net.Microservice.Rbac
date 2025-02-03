using CodeDesignPlus.Net.Microservice.Rbac.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

public sealed partial class Resource
{
    [GeneratedRegex(@"^0x[0-9]{32}$")]
    private static partial Regex Regex();

    public string Module { get; private set; }

    public string Service { get; private set; }

    public string Controller { get; private set; }

    public string Action { get; private set; }

    public HttpMethodEnum Method { get; private set; }

    private Resource(Guid id, string module, string service, string controller, string action, HttpMethodEnum method)
    {
        DomainGuard.IsNullOrEmpty(module, Errors.ModuleIsInvalid);
        DomainGuard.IsNullOrEmpty(service, Errors.ServiceIsInvalid);
        DomainGuard.IsNullOrEmpty(controller, Errors.ControllerIsInvalid);
        DomainGuard.IsNullOrEmpty(action, Errors.ActionIsInvalid);
        DomainGuard.IsTrue(method == HttpMethodEnum.None, Errors.MethodIsInvalid);

        DomainGuard.IsFalse(Regex().IsMatch(module), Errors.UnknownError);

        this.Module = module;
        this.Service = service;
        this.Controller = controller;
        this.Action = action;
        this.Method = method;
    }

    public static Resource Create(string module, string service, string controller, string action, HttpMethodEnum method)
    {
        return new Resource(Guid.NewGuid(), module, service, controller, action, method);
    }
}
