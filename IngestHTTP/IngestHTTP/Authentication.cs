using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IngestHTTP.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;

namespace IngestHTTP
{
    public class Authentication
    {
        private readonly IConfiguration _configuration;
        private String Token = String.Empty;
        public Authentication(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string AuthenticateData()
        {
            try
            {
                var postData = new AuthRequest
                {
                    username = _configuration["Username"],
                    password = _configuration["Password"],
                    companyid = Convert.ToInt32(_configuration["CompanyId"]),
                };
                var client = new HttpClient();
                var baseAddress = _configuration["IngestHTTPBaseAddress"];
                client.BaseAddress = new Uri(baseAddress);

                var json = JsonSerializer.Serialize(postData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.PostAsync("authenticate", content).Result;

                if (response.IsSuccessStatusCode == true)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var postResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent);
                    Token = GenerateToken(postResponse.jwtToken);
                    return Token;
                }
                else
                {
                    Logger logger = new Logger(_configuration);
                    logger.ErrorLogData(null, "Response Status Code Failed");                
                }

               
            }
            catch (Exception ex)
            {
                Logger logger = new Logger(_configuration);
                logger.ErrorLogData(ex, ex.Message);             
            }
            return Token;
        }

        public string GenerateToken(string req)
        {
            if (req.StartsWith("Bearer"))
            {
                return req.Substring(7);
            }
            else
            {
                return null;
            }
        }
    }
}
