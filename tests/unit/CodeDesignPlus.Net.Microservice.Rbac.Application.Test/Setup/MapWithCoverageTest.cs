using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CodeDesignPlus.Net.Microservice.Rbac.Application.Setup;

namespace CodeDesignPlus.Net.Microservice.Rbac.Application.Test.Setup;

/// <summary>
/// Cada <c>MapWith(src => new XDto { … })</c> asigna todas las propiedades del DTO (pendings/174).
/// </summary>
/// <remarks>
/// Un mapeo escrito a mano olvida en silencio el campo que se añade después al DTO: queda en su valor por defecto y nada
/// falla. En ms-commonareas así se perdió la política de cancelación (pendings/173). La prueba lee la expresión que guardó Mapster para
/// cada regla y la compara con las propiedades del destino, así que un mapeo nuevo entra solo. Los que construyen con
/// constructor (<c>new XCommand(…)</c>) no hacen falta: ahí el compilador ya exige todos los argumentos.
/// </remarks>
public class MapWithCoverageTest
{
    // Propiedad del DTO que un mapeo deja sin asignar a propósito, con su motivo. Vacío: hoy no hay ninguna.
    private static readonly Dictionary<(Type Destination, string Property), string> Unmapped = [];

    [Fact]
    public void Configure_EveryMapWith_AssignsEveryDestinationProperty()
    {
        MapsterConfigRbac.Configure();

        var missing = new List<string>();
        var checkedRules = 0;

        foreach (var (types, rule) in TypeAdapterConfig.GlobalSettings.RuleMap)
        {
            if (rule.Settings.ConverterFactory?.Invoke(null!) is not LambdaExpression { Body: MemberInitExpression body })
                continue;

            checkedRules++;

            var assigned = body.Bindings.Select(binding => binding.Member.Name).ToHashSet();

            missing.AddRange(types.Destination.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.SetMethod is { IsPublic: true })
                .Where(property => !assigned.Contains(property.Name))
                .Where(property => !Unmapped.ContainsKey((types.Destination, property.Name)))
                .Select(property => $"{types.Source.Name} -> {types.Destination.Name}.{property.Name}"));
        }

        Assert.True(checkedRules > 0, "No MapWith with an object initializer was found: the test is not looking where it should.");
        Assert.Empty(missing);
    }
}
