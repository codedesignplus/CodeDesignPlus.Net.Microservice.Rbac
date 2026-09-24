using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("200", "UnknownError");
    public static readonly Error InvalidRequest = new("201", "The request is invalid."); 
    public static readonly Error RbacAlreadyExists = new("202", "The Rbac already exists."); 
    public static readonly Error RbacNotFound = new("203", "The Rbac does not exist.");
    public static readonly Error RbacActive = new("204", "There is already an active Rbac.");
}
