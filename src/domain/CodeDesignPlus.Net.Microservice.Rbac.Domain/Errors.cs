using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Rbac.Domain;

public class Errors : IErrorCodes
{
    public static readonly Error UnknownError = new("100");
    public static readonly Error IdRoleIsInvalid = new("101");
    public static readonly Error NameRoleIsInvalid = new("102");
    public static readonly Error ModuleIsInvalid = new("103");
    public static readonly Error ServiceIsInvalid = new("104");
    public static readonly Error ControllerIsInvalid = new("105");
    public static readonly Error ActionIsInvalid = new("106");
    public static readonly Error MethodIsInvalid = new("107");
    public static readonly Error RbacNameIsInvalid = new("108");
    public static readonly Error DescriptionRoleIsInvalid = new("109");
    public static readonly Error RbacIdIsInvalid = new("110");
    public static readonly Error CreatedByIsInvalid = new("111");
    public static readonly Error UpdatedByIsInvalid = new("112");
    public static readonly Error DeletedByIsInvalid = new("113");
    public static readonly Error PermissionIdIsInvalid = new("114");
    public static readonly Error RoleIsInvalid = new("115");
    public static readonly Error ResourceIsInvalid = new("116");
    public static readonly Error PermissionNotFound = new("117");
    public static readonly Error ModuleIdIsInvalid = new("205");
}
