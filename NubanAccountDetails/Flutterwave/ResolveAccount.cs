﻿using NubanAccountDetails.Paystack;
using NubanAccountDetails.Services;

namespace NubanAccountDetails.Flutterwave
{
    public class ResolveAccount : IResolveAccount
    {
        private readonly HttpClient _client;
        protected readonly FlutterwaveResoleAccount _api;

        public ResolveAccount(FlutterwaveResoleAccount api)
        {
            _api = api;
        }

        public async Task<object> GetResolveAccountNumberAsync(string accountNumber, Dictionary<string, string> dict)
        {
            var tasks = dict.Select(code => ResolveAccountNumberAsync(accountNumber, code));

            while (tasks.Any())
            {
                Task<ResolveAccountResponse> completedTask = await Task.WhenAny(tasks);
                if (completedTask.Result != null)
                {
                    return completedTask.Result;
                }
                tasks = tasks.Except(new[] { completedTask });
            }
            ResolveAccountNumberResponse result = new()
            {
                Status = false,
                Message = "Account not found"
            };
            return result;
        }

        public async Task<ResolveAccountResponse> ResolveAccountNumberAsync(string accountNumber, KeyValuePair<string, string> code)
        {
            ResolveAccountResponse response = await _api.PostAsync<ResolveAccountResponse, object>("accounts/resolve", new
            {
                account_number = accountNumber,
                account_bank = code.Key
            }, code);
            return response;
        }

        public async Task<object> ResolveAccountNumber(string accountNumber)
        {
            var result = await GetResolveAccountNumberAsync(accountNumber, BanksAndCodes.banksAndCodes);
            return result;
        }
    }
}
