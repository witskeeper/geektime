using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace ConfigurationCustom
{
    class MyConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MyConfigurationProvider();
        }
    }
}
