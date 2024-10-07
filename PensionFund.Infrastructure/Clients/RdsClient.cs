using Npgsql;
using PensionFund.Infrastructure.Interfaces.Clients;

namespace PensionFund.Infrastructure.Clients
{
    public class RdsClient : IRdsClient
    {
        private readonly string _stringConnection;
        public RdsClient(string stringConnection)
        {
            _stringConnection = stringConnection;
        }

        public async Task<NpgsqlConnection> GetRDSConnection()
        {
            var _connection = new NpgsqlConnection(_stringConnection);
            await _connection.OpenAsync();
            return _connection;
        }
    }
}
