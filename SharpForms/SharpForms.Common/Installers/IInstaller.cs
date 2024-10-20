using Microsoft.Extensions.DependencyInjection;

namespace SharpForms.Common.Installers
{
    public interface IInstaller
    {
        void Install(IServiceCollection serviceCollection);
    }
}
