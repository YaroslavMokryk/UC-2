using Moq;
using Stripe;
using UC_2.Controllers;
using UC_2.Services;
using Xunit;

namespace UC_2.Tests
{
    public class StripeControllerTests
    {
        private readonly Mock<IStripeApiService> _stripeApiService;

        public StripeControllerTests()
        {
            _stripeApiService = new Mock<IStripeApiService>();
        }

        [Fact]
        public async Task GetStripeBalance_Success()
        {
            //Arrange
            var ct = new CancellationToken();
            var balance = new Balance();
            _stripeApiService.Setup(x => x.GetStripeBalance(ct))
                .ReturnsAsync(balance);
            var controller = new StripeController(_stripeApiService.Object);

            //Act
            var result = await controller.GetStripeBalance(ct);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(balance, result.Value);
        }

        [Fact]
        public async Task GetStripeBalanceTransactions_NoPagination_Success()
        {
            //Arrange
            var ct = new CancellationToken();
            var balanceTransactions = new StripeList<BalanceTransaction>()
            {
                Data = new List<BalanceTransaction>
                {
                    new BalanceTransaction { Id = "test" },
                    new BalanceTransaction { Id = "test2" },
                }
            };
            _stripeApiService.Setup(x => x.GetStripeBalanceTransactions(null, string.Empty, ct))
                .ReturnsAsync(balanceTransactions);
            var controller = new StripeController(_stripeApiService.Object);

            //Act
            var result = await controller.GetStripeBalanceTransactions(null, string.Empty, ct);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(balanceTransactions, result.Value);
        }

        [Fact]
        public async Task GetStripeBalanceTransactions_Pagination_Success()
        {
            //Arrange
            var ct = new CancellationToken();
            var balanceTransactions = new StripeList<BalanceTransaction>()
            {
                Data = new List<BalanceTransaction>
                {
                    new BalanceTransaction { Id = "test2" },
                }
            };
            _stripeApiService.Setup(x => x.GetStripeBalanceTransactions(100, "test", ct))
                .ReturnsAsync(balanceTransactions);
            var controller = new StripeController(_stripeApiService.Object);

            //Act
            var result = await controller.GetStripeBalanceTransactions(100, "test", ct);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(balanceTransactions, result.Value);
            Assert.Equal(balanceTransactions.Data, result.Value.Data);
        }
    }
}
