namespace CodeDesignPlus.Net.Microservice.Rbac.Application;

public class Errors: IErrorCodes
{    
    public const string UnknownError = "200 : UnknownError";
    public  const string InvalidRequest = "201 : The request is invalid."; 
    public  const string RbacAlreadyExists = "202 : The Rbac already exists."; 
    public  const string RbacNotFound = "203 : The Rbac does not exist."; 
}
