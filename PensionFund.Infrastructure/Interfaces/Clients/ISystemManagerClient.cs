using Amazon.SimpleSystemsManagement;

namespace PensionFund.Infrastructure.Interfaces.Clients
{
    public interface ISystemManagerClient
    {
        public IAmazonSimpleSystemsManagement GetSystemManagerClient();
    }
}
