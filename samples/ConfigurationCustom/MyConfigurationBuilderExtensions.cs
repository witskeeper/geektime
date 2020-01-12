using System;
using System.Collections.Generic;
using System.Text;
using ConfigurationCustom;
namespace Microsoft.Extensions.Configuration
{
    public static class MyConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddMyConfiguration(this IConfigurationBuilder builder)
        {
            builder.Add(new MyConfigurationSource());
            return builder;
        }
    }
}
