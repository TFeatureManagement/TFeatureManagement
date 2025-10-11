using Microsoft.Extensions.DependencyInjection;

namespace TFeatureManagement.DependencyInjection
{
    public static class ServiceCollectionDecoratorExtensions
    {
        public static void Decorate<TService, TDecoratorImplementation>(this IServiceCollection services)
        {
            var serviceDescriptorToDecorate = services.LastOrDefault(x => x.ServiceType.Equals(typeof(TService)));
            if (serviceDescriptorToDecorate != null)
            {
                if (serviceDescriptorToDecorate.ImplementationType != null)
                {
                    services.Add(new ServiceDescriptor(serviceDescriptorToDecorate.ImplementationType, serviceDescriptorToDecorate.ImplementationType, serviceDescriptorToDecorate.Lifetime));
                    services.Add(new ServiceDescriptor(serviceDescriptorToDecorate.ServiceType, (sp) => sp.GetRequiredService(serviceDescriptorToDecorate.ImplementationType), serviceDescriptorToDecorate.Lifetime));

                    var decoratorServiceDescriptor = new ServiceDescriptor(
                        serviceDescriptorToDecorate.ServiceType,
                        (serviceProvider) =>
                        {
                            var instanceToDecorate = serviceProvider.GetRequiredService(serviceDescriptorToDecorate.ImplementationType);
                            return ActivatorUtilities.CreateInstance(serviceProvider, typeof(TDecoratorImplementation), instanceToDecorate);
                        },
                        serviceDescriptorToDecorate.Lifetime);
                    services.Add(decoratorServiceDescriptor);
                }
                else if (serviceDescriptorToDecorate.ImplementationInstance != null)
                {
                    var decoratorServiceDescriptor = new ServiceDescriptor(
                        serviceDescriptorToDecorate.ServiceType,
                        (serviceProvider) =>
                        {
                            return ActivatorUtilities.CreateInstance(serviceProvider, typeof(TDecoratorImplementation), serviceDescriptorToDecorate.ImplementationInstance);
                        },
                        serviceDescriptorToDecorate.Lifetime);
                    services.Add(decoratorServiceDescriptor);
                }
                else if (serviceDescriptorToDecorate.ImplementationFactory != null)
                {
                    var decoratorServiceDescriptor = new ServiceDescriptor(
                        serviceDescriptorToDecorate.ServiceType,
                        (serviceProvider) =>
                        {
                            var instanceToDecorate = serviceDescriptorToDecorate.ImplementationFactory(serviceProvider);
                            return ActivatorUtilities.CreateInstance(serviceProvider, typeof(TDecoratorImplementation), instanceToDecorate);
                        },
                        serviceDescriptorToDecorate.Lifetime);
                    services.Add(decoratorServiceDescriptor);
                }
            }
        }    
    }
}
