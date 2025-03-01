using Newtonsoft.Json;
using NubanAccountDetails.Services;
using System.Net.Http.Headers;
using static NubanAccountDetails.Services.Paystack;

namespace NubanAccountDetails.Paystack
{
    public class ResolveAccount : IResolveAccount
    {
        protected readonly PaystackResolveAccount _api;
        private readonly string _apiKey;


        public ResolveAccount(PaystackResolveAccount api)
        {
            _api = api;
            _apiKey = "sk_test_6c6fc60af0119e14cad8cad7000eb9916014a998";
        }

        public async Task<object> GetResolveAccountNumberAsync(string accountNumber, Dictionary<string, string> dict)
        {
            var tasks = dict.Select(code => ResolveAccountNumberAsync(accountNumber, code));
            while (tasks.Any())
            {
                Task<ResolveAccountNumberResponse> completedTask = await Task.WhenAny(tasks);
                if (completedTask.Result != null)
                {
                    return completedTask.Result;
                }
                tasks = tasks.Except(new[] { completedTask });
            }
            ResolveAccountNumberResponse result = new ResolveAccountNumberResponse
            {
                Status = false,
                Message = "Account not found"
            };
            return result;
        }

        public async Task<ResolveAccountNumberResponse> ResolveAccountNumberAsync(string accountNumber, KeyValuePair<string, string> code)
        {
            ResolveAccountNumberResponse res = await _api.GetAsync<ResolveAccountNumberResponse, object>("bank/resolve", new
            {
                account_number = "3187664210",
                bank_code = "11"
            }, code.Value);
            return res;
        }

        public async Task<object> PaystackResolveAccountNumberAsync(string accountNumber)
        {
           // var getbanks = await GetBanks();
            object? result = await GetResolveAccountNumberAsync(accountNumber, BanksAndCodes.banksAndCodes);
            return result;
        }

        

        public async Task<HttpResponseMessage> GetRequest(string apiUrl, string apiKey)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var cts = new CancellationTokenSource();
                var recipientResponse = await httpClient.GetAsync(apiUrl, cts.Token);
                return recipientResponse;
            }
        }
    }
}
