using Amazon.DynamoDBv2;

namespace PensionFund.Infrastructure.Interfaces.Clients
{
    public interface IDynamoClient
    {
        public Task<IAmazonDynamoDB> GetConnection();
    }
}
