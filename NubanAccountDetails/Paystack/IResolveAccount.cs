namespace NubanAccountDetails.Paystack
{
    public interface IResolveAccount
    {
        Task<object> PaystackResolveAccountNumberAsync(string accountNumber);
    }
}
