using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NubanAccountDetails.Paystack;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace NubanAccountDetails.Flutterwave
{
    public class FlutterwaveResoleAccount
    {
        private readonly HttpClient _client;
        public static JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
        };

        public IResolveAccount ResolveAccount { get; }

        public FlutterwaveResoleAccount(string secretKey)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            _client = new HttpClient { BaseAddress = new Uri("https://api.flutterwave.com/v3/") };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ResolveAccount = new ResolveAccount(this);
        }


        public static TR ParseAndResolveMetadata<TR>(ref string rawJson, KeyValuePair<string, string> bankDetails) where TR : IResponse
        {
            JObject jObject = JObject.Parse(rawJson);

            jObject["status"] = jObject["status"].ToString();
            if (jObject["data"] is JObject dataObject && dataObject["metadata"] is JToken metadataToken)
            {
                dataObject["metadata"] = JsonConvert.DeserializeObject<JObject>(metadataToken.ToString());
            }

            if (bankDetails.Key != null && jObject["data"] is JObject data)
            {
                data["bank_name"] = bankDetails.Value.ToUpper();
                data["bank_Id"] = bankDetails.Key;
            }

            rawJson = jObject.ToString();
            TR val = JsonConvert.DeserializeObject<TR>(rawJson);

            if (val is IHasRawResponse hasRawResponse)
            {
                hasRawResponse.RawJson = rawJson;
            }

            return val;
        }


        private static string PrepareRequest<T>(T request)
        {
            (request as IPreparable)?.Prepare();
            return JsonConvert.SerializeObject(request, Formatting.Indented, SerializerSettings);
        }


        public async Task<TResponse> PostAsync<TResponse, T>(string relativeUrl, T request, KeyValuePair<string, string> value) where TResponse : IResponse
        {
            StringContent content = new StringContent(PrepareRequest(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(relativeUrl.TrimStart('/'), content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }

            string rawJson = await response.Content.ReadAsStringAsync();
            return ParseAndResolveMetadata<TResponse>(ref rawJson, value);
        }

        /*private T Post<T>(string relativeUrl,T request, KeyValuePair<string, string> value )
        {
            StringContent content = null;
            if (request != null)
            {
                content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            }

            string text = "?";
            if (value.Key != null)
            {
                
                    text = text + queryParameter.Key + "=" + queryParameter.Value + "&";
                
            }

            string result = _client.PostAsync(relativeUrl + text, content).Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(result);
        }*/
    }
}
