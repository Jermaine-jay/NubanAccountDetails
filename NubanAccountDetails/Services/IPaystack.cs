namespace NubanAccountDetails.Services
{
    public interface IPaystack
    {
        Task<Paystack.ListBankResponse> GetBanks();
        Task<object> PastackResolveAccountNumber(string accountNumber);
        //Task<object> Getallbanks();
    }
}
