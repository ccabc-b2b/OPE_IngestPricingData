using IngestHTTP.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

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
                    username = "ope.gccb@coca-cola.com",
                    password = "0p3G((b@2024",
                    companyid = 0,
                };
                var client = new HttpClient();
                var baseAddress ="http://10.165.12.50:7000/";
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
