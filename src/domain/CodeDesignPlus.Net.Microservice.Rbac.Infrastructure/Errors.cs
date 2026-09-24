using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Rbac.Infrastructure;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("300", "UnknownError");

    public static readonly Error ResourceNotFound = new("301", "The resource was not found");
}
