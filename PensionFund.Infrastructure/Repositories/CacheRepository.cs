using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using PensionFund.Domain.Entities;
using PensionFund.Domain.Entities.Responses;
using PensionFund.Infrastructure.Interfaces.Clients;
using PensionFund.Infrastructure.Interfaces.Repositories;
using PensionFund.Infrastructure.Utils;
using Serilog;

namespace PensionFund.Infrastructure.Repositories
{
    public class CacheRepository : IDynamoRepository
    {
        private readonly IDynamoClient _cacheClient;
        private readonly string _configurationsTableName;
        private readonly string _transactionsTableName;
        private readonly string _clientTableName;
        public CacheRepository(IDynamoClient cacheClient, string configurationsTableName,
            string transactionsTableName, string clientTableName)
        {
            _cacheClient = cacheClient;
            _configurationsTableName = configurationsTableName;
            _transactionsTableName = transactionsTableName;
            _clientTableName = clientTableName;
        }

        public async Task SaveTransaction(Transaction transaction)
        {
            try
            {
                var connection = await _cacheClient.GetConnection();
                var putItemRequest = new PutItemRequest
                {
                    TableName = _transactionsTableName,
                    Item = DynamoUtil.CreateItemRequest(transaction)
                };
                await connection.PutItemAsync(putItemRequest);
            }
            catch (Exception)
            {
                Log.Error("An error occurred creating the event in the database");
                throw;
            }
        }

        public async Task<Transaction> GetTransactionByClientName(string name)
        {
            try
            {

                var connection = await _cacheClient.GetConnection();
                Transaction transaction = new Transaction();
                QueryRequest queryRequest = new QueryRequest
                {
                    TableName = _transactionsTableName,
                    IndexName = "ClientName-index",
                    KeyConditionExpression = "ClientName  = :v_name",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":v_name", new AttributeValue { S =  name }}
                    },
                    ScanIndexForward = true
                };

                var result = await connection.QueryAsync(queryRequest);
                if (result.Items.Count > 0)
                {
                    var transactionDocument = Document.FromAttributeMap(result.Items[0]);
                    string transactionRequestJson = transactionDocument.ToJson();
                    transaction = JsonConvert.DeserializeObject<Transaction>(transactionRequestJson);

                    return transaction;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                Log.Error("There was an error while getting the fund configuration from the cache");
                throw;
            }
        }

        public async Task<FundConfiguration> GetFundConfigurationByName(string name)
        {
            try
            {
                var connection = await _cacheClient.GetConnection();
                QueryRequest queryRequest = new QueryRequest
                {
                    TableName = _configurationsTableName,
                    KeyConditionExpression = "FundName  = :v_name",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":v_name", new AttributeValue { S =  name }}
                    },
                    ScanIndexForward = true
                };

                var result = await connection.QueryAsync(queryRequest);
                if (result.Items.Count > 0)
                {
                    var fundConfigurationRequestDocument = Document.FromAttributeMap(result.Items[0]);
                    string fundConfigurationRequestJson = fundConfigurationRequestDocument.ToJson();
                    var fundConfigurationResponse = JsonConvert.DeserializeObject<FundConfiguration>(fundConfigurationRequestJson);

                    return fundConfigurationResponse;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                Log.Error("There was an error while getting the fund configuration from the cache");
                throw;
            }
        }

        public async Task<List<FundConfigurationResponse>> GetFundConfigurations()
        {
            try
            {
                var itemsManagers = new List<FundConfigurationResponse>();
                var connection = await _cacheClient.GetConnection();
                ScanRequest scanRequest = new ScanRequest()
                {
                    TableName = _configurationsTableName
                };

                var result = await connection.ScanAsync(scanRequest);
                if (result.Items.Count > 0)
                {
                    foreach (var item in result.Items)
                    {
                        var fundConfigurationRequestDocument = Document.FromAttributeMap(item);
                        string fundConfigurationRequestJson = fundConfigurationRequestDocument.ToJson();
                        var fundConfigurationsResponse = JsonConvert.DeserializeObject<FundConfigurationResponse>(fundConfigurationRequestJson);
                        itemsManagers.Add(fundConfigurationsResponse);
                    }
                    return itemsManagers;
                }
                else
                {
                    Log.Error("Fund configuration is null or the table");
                    return null;
                }
            }
            catch (Exception)
            {
                Log.Error("There was an error while getting the fund configurations from the dynamo");
                throw;
            }
        }

        public async Task<List<Transaction>> GetTransactionByDate(string date)
        {
            try
            {
                var connection = await _cacheClient.GetConnection();
                Transaction transaction = new Transaction();
                List<Transaction> transactions = new List<Transaction>();
                var scanRequest = new ScanRequest
                {
                    TableName = _transactionsTableName,
                    FilterExpression = $"ModificationDate >= :v_date",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":v_date", new AttributeValue { S = date } }
                    }
                };

                var result = await connection.ScanAsync(scanRequest);
                if (result.Items.Count > 0)
                {
                    foreach (var item in result.Items)
                    {
                        var transactionsDocument = Document.FromAttributeMap(item);
                        string transactionsRequestJson = transactionsDocument.ToJson();
                        transaction = JsonConvert.DeserializeObject<Transaction>(transactionsRequestJson);
                        transactions.Add(transaction);
                    }


                    return transactions;
                }
                else
                    return transactions;
            }
            catch (Exception)
            {
                Log.Error("There was an error while getting transactions");
                throw;
            }
        }

        public async Task<ClientInformation> GetClient(string clientName)
        {
            try
            {
                var connection = await _cacheClient.GetConnection();
                QueryRequest queryRequest = new QueryRequest
                {
                    TableName = _clientTableName,
                    KeyConditionExpression = "ClientName  = :v_name",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":v_name", new AttributeValue { S =  clientName }}
                    },
                    ScanIndexForward = true
                };

                var result = await connection.QueryAsync(queryRequest);
                if (result.Items.Count > 0)
                {
                    var clientRequestDocument = Document.FromAttributeMap(result.Items[0]);
                    string clientRequestJson = clientRequestDocument.ToJson();
                    var clientResponse = JsonConvert.DeserializeObject<ClientInformation>(clientRequestJson);

                    return clientResponse;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                Log.Error("There was an error while getting the client configuration from the cache");
                throw;
            }
        }

        public async Task SaveClient(ClientInformation client)
        {
            try
            {
                var connection = await _cacheClient.GetConnection();
                var putItemRequest = new PutItemRequest
                {
                    TableName = _clientTableName,
                    Item = DynamoUtil.CreateItemRequest(client)
                };
                await connection.PutItemAsync(putItemRequest);
            }
            catch (Exception)
            {
                Log.Error("An error occurred creating the event in the database");

            }
        }
    }
}
