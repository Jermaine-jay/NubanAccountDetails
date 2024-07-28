using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace NubanAccountDetails.Services
{
    public class GetDetails : IGetDetails
    {
        private readonly IConfiguration _configuration;
        public GetDetails(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /*public async Task<object> ResolveAccountNumber(string apiKey, string apiUrl, Dictionary<string, string> dict)
        {
            var tasks = dict.Select(code => ResolveAccountNumberAsync(apiKey, apiUrl + code.Key, code));
            var completedTask = await Task.WhenAny(tasks);

            if (completedTask != null && completedTask.Result != null)
            {
                return completedTask.Result;
            }

            return "Account does not exist!";
        }*/


        public async Task<object> ResolveAccountNumber(string apiKey, string apiUrl, Dictionary<string, string> dict)
        {
            var tasks = dict.Select(code => ResolveAccountNumberAsync(apiKey, apiUrl + code.Key, code));
            while (tasks.Any())
            {
                var completedTask = await Task.WhenAny(tasks);

                if (completedTask.Result != null)
                {
                    return completedTask.Result;
                }
                var result = new Response
                {
                    Status = "false",
                    Message = "Account not found"
                };
                return result;
            }

            return HttpStatusCode.BadRequest;
        }


        public async Task<object> ResolveAccountNumber(string apiKey, string apiUrl, string accountNumber, Dictionary<string, string> dict)
        {
            var tasks = dict.Select(code => ResolveAccountNumberAsync(apiKey, apiUrl, accountNumber, code));
            while (tasks.Any())
            {
                var completedTask = await Task.WhenAny(tasks);

                if (completedTask.Result != null)
                {
                    return completedTask.Result;
                }

                tasks = tasks.Except(new[] { completedTask });
            }
            return tasks.Where(c=>c.Status.Equals("false"));
        }


        private async Task<Response> ResolveAccountNumberAsync(string apiKey, string apiUrl, KeyValuePair<string, string> code)
        {
            var recipientResponse = await GetRequest(apiUrl, apiKey);

            if (recipientResponse.IsSuccessStatusCode)
            {
                var listResponse = await recipientResponse.Content.ReadAsStringAsync();
                var getResponse = JsonConvert.DeserializeObject<Response>(listResponse);
                if (getResponse.Status != "false")
                {
                    getResponse.Data.Bank_name = (code.Value).ToUpper();
                    return getResponse;
                }
                return getResponse;
            }

            return null;
        }

        private async Task<Data> ResolveAccountNumberAsync(string apiKey, string apiUrl, string accountNumber, KeyValuePair<string, string> code)
        {
            var request = new PostRequestDto
            {
                account_number = accountNumber,
                account_bank = code.Key,
            };

            var recipientResponse = await PostRequest(apiUrl, apiKey, request);
            if (recipientResponse.IsSuccessStatusCode)
            {
                var listResponse = await recipientResponse.Content.ReadAsStringAsync();
                var getResponse = JsonConvert.DeserializeObject<Response>(listResponse);

                if (getResponse.Status != "false")
                {
                    getResponse.Data.Bank_name = (code.Value).ToUpper();
                    getResponse.Data.Bank_Id = code.Key;
                    return getResponse.Data;
                }
            }

            return null;
        }


        public async Task<HttpResponseMessage> GetRequest(string apiUrl, string apiKey)
        {
            using (var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var cts = new CancellationTokenSource();
                var recipientResponse = await _httpClient.GetAsync(apiUrl, cts.Token);
                return recipientResponse;
            }
        }

        public async Task<HttpResponseMessage> PostRequest<T>(string url, string apiKey, T? request) where T : class
        {
            using (var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var recipientResponse = await _httpClient.PostAsync(url, httpContent);
                return recipientResponse;
            }
        }

        public class PostRequestDto
        {
            public string account_number { get; set; }
            public string account_bank { get; set; }
        }

        public class Response
        {
            public string Status { get; set; }
            public string Message { get; set; }

            public Data Data { get; set; }
        }

        public class Data
        {
            public string Bank_name { get; set; }
            public string Bank_Id { get; set; }
            public string Account_name { get; set; }
            public string Account_number { get; set; }

        }

        public class Result
        {
            public string BankId { get; set; }
            public string AccountName { get; set; }
            public string AccountNumber { get; set; }

        }
    }
}
