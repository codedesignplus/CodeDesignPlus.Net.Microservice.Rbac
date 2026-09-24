using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain;

public class Errors : IErrorCodes
{
    public static readonly Error UnknownError = new("100", "UnknownError");
    public static readonly Error IdRoleIsInvalid = new("101", "The id of the role is invalid.");
    public static readonly Error NameRoleIsInvalid = new("102", "The name of the role is invalid.");
    public static readonly Error ModuleIsInvalid = new("103", "The module is invalid.");
    public static readonly Error ServiceIsInvalid = new("104", "The service is invalid.");
    public static readonly Error ControllerIsInvalid = new("105", "The controller is invalid.");
    public static readonly Error ActionIsInvalid = new("106", "The action is invalid.");
    public static readonly Error MethodIsInvalid = new("107", "The method is invalid.");
    public static readonly Error RbacNameIsInvalid = new("108", "The name of the rbac is invalid.");
    public static readonly Error DescriptionRoleIsInvalid = new("109", "The description of the role is invalid.");
    public static readonly Error RbacIdIsInvalid = new("110", "The id of the rbac is invalid.");
    public static readonly Error CreatedByIsInvalid = new("111", "The created by is invalid.");
    public static readonly Error UpdatedByIsInvalid = new("112", "The updated by is invalid.");
    public static readonly Error DeletedByIsInvalid = new("113", "The deleted by is invalid.");
    public static readonly Error PermissionIdIsInvalid = new("114", "The id of the permission is invalid.");
    public static readonly Error RoleIsInvalid = new("115", "The role is invalid.");
    public static readonly Error ResourceIsInvalid = new("116", "The resource is invalid.");
    public static readonly Error PermissionNotFound = new("117", "The permission was not found.");
    public static readonly Error ModuleIdIsInvalid = new("205", "The id of the module is invalid.");
}
