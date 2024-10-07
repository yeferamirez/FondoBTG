using PensionFund.Domain.Constants;
using PensionFund.Domain.Entities.Requests;
using PensionFund.Infrastructure.Interfaces.Repositories;
using Serilog;

namespace PensionFund.Business.Services
{
    public class NotificationService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly ISmsRepository _smsRepository;

        public NotificationService(IEmailRepository emailRepository, ISmsRepository smsRepository)
        {
            _emailRepository = emailRepository;
            _smsRepository = smsRepository;
        }
        public NotificationService() { }

        public async Task SendNotification(SubscribeFundRequest subscribeFund)
        {
            try
            {
                if (NotificationTypeConstants.SMS.Equals(subscribeFund.NotificationType))
                    await _emailRepository.SendEmailNotification(subscribeFund.Email);
                else
                    await _smsRepository.SendSmsNotification(subscribeFund.PhoneNumber);
            }
            catch (Exception)
            {
                Log.Error("There was an error when trying to send notification");
                throw;
            }
        }
    }
}
