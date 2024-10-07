using Amazon.SimpleEmail;
using PensionFund.Infrastructure.Interfaces.Clients;

namespace PensionFund.Infrastructure.Clients
{
    public class EmailClient: IEmailClient
    {
        private readonly IAmazonSimpleEmailService _amazonSimpleEmail;

        public EmailClient(IAmazonSimpleEmailService amazonSimpleEmail)
        {
            _amazonSimpleEmail = amazonSimpleEmail;
        }

        public async Task<IAmazonSimpleEmailService> GetConnection()
        {
            return _amazonSimpleEmail;
        }
    }
}
