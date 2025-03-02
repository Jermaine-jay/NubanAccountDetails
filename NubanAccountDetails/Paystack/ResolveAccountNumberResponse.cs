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

    public class BankResponse : HasRawResponse, IApiResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
        public IEnumerable<Data> data { get; set; }
        public class Data
        {
            public string id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
            public string code { get; set; }
            public string longcode { get; set; }
            public string pay_with_bank { get; set; }
            public string active { get; set; }
            public string country { get; set; }
            public string currency { get; set; }
            public string type { get; set; }
            public string is_deleted { get; set; }
            public string createdAt { get; set; }
            public string updatedAt { get; set; }

        }
    }
}
