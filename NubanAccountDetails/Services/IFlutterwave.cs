namespace NubanAccountDetails.Services
{
    public interface IFlutterwave
    {
        Task<object> FlutterwaveResolveAccountNumber(string accountNumber);
    }
}
