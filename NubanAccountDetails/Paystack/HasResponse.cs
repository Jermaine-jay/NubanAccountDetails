namespace NubanAccountDetails.Paystack
{
    public class HasRawResponse : IHasRawResponse
    {
        public string RawJson { get; set; }
    }

    public interface IHasRawResponse
    {
        string RawJson { get; set; }
    }
}
