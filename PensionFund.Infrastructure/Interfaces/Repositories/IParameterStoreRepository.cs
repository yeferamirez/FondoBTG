namespace PensionFund.Infrastructure.Interfaces.Repositories
{
    public interface IParameterStoreRepository
    {
        public Task<string> GetParameterStore(string parameterName);
        public Task<Dictionary<string, string>> GetParameters(string appId, List<string> parametersName);
    }
}
