namespace NubanAccountDetails.Response
{
    public class ListBankResponse
    {
        public string status { get; set; }
        public string message { get; set; }
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

