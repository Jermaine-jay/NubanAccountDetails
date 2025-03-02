namespace NubanAccountDetails.Response
{
    public class ResponseDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public string Bank_name { get; set; }
        public string Bank_Id { get; set; }
        public string Account_name { get; set; }
        public string Account_number { get; set; }
    }
}
