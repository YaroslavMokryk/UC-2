using Stripe;

namespace UC_2.Services
{
    public interface IStripeApiService
    {
        Task<Balance> GetStripeBalance(CancellationToken ct);
    }
}
