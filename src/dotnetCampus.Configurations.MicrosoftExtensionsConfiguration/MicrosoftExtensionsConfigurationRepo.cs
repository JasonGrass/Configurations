using System;
using System.Collections.Generic;
using dotnetCampus.Configurations.Core;
using Microsoft.Extensions.Configuration;

namespace dotnetCampus.Configurations.MicrosoftExtensionsConfiguration
{
    internal class MicrosoftExtensionsConfigurationRepo : ConfigurationRepo
    {
        public MicrosoftExtensionsConfigurationRepo(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override ConfigurationValue? GetValue(string key)
        {
            return ConfigurationValue.Create(Configuration[key]);
        }

        public override void SetValue(string key, ConfigurationValue? value)
        {
            Configuration[key] = value?.Value;
        }

        public override void ClearValues(Predicate<string> keyFilter)
        {
            var removeList = new List<string>();
            foreach (var (key, _) in Configuration.AsEnumerable())
            {
                if (keyFilter(key))
                {
                    removeList.Add(key);
                }
            }

            foreach (var key in removeList)
            {
                SetValue(key, null);
            }
        }

        private IConfiguration Configuration { get; }
    }
}
