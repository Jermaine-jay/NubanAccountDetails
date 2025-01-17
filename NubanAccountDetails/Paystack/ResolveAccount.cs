using NubanAccountDetails.Services;

namespace NubanAccountDetails.Paystack
{
    public class ResolveAccount : IResolveAccount
    {
        protected readonly PaystackResolveAccount _api;

        public ResolveAccount(PaystackResolveAccount api)
        {
            _api = api;
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
            object? result = await GetResolveAccountNumberAsync(accountNumber, BanksAndCodes.banksAndCodes);
            return result;
        }
    }
}
