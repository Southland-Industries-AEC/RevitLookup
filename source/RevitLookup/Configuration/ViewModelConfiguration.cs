using Microsoft.Extensions.DependencyInjection;

namespace RevitLookup.Configuration;

public static class ViewModelConfiguration
{
    public static void AddViewModels(this IServiceCollection services)
    {
        services.Scan(selector => selector.FromAssemblyOf<Application>()
            .AddClasses(filter => filter.Where(type => type.Name.EndsWith("ViewModel")))
            .AsImplementedInterfaces(type => type.Name.EndsWith("ViewModel"))
            .WithScopedLifetime());
    }
}