namespace NubanAccountDetails.Flutterwave
{
    public interface IResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }

        //public Data Data { get; set; }
    }
}
