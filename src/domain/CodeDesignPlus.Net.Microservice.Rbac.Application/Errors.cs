using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("200");
    public static readonly Error InvalidRequest = new("201"); 
    public static readonly Error RbacAlreadyExists = new("202"); 
    public static readonly Error RbacNotFound = new("203");
    public static readonly Error RbacActive = new("204");
}
