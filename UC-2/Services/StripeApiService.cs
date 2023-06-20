using Stripe;

namespace UC_2.Services
{
    public class StripeApiService : IStripeApiService
    {
        private readonly BalanceService _stripeBalanceService;
        private readonly BalanceTransactionService _stripeBalanceTransactionService;

        public StripeApiService(
            BalanceService stripeBalanceService,
            BalanceTransactionService stripeBalanceTransactionService
        ) {
            _stripeBalanceService = stripeBalanceService;
            _stripeBalanceTransactionService = stripeBalanceTransactionService;
        }

        /// <summary>
        /// Get Balances from Stripe API
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Stripe Balance information</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public async Task<Balance> GetStripeBalance(CancellationToken ct)
        {
            try
            {
                Balance result = await _stripeBalanceService.GetAsync(cancellationToken: ct);
                return result;
            }
            catch (StripeException ex)
            {
                throw new BadHttpRequestException(ex.StripeError.Message);
            }
        }

        /// <summary>
        /// Get Balance Transactions from Stripe API
        /// </summary>
        /// <param name="limit">Maximum number of transaction to retrieve</param>
        /// <param name="offset">Last transaction id to resume pagination</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Stripe Balance information</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public async Task<StripeList<BalanceTransaction>> GetStripeBalanceTransactions(long? limit, string offset, CancellationToken ct)
        {
            var options = new BalanceTransactionListOptions
            {
                Limit = limit,
                StartingAfter = offset
            };
            try
            {
                StripeList<BalanceTransaction> result = await _stripeBalanceTransactionService.ListAsync(options: options, cancellationToken: ct);
                return result;
            }
            catch (StripeException ex)
            {
                throw new BadHttpRequestException(ex.StripeError.Message);
            }
        }
    }
}
