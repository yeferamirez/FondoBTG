using Moq;
using PensionFund.Business.Services;
using PensionFund.Infrastructure.Interfaces.Repositories;
using PensionFund.Test.Mock;

namespace PensionFund.Test.Tests
{
    public class PensionFundServiceTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetTransactions(int test)
        {
            var mockCacheRepository = new MockCacheRepository().GetTransaction().Object;
            var mockNotificationService = new Mock<NotificationService>().Object;
            var mockRdsRepository = new Mock<IRdsRepository>().Object;
            if (test == 1)
            {
                var basicService = new PensionFundService(mockCacheRepository, mockRdsRepository, mockNotificationService);
                await basicService.GetTransactions("2024-10-05");
            }
            else
            {
                var mockCacheRepositoryException = new MockCacheRepository().GetTransactionException().Object;
                var basicService = new PensionFundService(mockCacheRepositoryException, mockRdsRepository, mockNotificationService);
                await Assert.ThrowsAsync<Exception>(() => basicService.GetTransactions("2024-10-05"));
            }
        }
    }
}
