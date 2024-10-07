using Amazon.SimpleNotificationService;

namespace PensionFund.Infrastructure.Interfaces.Clients
{
    public interface ISmsClient
    {
        public Task<AmazonSimpleNotificationServiceClient> GetConnection();
    }
}
