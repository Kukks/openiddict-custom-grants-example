using System.IO;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openiddict_test.Contracts;

namespace openiddict_test.Services
{
    public class FacebookService : ISocialService
    {
        public bool VerifyAccessToken(string accessToken, out object response)
        {
            var requestUrl = $"https://graph.facebook.com/me?access_token={accessToken}";


           
            using (var httpClient = new HttpClient())
            {
                var response1 = httpClient.GetAsync(requestUrl).Result;
                using (var stream = response1.Content.ReadAsStreamAsync().Result)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        response = JObject.Parse(reader.ReadToEnd());
                    }
                }


                return response1.StatusCode == HttpStatusCode.OK;
            }


        }
    }
}