namespace CodeDesignPlus.Net.Microservice.Rbac.Domain.Enums;

public enum HttpMethodEnum
{
    None,
    POST,
    PUT,
    DELETE,
    // Era "PATH", una errata. Se guarda el numero (4), no el nombre, asi que renombrarlo no toca los datos.
    PATCH,
    GET
}
