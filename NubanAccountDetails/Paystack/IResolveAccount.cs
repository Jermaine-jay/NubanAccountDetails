

namespace NubanAccountDetails.Paystack
{
    public interface IResolveAccount
    {
        Task<Dictionary<string, string>> GetBanks();
        Task<ResolveAccountNumberResponse> PaystackResolveAccountNumberAsync(string accountNumber);
    }
}
