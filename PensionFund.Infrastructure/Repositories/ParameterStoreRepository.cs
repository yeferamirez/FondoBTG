using Amazon.SimpleSystemsManagement.Model;
using Newtonsoft.Json;
using PensionFund.Infrastructure.Interfaces.Clients;
using PensionFund.Infrastructure.Interfaces.Repositories;
using Serilog;

namespace PensionFund.Infrastructure.Repositories
{
    public class ParameterStoreRepository : IParameterStoreRepository
    {
        private readonly ISystemManagerClient _systemManagerClient;
        private readonly string _systemManagerPath;

        public ParameterStoreRepository(ISystemManagerClient clientSystemsManagement, string systemManagerPath)
        {
            _systemManagerClient = clientSystemsManagement;
            _systemManagerPath = systemManagerPath;
        }

        public async Task<string> GetParameterStore(string parameterName)
        {
            try
            {
                var request = new GetParameterRequest()
                {
                    Name = $"{_systemManagerPath}/{parameterName}",
                    WithDecryption = true
                };
                var response = await _systemManagerClient.GetSystemManagerClient().GetParameterAsync(request);
                return response.Parameter.Value;
            }
            catch (Exception e)
            {
                Log.Error(JsonConvert.SerializeObject(e));
                Log.Error("There was an error while trying connect to the system manager");
                throw;
            }
        }

        public async Task<Dictionary<string, string>> GetParameters(string appId, List<string> parametersName)
        {
            try
            {
                var request = new GetParametersRequest();
                foreach (var item in parametersName)
                    request.Names.Add($"{appId}/{item}");
                var response = await _systemManagerClient.GetSystemManagerClient().GetParametersAsync(request);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                foreach (var item in response.Parameters)
                    parameters.Add(item.Name.Split("/")[2], item.Value);
                return parameters;
            }
            catch (Exception)
            {
                Log.Error("There was an error while trying connect to the system manager");
                throw;
            }
        }
    }
}
