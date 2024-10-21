using Azure.Storage.Blobs;
using IngestHTTP.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace IngestHTTP
{
    public class FetchMasterData
    {   
        readonly static string containerName = Properties.Settings.Default.ContainerName;
        static string blobDirectoryPrefix = Properties.Settings.Default.BlobDirectoryPrefix;
        private readonly IConfiguration _configuration;
        public FetchMasterData(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void getData(string process,string token)
        {
            try
            {
                var client = new HttpClient();
                var baseAddress = _configuration["IngestHTTPBaseAddress"];
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                int count = Convert.ToInt32(_configuration["Count"]);
                var processAddress = _configuration[process + "Address"];
                processAddress = processAddress + "page=1" + "&count=" + count;
                var response = client.GetAsync(processAddress).Result;
                var json = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode == true)
                {
                    ProcessJsonEntity processdataList = JsonConvert.DeserializeObject<ProcessJsonEntity>(json);
                    var total = Convert.ToInt32(processdataList.total);
                    int totalPage = (int)Math.Ceiling((double)total / (double)count);
                    var page = 1;
                    do
                    {
                        processAddress = _configuration[process + "Address"];
                        processAddress = processAddress + "page=" + page + "&count=" + count;
                        response = client.GetAsync(processAddress).Result;
                        json = response.Content.ReadAsStringAsync().Result;

                        BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration["StorageKey"]);
                        BlobContainerClient containerClient = new BlobContainerClient(_configuration["StorageKey"], containerName);
                        using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                        {
                            containerClient.UploadBlob(blobDirectoryPrefix + process + "/" + process + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + page, stream);
                        }
                        page++;
                    } while (page <= totalPage);
                }
                else
                {
                    Logger logger = new Logger(_configuration);
                    logger.ErrorLogData(null,"Response Status Code Failed");
                }
            }
            catch (Exception ex)
            {
                Logger logger = new Logger(_configuration);
                logger.ErrorLogData(ex,ex.Message);
            }
        }
    }
}
