﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence
{
    static class Configuration
    {
        static public string? ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new();
                try
                {
                    configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ECommerceMVC"));
                    configurationManager.AddJsonFile("appsettings.json");
                }
                catch
                {
                    configurationManager.AddJsonFile("appsettings.json");
                }

                return configurationManager.GetConnectionString("PostgreSQL");
            }
        }
    }

}
