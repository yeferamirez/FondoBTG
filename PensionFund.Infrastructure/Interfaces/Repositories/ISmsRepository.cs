
namespace PensionFund.Infrastructure.Interfaces.Repositories
{
    public interface ISmsRepository
    {
        public Task SendSmsNotification(string phoneNumber);
    }
}
