using Amazon.SimpleSystemsManagement;
using PensionFund.Infrastructure.Interfaces.Clients;

namespace PensionFund.Infrastructure.Clients
{
    public class SystemManagerClient : ISystemManagerClient
    {
        private readonly IAmazonSimpleSystemsManagement _systemsManagementClient;
        public SystemManagerClient(IAmazonSimpleSystemsManagement systemsManagementClient)
        {
            _systemsManagementClient = systemsManagementClient;
        }

        public IAmazonSimpleSystemsManagement GetSystemManagerClient()
        {
            return _systemsManagementClient;
        }
    }
}
