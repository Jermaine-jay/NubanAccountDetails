namespace NubanAccountDetails.Paystack
{
    public class ResolveAccount : IResolveAccount
    {
        protected readonly PaystackResolveAccount _api;
        private readonly string _currency;


        public ResolveAccount(PaystackResolveAccount api, string currency)
        {
            _api = api;
            _currency = currency;
        }

        public async Task<ResolveAccountNumberResponse> GetResolveAccountNumberAsync(string accountNumber, Dictionary<string, string> dict)
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

        public async Task<ResolveAccountNumberResponse> PaystackResolveAccountNumberAsync(string accountNumber)
        {
            Dictionary<string, string> getbanks = await GetBanks();
            ResolveAccountNumberResponse result = await GetResolveAccountNumberAsync(accountNumber, getbanks);
            return result;
        }

        public async Task<Dictionary<string, string>> GetBanks()
        {
            Dictionary<string, string> listofBanksCode = new Dictionary<string, string>();

            BankResponse recipientResponse = await _api.GetAsync<BankResponse, object>("bank?currency", new
            {
                currency =  _currency ?? "NGN"
            });

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
            return listofBanksCode;
        }
    }
}
