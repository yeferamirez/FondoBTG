using Moq;
using PensionFund.Infrastructure.Interfaces.Repositories;

namespace PensionFund.Test.Mock
{
    public class MockCacheRepository : Mock<ICacheRepository>
    {
        public MockCacheRepository GetTransaction()
        {
            Setup(x => x.GetTransactionByDate(It.IsAny<string>()));
            return this;
        }

        public MockCacheRepository GetTransactionException()
        {
            Setup(x => x.GetTransactionByDate(It.IsAny<string>())).ThrowsAsync(new Exception("Mock Exception"));
            return this;
        }
    }
}
