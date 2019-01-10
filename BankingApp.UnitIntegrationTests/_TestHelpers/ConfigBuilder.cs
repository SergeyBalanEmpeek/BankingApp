using Microsoft.Extensions.Configuration;
using System.IO;

namespace BankingApp.UnitIntegrationTests
{
    public class ConfigBuilder
    {
        public static IConfigurationRoot GetInstance()
        {
            string configFile = "appsettings.json";
            string configFolder = Directory.GetCurrentDirectory();

            
            if (File.Exists(configFolder + Path.DirectorySeparatorChar + configFile) == false)
            {
                //Build does not contain a configuration file
                //Try to find a main project file

                while (Path.GetPathRoot(configFolder) != configFolder)                          //stop if disk drive reached
                {
                    configFolder = Directory.GetParent(configFolder).FullName;
                    if (configFolder.EndsWith("BankingApp")) break;                             //project root folder was founded
                }

                configFolder += Path.DirectorySeparatorChar + "BankingApp.WebAPI";              //change to main project
            }


            var builder = new ConfigurationBuilder()
            .SetBasePath(configFolder)
            .AddJsonFile(configFile, false);

            return builder.Build();
        }
    }
}