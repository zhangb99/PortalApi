using System;

namespace PortalApi.Common
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class SwaggerDefaultValueAttribute : Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public SwaggerEnvEnum SwaggerEnv { get; set; }

        public SwaggerDefaultValueAttribute(string value)
        {
            this.Value = value;
            SwaggerEnv = SwaggerEnvEnum.DEBUG | SwaggerEnvEnum.TEST;
        }

        public SwaggerDefaultValueAttribute(string name, string value)
            : this(value)
        {
            this.Name = name;
            SwaggerEnv = SwaggerEnvEnum.DEBUG | SwaggerEnvEnum.TEST;
        }
    }

    [Flags]
    public enum SwaggerEnvEnum
    {
        DEBUG = 1,
        TEST = 2,
        PROD = 4,
        ALL = 7
    }
}
