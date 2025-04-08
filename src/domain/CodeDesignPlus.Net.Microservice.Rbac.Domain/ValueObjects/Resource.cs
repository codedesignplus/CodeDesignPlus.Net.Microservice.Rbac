using System.Text.Json.Serialization;
using CodeDesignPlus.Net.Microservice.Rbac.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain.ValueObjects;

public sealed partial class Resource
{
    public Guid Id { get; private set; }

    public string Module { get; private set; }

    public string Service { get; private set; }

    public string Controller { get; private set; }

    public string Action { get; private set; }

    public HttpMethodEnum Method { get; private set; }

    [JsonConstructor]
    public Resource(Guid id, string module, string service, string controller, string action, HttpMethodEnum method)
    {
        DomainGuard.GuidIsEmpty(id, Errors.ModuleIdIsInvalid);
        DomainGuard.IsNullOrEmpty(module, Errors.ModuleIsInvalid);
        DomainGuard.IsNullOrEmpty(service, Errors.ServiceIsInvalid);
        DomainGuard.IsNullOrEmpty(controller, Errors.ControllerIsInvalid);
        DomainGuard.IsNullOrEmpty(action, Errors.ActionIsInvalid);
        DomainGuard.IsTrue(method == HttpMethodEnum.None, Errors.MethodIsInvalid);

        this.Id = id;
        this.Module = module;
        this.Service = service;
        this.Controller = controller;
        this.Action = action;
        this.Method = method;
    }

    public static Resource Create(Guid id, string module, string service, string controller, string action, HttpMethodEnum method)
    {
        return new Resource(id, module, service, controller, action, method);
    }
}
