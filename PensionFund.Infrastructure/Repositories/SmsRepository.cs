using Amazon.SimpleNotificationService.Model;
using PensionFund.Domain.Constants;
using PensionFund.Domain.Exceptions;
using PensionFund.Infrastructure.Interfaces.Clients;
using PensionFund.Infrastructure.Interfaces.Repositories;
using Serilog;

namespace PensionFund.Infrastructure.Repositories
{
    public class SmsRepository : ISmsRepository
    {
        private readonly ISmsClient _smsClient;
        public SmsRepository(ISmsClient smsClient)
        {
            _smsClient = smsClient;
        }

        public async Task SendSmsNotification(string phoneNumber)
        {
            try
            {
                var publishRequest = new PublishRequest { PhoneNumber = phoneNumber };
                var connection = await _smsClient.GetConnection();
                var response = await connection.PublishAsync(publishRequest);
                if (response == null)
                    throw new NotificationException(ExceptionConstants.NOT_SEND_SMS);
            }
            catch (NotificationException e)
            {
                Log.Error(e.Message);
            }
            catch (Exception)
            {
                Log.Error("There was an error when trying to send sms");
                throw;
            }

        }
    }
}
