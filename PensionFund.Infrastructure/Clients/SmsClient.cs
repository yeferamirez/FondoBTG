using Amazon.SimpleNotificationService;
using PensionFund.Infrastructure.Interfaces.Clients;

namespace PensionFund.Infrastructure.Clients
{
    public class SmsClient : ISmsClient
    {
        private readonly AmazonSimpleNotificationServiceClient _serviceClient;

        public SmsClient(AmazonSimpleNotificationServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<AmazonSimpleNotificationServiceClient> GetConnection()
        {
            return _serviceClient;
        }
    }
}
