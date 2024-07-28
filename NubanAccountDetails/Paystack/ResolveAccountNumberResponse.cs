using Newtonsoft.Json;

namespace NubanAccountDetails.Paystack
{
    public class ResolveAccountNumberResponse : HasRawResponse, IApiResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

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
