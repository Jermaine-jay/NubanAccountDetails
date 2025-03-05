namespace NubanAccountDetails.Services
{
    public class Flutterwave: IFlutterwave
    {
        public readonly IGetDetails _getDetails;
        private readonly string _apiKey;
        public Flutterwave(IGetDetails getDetails)
        {
            _getDetails = getDetails;
            _apiKey = "FLWSECK_TEST-689f4305ab8e742e8c2008a985a5c647-X";

        }

        public async Task<object> FlutterwaveResolveAccountNumber(string accountNumber)
        {
            string? apiUrl = $"https://api.flutterwave.com/v3/accounts/resolve";
            //var apiUrl = $"https://cashierapi.opayweb.com/api/v3/verification/accountNumber/resolve?bankCode=057&bankAccountNo=2088238216&countryCode=NG";
            object? result = await _getDetails.ResolveFlutterAccountNumber(_apiKey, apiUrl,accountNumber, BanksAndCodes.banksAndCodes);
            return result;
        }
    }
}
