using Microsoft.AspNetCore.Mvc;
using NubanAccountDetails.Flutterwave;

namespace NubanAccountDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlutterController : Controller
    {
        public FlutterwaveResoleAccount _flutterwaveResolveAccount;
        private readonly string _apiKey;
        public FlutterController()
        {
            _apiKey = "FLWSECK_TEST-689f4305ab8e742e8c2008a985a5c647-X";
            _flutterwaveResolveAccount = new FlutterwaveResoleAccount(_apiKey);
        }

        [HttpGet("resolve-flutter-account")]
        public async Task<IActionResult> GetPaystack(string request)
        {
            var result = await _flutterwaveResolveAccount.ResolveAccount.ResolveAccountNumber(request);
            return Ok(result);
        }
    }
}
