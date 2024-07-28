namespace NubanAccountDetails.Services
{
    public interface IGetDetails
    {
        Task<object> ResolveAccountNumber(string apiKey, string apiUrl, Dictionary<string, string> dict);
        Task<object> ResolveAccountNumber(string apiKey, string apiUrl, string accountNumber, Dictionary<string, string> dict);
    }
}
