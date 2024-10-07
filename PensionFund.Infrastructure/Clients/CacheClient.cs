using Amazon.DynamoDBv2;
using PensionFund.Infrastructure.Interfaces.Clients;

namespace PensionFund.Infrastructure.Clients
{
    public class CacheClient : ICacheClient
    {
        private readonly IAmazonDynamoDB _clientDynamo;

        public CacheClient(IAmazonDynamoDB clientDynamo)
        {
            _clientDynamo = clientDynamo;
        }

        public async Task<IAmazonDynamoDB> GetConnection()
        {
            return _clientDynamo;
        }
    }
}
