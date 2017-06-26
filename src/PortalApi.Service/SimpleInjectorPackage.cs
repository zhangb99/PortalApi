using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using MediatR;
using NPoco;
using PortalApi.Common;
using SimpleInjector;
using SimpleInjector.Packaging;
using Fasterflect;

namespace PortalApi.Service
{
    public class SimpleInjectorPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IDatabase>(() => new Database("DefaultDb"), Lifestyle.Scoped);

            var assemblies = GetAssemblies().ToList();
            
            container.Register<ICacheService, MemoryCacheService>();
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<,>), assemblies);
            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));

            container.RegisterCollection(typeof(IValidator<>), assemblies);
            container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(ValidationRequestDecorator<,>));

            // decorators
            container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(AuditingRequestDecorator<,>),
                c => c.ImplementationType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                    .SelectMany(i => i.GetGenericArguments()).First()
                    .UnderlyingSystemType.Attributes(typeof(AuditRequestAttribute)).Any());

            //container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionRequestDecorator<,>),
            //    c => c.ImplementationType.GetInterfaces()
            //        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
            //        .SelectMany(i => i.GetGenericArguments()).First()
            //        .UnderlyingSystemType.Attributes(typeof(TransactionAttribute)).Any());
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(PingRequest).Assembly;
            yield return typeof(PingHandler).Assembly;
        }
    }
}
