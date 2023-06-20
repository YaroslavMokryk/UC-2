using Microsoft.AspNetCore.Mvc;
using Stripe;
using UC_2.Services;

namespace UC_2.Controllers
{
    [Route("api/[controller]")]
    public class StripeController
    {
        private readonly IStripeApiService _stripeService;

        public StripeController(IStripeApiService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpGet("balance")]
        public async Task<ActionResult<Balance>> GetStripeBalance(CancellationToken ct)
        {
            Balance result = await _stripeService.GetStripeBalance(ct);

            return result;
        }
    }
}
