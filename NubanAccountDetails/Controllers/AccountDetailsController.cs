using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NubanAccountDetails.Services;

namespace NubanAccountDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountDetailsController : ControllerBase
    {
        private readonly IGetDetails _getDetails;
        private readonly IPaystack _paystack;
        private readonly IFlutterwave _flutterwave;
        private readonly IMemoryCache _cache;
        public AccountDetailsController(IGetDetails getDetails, IPaystack paystack, IFlutterwave flutterwave) 
        {
            _getDetails = getDetails;
            _paystack = paystack;
            _flutterwave = flutterwave;
        }

        [HttpGet("paystack")]
        public async Task<IActionResult> GetPaystack(string request)
        {
            var result = await _paystack.PastackResolveAccountNumber(request);
            return Ok(result);
        }

        [HttpGet("paystack-banks")]
        public async Task<IActionResult> GetPaystackbanks()
        {

            var result = await _paystack.GetBanks();
            return Ok(result);
        }


        [HttpGet("flutterwave")]
        public async Task<IActionResult> GetFlutterwave(string request)
        {

            var result = await _flutterwave.FlutterwaveResolveAccountNumber(request);
            return Ok(result);
        }
    }
}
