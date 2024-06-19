using Azure.Identity;
using Microsoft.Extensions.Configuration;

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

