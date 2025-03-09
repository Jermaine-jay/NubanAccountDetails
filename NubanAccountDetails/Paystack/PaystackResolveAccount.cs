using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Http.Headers;

namespace NubanAccountDetails.Paystack
{
    public class PaystackResolveAccount
    {
        private readonly HttpClient _client;
        public static JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
        };

        public IResolveAccount ResolveAccount { get; }

        public PaystackResolveAccount(string secretKey, string currency = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            _client = new HttpClient { BaseAddress = new Uri("https://api.paystack.co/") };
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ResolveAccount = new ResolveAccount(this, currency);
        }

        public static TR ParseAndResolveMetadata<TR>(ref string rawJson, string bankName = null) where TR : IApiResponse
        {
            JObject jObject = JObject.Parse(rawJson);

            if (jObject["data"] is JObject dataObject && dataObject["metadata"] is JToken metadataToken)
            {
                dataObject["metadata"] = JsonConvert.DeserializeObject<JObject>(metadataToken.ToString());
            }

            if (bankName != null && jObject["data"] is JObject data)
            {
                data["bank_name"] = bankName.ToUpper();
            }

            rawJson = jObject.ToString();
            TR val = JsonConvert.DeserializeObject<TR>(rawJson);

            if (val is IHasRawResponse hasRawResponse)
            {
                hasRawResponse.RawJson = rawJson;
            }

            return val;
        }

        public async Task<TResponse> GetAsync<TResponse, T>(string relativeUrl, T request, string bankName = null) where TResponse : class, IApiResponse
        {
            IPreparable? preparable = request as IPreparable;
            string queryString = request != null ? "?" + request.ToQueryString() : string.Empty;

            preparable?.Prepare();
            HttpResponseMessage response = await _client.GetAsync(relativeUrl.TrimStart('/') + queryString);
            string rawJson = await response.Content.ReadAsStringAsync();
            return ParseAndResolveMetadata<TResponse>(ref rawJson, bankName);
        }

        private static string PrepareRequest<T>(T request)
        {
            (request as IPreparable)?.Prepare();
            return JsonConvert.SerializeObject(request, Formatting.Indented, SerializerSettings);
        }

        public async Task<TResponse> PostAsync<TResponse, T>(string relativeUrl, T request) where TResponse : IApiResponse
        {
            string rawJson = _client.PostAsync(relativeUrl.TrimStart(new char[1] { '/' }), new StringContent(PrepareRequest(request))).Result.Content.ReadAsStringAsync().Result;
            return ParseAndResolveMetadata<TResponse>(ref rawJson);
        }
    }
}
