namespace NubanAccountDetails.Services
{
    public interface IGetDetails
    {
        //Task<object> ResolveFlutterAccountNumber(string apiKey, string apiUrl, Dictionary<string, string> dict);
        Task<object> ResolveFlutterAccountNumber(string apiKey, string apiUrl, string accountNumber, Dictionary<string, string> dict);
        Task<object> ResolveAccountNumber(string apiKey, string apiUrl);
        Task<Dictionary<string, string>> GetBanks(string apikey);
    }
}
