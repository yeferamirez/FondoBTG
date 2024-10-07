using Amazon.SimpleEmail;

namespace PensionFund.Infrastructure.Interfaces.Clients
{
    public interface IEmailClient
    {
        public Task<IAmazonSimpleEmailService> GetConnection();
    }
}
