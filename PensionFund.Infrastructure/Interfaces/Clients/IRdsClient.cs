using Npgsql;

namespace PensionFund.Infrastructure.Interfaces.Clients
{
    public interface IRdsClient
    {
        public Task<NpgsqlConnection> GetRDSConnection();
    }
}
