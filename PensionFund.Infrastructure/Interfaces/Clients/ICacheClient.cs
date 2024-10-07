using Amazon.DynamoDBv2;

namespace PensionFund.Infrastructure.Interfaces.Clients
{
    public interface ICacheClient
    {
        public Task<IAmazonDynamoDB> GetConnection();
    }
}
