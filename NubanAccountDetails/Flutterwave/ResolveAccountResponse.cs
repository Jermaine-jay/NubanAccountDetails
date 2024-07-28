using Newtonsoft.Json;
using NubanAccountDetails.Paystack;

namespace NubanAccountDetails.Flutterwave
{

    public class ResolveAccountResponse : HasRawResponse, IResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public ResolveAcountNumber.Data Data { get; set; }
    }

    public class ResolveAcountNumber
    {
        public class Data
        {
            [JsonProperty("bank_name")]
            public string BankName { get; set; }

            [JsonProperty("bank_id")]
            public string BankId { get; set; }

            [JsonProperty("account_number")]
            public string AccountNumber { get; set; }

            [JsonProperty("account_name")]
            public string AccountName { get; set; }

        }
    }
}
