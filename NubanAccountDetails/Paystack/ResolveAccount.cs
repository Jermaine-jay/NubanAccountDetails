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
                account_number = accountNumber,
                bank_code = code.Key
            }, code.Value);
            return res;
        }

        public async Task<object> PaystackResolveAccountNumberAsync(string accountNumber)
        {
            var getbanks = await GetBanks();
            object? result = await GetResolveAccountNumberAsync(accountNumber, getbanks);
            return result;
        }

        public async Task<Dictionary<string, string>> GetBanks()
        {
            var apiUrl = "https://api.paystack.co/bank?currency=NGN";
            var listofBanksCode = new Dictionary<string, string>();

            var recipientResponse = await GetRequest(apiUrl, _apiKey);

            if (recipientResponse.IsSuccessStatusCode)
            {
                var listResponse = await recipientResponse.Content.ReadAsStringAsync();
                var getResponse = JsonConvert.DeserializeObject<ListBankResponse>(listResponse);

                var tasks = getResponse.data.Select(async pair =>
                {
                    //var bankResponse = await _resolveAccount.ResolveAccountNumber(_apiKey, apiUrl + pair.code, new KeyValuePair<string, string>(pair.code, pair.name));
                    if (pair.code != null && pair.name != null)
                    {
                        listofBanksCode.Add(pair.code, pair.name);
                    }
                });

                await Task.WhenAll(tasks);
                return listofBanksCode;
            }

            return null;
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
