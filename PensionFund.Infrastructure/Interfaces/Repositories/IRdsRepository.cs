using PensionFund.Domain.Entities.Responses;

namespace PensionFund.Infrastructure.Interfaces.Repositories
{
    public interface IRdsRepository
    {
        public Task<List<string>> GetClients(string city);
        public Task<List<FundConfigurationResponse>> GetFundconfiguration();
    }
}
