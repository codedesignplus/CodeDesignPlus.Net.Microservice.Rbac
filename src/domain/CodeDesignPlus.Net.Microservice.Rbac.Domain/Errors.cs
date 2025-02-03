namespace CodeDesignPlus.Net.Microservice.Rbac.Domain;

public class Errors : IErrorCodes
{
    public const string UnknownError = "100 : UnknownError";
    public const string IdRoleIsInvalid = "101 : The id of the role is invalid.";
    public const string NameRoleIsInvalid = "102 : The name of the role is invalid.";
    public const string ModuleIsInvalid = "103 : The module is invalid.";
    public const string ServiceIsInvalid = "104 : The service is invalid.";
    public const string ControllerIsInvalid = "105 : The controller is invalid.";
    public const string ActionIsInvalid = "106 : The action is invalid.";
    public const string MethodIsInvalid = "107 : The method is invalid.";
    public const string RbacNameIsInvalid = "108 : The name of the rbac is invalid.";
    public const string DescriptionRoleIsInvalid = "109 : The description of the role is invalid.";
    public const string RbacIdIsInvalid = "110 : The id of the rbac is invalid.";
    public const string CreatedByIsInvalid = "111 : The created by is invalid.";
    public const string UpdatedByIsInvalid = "112 : The updated by is invalid.";
    public const string DeletedByIsInvalid = "113 : The deleted by is invalid.";
    public const string PermissionIdIsInvalid = "114 : The id of the permission is invalid.";
    public const string RoleIsInvalid = "115 : The role is invalid.";
    public const string ResourceIsInvalid = "116 : The resource is invalid.";
    public const string PermissionNotFound = "117 : +The permission was not found.";
}
