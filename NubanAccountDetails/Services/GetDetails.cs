using Newtonsoft.Json;
using NubanAccountDetails.Response;
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

        /*public async Task<object> ResolveFlutterAccountNumber(string apiKey, string apiUrl, Dictionary<string, string> dict)
        {
            var tasks = dict.Select(code => ResolveFlutterAccountNumberAsync(apiKey, apiUrl + code.Key, code));
            var completedTask = await Task.WhenAny(tasks);

            if (completedTask != null && completedTask.Result != null)
            {
                return completedTask.Result;
            }

            return "Account does not exist!";
        }*/


        //public async Task<object> ResolveFlutterAccountNumber(string apiKey, string apiUrl)
        //{
        //    var a = GetBanks(apiKey);
        //    var tasks = a.Select(code => ResolveFlutterAccountNumberAsync(apiKey, apiUrl + code.Key, code)).ToList();
        //    while (tasks.Any())
        //    {
        //        var completedTask = await Task.WhenAny(tasks);
        //        tasks.Remove(completedTask); // Remove completed task from the list
        //        if (await completedTask != null)
        //        {
        //            return completedTask.Result;
        //        }
        //        var result = new ResponseDto
        //        {
        //            Status = "false",
        //            Message = "Account not found"
        //        };
        //        return result;
        //    }

        //    return HttpStatusCode.BadRequest;
        //}


        public async Task<object> ResolveAccountNumber(string apiKey, string apiUrl)
        {
            var banks = await GetBanks(apiKey);
            var tasks = banks.Select(async bank =>
            {
                return await GetBankNameAsync(apiKey, $"{apiUrl}{bank.Key}", bank);
            }).ToList();

            while (tasks.Any())
            {
                var completedTask = await Task.WhenAny(tasks);
                tasks.Remove(completedTask);

                var result = await completedTask;

                if (result != null && result.Status.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return result;
                }
            }

            return new ResponseDto
            {
                Status = "false",
                Message = "Account not found"
            };
        }

        public async Task<object> ResolveFlutterAccountNumber(string apiKey, string apiUrl, string accountNumber, Dictionary<string, string> dict)
        {
            var tasks = dict.Select(code => ResolveFlutterAccountNumberAsync(apiKey, apiUrl, accountNumber, code));
            while (tasks.Any())
            {
                Task<Data> completedTask = await Task.WhenAny(tasks);

                if (completedTask.Result != null)
                {
                    return completedTask.Result;
                }

                tasks = tasks.Except(new[] { completedTask });
            }
            return tasks.Where(c => c.Status.Equals("false"));
        }


        private async Task<ResponseDto> GetBankNameAsync(string apiKey, string apiUrl, KeyValuePair<string, string> code)
        {
            HttpResponseMessage recipientResponse = await GetRequest(apiUrl, apiKey);

            if (recipientResponse.IsSuccessStatusCode)
            {
                string listResponse = await recipientResponse.Content.ReadAsStringAsync();
                ResponseDto getResponse = JsonConvert.DeserializeObject<ResponseDto>(listResponse);
                if (getResponse.Status != "false")
                {
                    getResponse.Data.Bank_name = (code.Value).ToUpper();
                    return getResponse;
                }
                return getResponse;
            }

            return null;
        }

        private async Task<Data> ResolveFlutterAccountNumberAsync(string apiKey, string apiUrl, string accountNumber, KeyValuePair<string, string> code)
        {
            PostRequestDto request = new PostRequestDto
            {
                account_number = accountNumber,
                account_bank = code.Key,
            };

            HttpResponseMessage recipientResponse = await PostRequest(apiUrl, apiKey, request);
            if (recipientResponse.IsSuccessStatusCode)
            {
                string listResponse = await recipientResponse.Content.ReadAsStringAsync();
                ResponseDto getResponse = JsonConvert.DeserializeObject<ResponseDto>(listResponse);

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
            using (HttpClient _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                CancellationTokenSource cts = new CancellationTokenSource();
                HttpResponseMessage recipientResponse = await _httpClient.GetAsync(apiUrl, cts.Token);
                return recipientResponse;
            }
        }

        public async Task<HttpResponseMessage> PostRequest<T>(string url, string apiKey, T? request) where T : class
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                string jsonContent = JsonConvert.SerializeObject(request);
                StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage recipientResponse = await _httpClient.PostAsync(url, httpContent);
                return recipientResponse;
            }
        }

        public async Task<Dictionary<string, string>> GetBanks(string apikey)
        {
            var apiUrl = "https://api.paystack.co/bank?currency=NGN";
            var listofBanksCode = new Dictionary<string, string>();

            var recipientResponse = GetRequest(apiUrl, apikey).Result;

            if (recipientResponse.IsSuccessStatusCode)
            {
                var listResponse = recipientResponse.Content.ReadAsStringAsync().Result;
                var getResponse = JsonConvert.DeserializeObject<ListBankResponse>(listResponse);

                /* var tasks = getResponse.data.Select(async pair =>
                 {
                     //var bankResponse = await _resolveAccount.ResolveFlutterAccountNumber(_apiKey, apiUrl + pair.code, new KeyValuePair<string, string>(pair.code, pair.name));
                     if (pair.code != null && pair.name != null)
                     {
                         listofBanksCode.Add(pair.code, pair.name);
                     }
                 });*/
                foreach (var pair in getResponse.data)
                {
                    if (!string.IsNullOrEmpty(pair.code) && !string.IsNullOrEmpty(pair.name))
                    {
                        listofBanksCode[pair.code] = pair.name; // Using indexer to avoid duplicate key exception
                    }
                }
                //await Task.WhenAll(tasks);
                return listofBanksCode;
            }

            return null;
        }
    }
}
