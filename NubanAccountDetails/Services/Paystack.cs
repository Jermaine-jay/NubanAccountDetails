using Newtonsoft.Json;
using NubanAccountDetails.Flutterwave;
using System.Net.Http.Headers;

namespace NubanAccountDetails.Services
{
    public class Paystack : IPaystack
    {
        private readonly IGetDetails _getDetails;
        private readonly string _apiKey;
        public Paystack(IGetDetails getDetails)
        {
            _getDetails = getDetails;
            _apiKey = "sk_test_6c6fc60af0119e14cad8cad7000eb9916014a998";
        }

        public async Task<object> PastackResolveAccountNumber(string accountNumber)
        {
            string? apiUrl = $"https://api.paystack.co/bank/resolve?account_number={accountNumber}&bank_code=";
            object? result = await _getDetails.ResolveAccountNumber(_apiKey, apiUrl);
            return result;
        }

        /*public async Task<ListBankResponse> GetBanks()
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
                    var bankResponse = await _resolveAccount.ResolveAccountNumber(_apiKey, apiUrl + pair.code, new KeyValuePair<string, string>(pair.code, pair.name));
                    if (bankResponse != null && bankResponse.Bank_name != null)
                    {
                        listofBanksCode.Add(pair.code, pair.name);
                    }
                });

                await Task.WhenAll(tasks);
                return getResponse;
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
        }*/

        /*ublic async Task<object> ResolveAccountNumber(string apiKey, string apiUrl, Dictionary<string, string> dict)
        {
            var tasks = dict.Select(code => ResolveAccountNumberAsync(apiKey, apiUrl + code.Key, code));
            var completedTasks = await Task.WhenAll(tasks);

            foreach (var completedTask in completedTasks)
            {
                if (completedTask != null && completedTask.Bank_name != null)
                {
                    return completedTasks;
                }
            }

            return "Account does not exist!";
        }

        private async Task<Data> ResolveAccountNumberAsync(string apiKey, string apiUrl, KeyValuePair<string, string> code)
        {
            var recipientResponse = await GetRequest(apiUrl, apiKey);

            if (recipientResponse.IsSuccessStatusCode)
            {
                var listResponse = await recipientResponse.Content.ReadAsStringAsync();
                var getResponse = JsonConvert.DeserializeObject<Response>(listResponse);

                if (getResponse.Status != "false")
                {
                    var data = getResponse.Data;
                    data.Bank_name = code.Value.ToUpper();
                    return data;
                }
            }

            return null;
        }
*/


        public class ListBankResponse
        {
            public string status { get; set; }
            public string message { get; set; }
            public IEnumerable<Data> data { get; set; }
            public class Data
            {
                public string id { get; set; }
                public string name { get; set; }
                public string slug { get; set; }
                public string code { get; set; }
                public string longcode { get; set; }
                public string pay_with_bank { get; set; }
                public string active { get; set; }
                public string country { get; set; }
                public string currency { get; set; }
                public string type { get; set; }
                public string is_deleted { get; set; }
                public string createdAt { get; set; }
                public string updatedAt { get; set; }

            }
        }

    }
    /* public static class ExtendingMethods
     {
         public static async Task ExtendDictionaryAsync(this Dictionary<string, string> dict)
         {
             var result = dict.Select(x => ResolveDictAsync(x));
             await Task.WhenAll(result);
         }
         public static async Task ResolveDictAsync(KeyValuePair<string, string> dict)
         {
             Console.Write($"[{dict.Key}, {dict.Value}]");
         }

         public static IEnumerable<KeyValuePair<string, string>> ExtendDictionary(this Dictionary<string, string> dict)
         {
             var result = dict.Select(x => ResolveDict(x));
             return result;
         }


         public static KeyValuePair<string, string> ResolveDict(KeyValuePair<string, string> dict)
         {
             Console.Write($"[{dict.Key}, {dict.Value}]");
             return dict;
         }
     }*/
}

