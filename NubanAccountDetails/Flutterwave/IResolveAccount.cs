namespace NubanAccountDetails.Flutterwave
{
    public interface IResolveAccount
    {
        Task<object> ResolveAccountNumber(string accountNumber);
    }
}
