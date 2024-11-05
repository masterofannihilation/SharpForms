using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Common.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace SharpForms.Api.DAL.Memory
{
    public class ApiDalMemoryInstaller : IInstaller
    {
        public void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(selector =>
                selector.FromAssemblyOf<ApiDalMemoryInstaller>()
                        .AddClasses(classes => classes.AssignableTo(typeof(IApiRepository<>)))
                            .AsMatchingInterface()
                            .WithTransientLifetime()
                        .AddClasses(classes => classes.AssignableTo<Storage>())
                            .AsSelf()
                            .WithSingletonLifetime()
            );
        }
    }
}
