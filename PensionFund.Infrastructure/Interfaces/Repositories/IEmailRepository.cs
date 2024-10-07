namespace PensionFund.Infrastructure.Interfaces.Repositories
{
    public interface IEmailRepository
    {
        public Task SendEmailNotification(string email);
    }
}
