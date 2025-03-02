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
            var tasks = dict.Select(async bank =>
            {
                return await ResolveAccountNumberAsync(accountNumber, bank);
            }).ToList();

            while (tasks.Any())
            {
                Task<ResolveAccountNumberResponse> completedTask = await Task.WhenAny(tasks);
                tasks.Remove(completedTask);

                ResolveAccountNumberResponse response = await completedTask;

                if (response != null && response.Status.Equals(true))
                {
                    return response;
                }
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
                bank_code = code.Key,
            }, code.Value);
            return res;
        }

        public async Task<object> PaystackResolveAccountNumberAsync(string accountNumber)
        {
            var getbanks = await GetBanks();
            var result = await GetResolveAccountNumberAsync(accountNumber, getbanks);
            return result;
        }



        //public async Task<HttpResponseMessage> GetRequest(string apiUrl, string apiKey)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var cts = new CancellationTokenSource();
        //        var recipientResponse = await httpClient.GetAsync(apiUrl, cts.Token);
        //        return recipientResponse;
        //    }
        //}

        public async Task<Dictionary<string, string>> GetBanks()
        {
            var listofBanksCode = new Dictionary<string, string>();

            var recipientResponse = await _api.GetAsync<BankResponse, object>("bank?currency", new
            {
                currency = "NGN"
            }, null);

            if (recipientResponse.Status)
            {
                foreach (var pair in recipientResponse.data)
                {
                    if (!string.IsNullOrEmpty(pair.code) && !string.IsNullOrEmpty(pair.name))
                    {
                        listofBanksCode[pair.code] = pair.name;
                    }
                }
                return listofBanksCode;
            }
            return null;
        }
    }
}
