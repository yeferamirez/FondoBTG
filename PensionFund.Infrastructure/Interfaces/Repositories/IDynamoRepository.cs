using PensionFund.Domain.Entities;
using PensionFund.Domain.Entities.Responses;

namespace PensionFund.Infrastructure.Interfaces.Repositories
{
    public interface IDynamoRepository
    {
        public Task SaveTransaction(Transaction transaction);
        public Task<FundConfiguration> GetFundConfigurationByName(string name);
        public Task<Transaction> GetTransactionByClientName(string name);
        public Task<List<Transaction>> GetTransactionByDate(string date);
        public Task<ClientInformation> GetClient(string clientName);
        public Task SaveClient(ClientInformation client);
        public Task<List<FundConfigurationResponse>> GetFundConfigurations();
    }
}
