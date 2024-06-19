using Azure.Identity;
using FluentAssertions.Common;
using IngestHTTP.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace IngestHTTP
{
    public class Program
    {
        static void Main(string[] args)
        {

            try
            {
                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddAzureKeyVault(new Uri(Properties.Settings.Default.KeyVaultURI), new DefaultAzureCredential());
                IConfiguration configuration = builder.Build();
                
                Authentication authenticate = new Authentication(configuration);
                string token = authenticate.AuthenticateData();
                FetchMasterData jsonData = new FetchMasterData(configuration);
                jsonData.getData(args[0],token);
            }
            catch (Exception ex)
            {
                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddAzureKeyVault(new Uri(Properties.Settings.Default.KeyVaultURI), new DefaultAzureCredential());
                IConfiguration configuration = builder.Build();
                Logger logger = new Logger(configuration);
                logger.ErrorLogData(ex, ex.Message);
            }
            
        }
    }
}

