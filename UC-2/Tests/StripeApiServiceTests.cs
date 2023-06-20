using Moq;
using Stripe;
using System.Xml.Linq;
using UC_2.Services;
using Xunit;

namespace UC_2.Tests
{
    public class StripeApiServiceTests
    {
        private readonly Mock<BalanceService> _stripeBalanceService;
        private readonly Mock<BalanceTransactionService> _stripeBalanceTransactionService;

        public StripeApiServiceTests()
        {
            _stripeBalanceService = new Mock<BalanceService>();
            _stripeBalanceTransactionService = new Mock<BalanceTransactionService>();
        }

        [Fact]
        public async Task GetStripeBalance_Success()
        {
            //Arrange
            var ct = new CancellationToken();
            var balance = new Balance();
            _stripeBalanceService.Setup(x => x.GetAsync(null, ct))
                .ReturnsAsync(balance);
            var service = new StripeApiService(_stripeBalanceService.Object, _stripeBalanceTransactionService.Object);

            //Act
            var result = await service.GetStripeBalance(ct);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(balance, result);
        }

        [Fact]
        public async Task GetStripeBalance_StripeError_ThrowsException()
        {
            //Arrange
            var ct = new CancellationToken();
            var ex = new StripeException
            {
                StripeError = new StripeError
                {
                    Message = "test"
                }
            };
            _stripeBalanceService.Setup(x => x.GetAsync(null, ct))
                .ThrowsAsync(ex);
            var service = new StripeApiService(_stripeBalanceService.Object, _stripeBalanceTransactionService.Object);

            //Act
            Exception result = await Assert.ThrowsAsync<BadHttpRequestException>(() => service.GetStripeBalance(ct));

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result);
            Assert.NotNull(result.Message);
            Assert.Equal(ex.StripeError.Message, result.Message);
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
            _stripeBalanceTransactionService.Setup(x => x.ListAsync(It.IsAny<BalanceTransactionListOptions>(), null, ct))
                .ReturnsAsync(balanceTransactions);
            var service = new StripeApiService(_stripeBalanceService.Object, _stripeBalanceTransactionService.Object);

            //Act
            var result = await service.GetStripeBalanceTransactions(null, string.Empty, ct);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(balanceTransactions, result);
            Assert.Equal(balanceTransactions.Data, result.Data);
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
            _stripeBalanceTransactionService.Setup(x => x.ListAsync(It.IsAny<BalanceTransactionListOptions>(), null, ct))
                .ReturnsAsync(balanceTransactions);
            var service = new StripeApiService(_stripeBalanceService.Object, _stripeBalanceTransactionService.Object);

            //Act
            var result = await service.GetStripeBalanceTransactions(100, "test", ct);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(balanceTransactions, result);
            Assert.Equal(balanceTransactions.Data, result.Data);
        }

        [Fact]
        public async Task GetStripeBalanceTransactions_StripeError_ThrowsException()
        {
            //Arrange
            var ct = new CancellationToken();
            var ex = new StripeException
            {
                StripeError = new StripeError
                {
                    Message = "test"
                }
            };
            _stripeBalanceTransactionService.Setup(x => x.ListAsync(It.IsAny<BalanceTransactionListOptions>(), null, ct))
                .ThrowsAsync(ex);
            var service = new StripeApiService(_stripeBalanceService.Object, _stripeBalanceTransactionService.Object);

            //Act
            Exception result = await Assert.ThrowsAsync<BadHttpRequestException>(() => service.GetStripeBalanceTransactions(null, string.Empty, ct));

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Message);
            Assert.Equal(ex.StripeError.Message, result.Message);
        }
    }
}
