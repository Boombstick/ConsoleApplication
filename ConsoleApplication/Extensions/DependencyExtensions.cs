using System.Reflection;
using Microsoft.Extensions.DependencyInjection;


namespace ConsoleApplication.Extensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddChildClassesAsSingletone(this IServiceCollection services, Type baseClass)
        {
            var subclassesTypes = Assembly
                .GetAssembly(baseClass)
                .GetTypes()
                .Where(t => t.IsSubclassOf(baseClass) && !t.IsAbstract);

            foreach (var childClass in subclassesTypes)
                services.AddSingleton(baseClass, childClass);

            return services;
        }
    }
}
