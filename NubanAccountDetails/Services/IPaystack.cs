namespace NubanAccountDetails.Services
{
    public interface IPaystack
    {
        Task<object> PastackResolveAccountNumber(string accountNumber);
        //Task<object> Getallbanks();
    }
}
