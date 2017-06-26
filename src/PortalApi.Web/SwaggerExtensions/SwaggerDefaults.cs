using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using PortalApi.Common;
using Swashbuckle.Swagger;

namespace SwaggerExtensions
{
    public class DefaultValuesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                return;

            IDictionary<string, object> parameterValuePairs =
            GetParameterValuePairs(apiDescription.ActionDescriptor);

            foreach (var param in operation.parameters)
            {
                if (param.schema != null && param.schema.@ref != null)
                {
                    string schemaName = param.schema.@ref.Split('/').LastOrDefault();
                    if (schemaRegistry.Definitions.ContainsKey(schemaName))
                        foreach (var props in schemaRegistry.Definitions[schemaName].properties)
                        {
                            if (parameterValuePairs.ContainsKey(props.Key))
                                props.Value.@default = parameterValuePairs[props.Key];
                        }
                }
                var parameterValuePair = parameterValuePairs.FirstOrDefault(p => p.Key.IndexOf(param.name, StringComparison.InvariantCultureIgnoreCase) >= 0);
                param.@default = parameterValuePair.Value;
            }
        }

        private IDictionary<string, object> GetParameterValuePairs(HttpActionDescriptor actionDescriptor)
        {
            var env = (SwaggerEnvEnum)Enum.Parse(typeof(SwaggerEnvEnum), App.Configuration.Environment);

            IDictionary<string, object> parameterValuePairs = new Dictionary<string, object>();

            foreach (SwaggerDefaultValueAttribute defaultValue in actionDescriptor.GetCustomAttributes<SwaggerDefaultValueAttribute>())
            {
                if (defaultValue.SwaggerEnv.HasFlag(env))
                {
                    parameterValuePairs.Add(defaultValue.Name, defaultValue.Value);
                }
            }

            foreach (var parameter in actionDescriptor.GetParameters())
            {
                if (!parameter.ParameterType.IsPrimitive)
                {
                    foreach (PropertyInfo property in parameter.ParameterType.GetProperties())
                    {
                        var defaultValue = GetDefaultValue(property);

                        if (defaultValue != null)
                        {
                            parameterValuePairs.Add(property.Name, defaultValue);
                        }
                    }
                }
            }

            return parameterValuePairs;
        }

        private static object GetDefaultValue(PropertyInfo property)
        {
            var customAttribute = property.GetCustomAttributes<SwaggerDefaultValueAttribute>().FirstOrDefault();

            if (customAttribute != null)
            {
                var env = (SwaggerEnvEnum)Enum.Parse(typeof(SwaggerEnvEnum), App.Configuration.Environment);

                if (customAttribute.SwaggerEnv.HasFlag(env))
                {
                    return customAttribute.Value;
                }
            }

            return null;
        }
    }
}