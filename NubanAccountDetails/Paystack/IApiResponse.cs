namespace NubanAccountDetails.Paystack
{
    public interface IApiResponse
    {
        bool Status { get; set; }

        string Message { get; set; }
    }
}
