using System.Reflection;
using MediatR;
using PortalApi.Web.Controllers;
using System.Web.Http;
using NPoco;
using Owin;
using PortalApi.Service;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Integration.WebApi;

namespace PortalApi.Web
{
    public static class SimpleInjectorWebApi
    {
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize(IAppBuilder app)
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration, Assembly.GetExecutingAssembly());
            container.RegisterPackages();

            using (container.BeginExecutionContextScope())
            {
                GlobalConfiguration.Configuration.Filters.Add(
                    new ApiAuthorizationFilter(container.GetInstance<IDatabase>(), container.GetInstance<ICacheService>()));
            }

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void InitializeContainer(Container container)
        {
            container.RegisterInitializer<BaseApiController>(x =>
            {
                //x._validatorFactory = container.GetInstance<IValidatorFactory>();
                x._mediator = container.GetInstance<IMediator>();
            });

            //container.Register<IValidatorFactory>(() => new FluentValidatorFactory(config));

            //FluentValidationModelValidatorProvider.Configure(config,
            //    provider =>
            //    {
            //        provider.ValidatorFactory = new FluentValidatorFactory(config);
            //    });
        }
    }
}