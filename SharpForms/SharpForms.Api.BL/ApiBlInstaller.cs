using SharpForms.Common.BL.Facades;
using SharpForms.Common.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace SharpForms.Api.BL
{
    public class ApiBlInstaller : IInstaller
    {
        public void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(selector =>
                selector.FromAssemblyOf<ApiBlInstaller>()
                        .AddClasses(classes => classes.AssignableTo<IAppFacade>())
                        .AsSelfWithInterfaces()
                        .WithScopedLifetime());
        }
    }
}
