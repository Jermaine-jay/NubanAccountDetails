using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NubanAccountDetails.Paystack;

namespace NubanAccountDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaystackController : ControllerBase
    {
        public PaystackResolveAccount _paystackResolveAccount;
        private readonly string _apiKey;
        public PaystackController()
        {
            _apiKey = "sk_test_6c6fc60af0119e14cad8cad7000eb9916014a998";
            _paystackResolveAccount = new PaystackResolveAccount(_apiKey);
        }

        [HttpGet("resolve-paystack-account")]
        public async Task<IActionResult> GetPaystack(string request)
        {
            var result = await _paystackResolveAccount.ResolveAccount.PaystackResolveAccountNumberAsync(request);
            return Ok(result);
        }
    }
}
