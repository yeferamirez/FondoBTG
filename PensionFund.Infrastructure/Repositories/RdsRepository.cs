using Npgsql;
using PensionFund.Domain.Entities.Responses;
using PensionFund.Infrastructure.Interfaces.Clients;
using PensionFund.Infrastructure.Interfaces.Repositories;
using Serilog;
using System.Text;

namespace PensionFund.Infrastructure.Repositories
{
    public class RdsRepository : IRdsRepository
    {
        private readonly IRdsClient _rdsClient;
        public RdsRepository(IRdsClient rdsClient)
        {
            _rdsClient = rdsClient;
        }

        public async Task<List<string>> GetClients(string city)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append($" SELECT DISTINCT c.nombre, c.apellidos FROM \"Cliente\" c ");
                query.Append($" JOIN \"Inscripcion\" i ON c.\"id\" = i.\"idCliente\" ");
                query.Append($" JOIN \"Producto\" p ON i.\"idProducto\" = p.\"id\" ");
                query.Append($" JOIN \"Disponibilidad\" d ON p.\"id\" = d.\"idProducto\" ");
                query.Append($" JOIN \"Sucursal\" s ON s.\"id\" = d.\"idSucursal\" ");
                query.Append($" JOIN \"Visitan\" v ON v.\"idCliente\"= c.\"id\" AND d.\"idSucursal\" = v.\"idSucursal\" ");
                query.Append($" WHERE d.\"idSucursal\" = v.\"idSucursal\" and s.\"ciudad\" = '{city}'; ");
                string clientName = string.Empty;
                var clientNames = new List<string>();
                var connection = await _rdsClient.GetRDSConnection();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand(query.ToString(), connection))
                    {
                        command.Parameters.AddWithValue("@city", city);
                        using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                clientName = $"{dataReader[0]} {dataReader[1]}";
                                clientNames.Add(clientName);
                            }
                            await connection.DisposeAsync();
                        }
                    }
                }
                return clientNames;
            }
            catch (Exception)
            {
                Log.Error($"There was an error while trying to get clients from database");
                throw;
            }
        }

        public async Task<List<FundConfigurationResponse>> GetFundconfiguration()
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append($" SELECT \"nombre\", \"tipoProducto\" FROM \"Producto\" ");
                var fundConfigurations = new List<FundConfigurationResponse>();
                var connection = await _rdsClient.GetRDSConnection();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand(query.ToString(), connection))
                    {
                        ;
                        using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var fundConfiguration = new FundConfigurationResponse();
                                fundConfiguration.FundName = dataReader[0].ToString();
                                fundConfiguration.Category = dataReader[1].ToString();
                                fundConfigurations.Add(fundConfiguration);
                            }
                            await connection.DisposeAsync();
                        }
                    }
                }
                return fundConfigurations;
            }
            catch (Exception)
            {
                Log.Error($"There was an error while trying to get clients from database");
                throw;
            }
        }
    }
}
