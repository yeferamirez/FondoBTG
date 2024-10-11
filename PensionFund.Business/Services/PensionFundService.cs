using PensionFund.Domain.Constants;
using PensionFund.Domain.Entities.Requests;
using PensionFund.Domain.Entities.Responses;
using PensionFund.Infrastructure.Interfaces.Repositories;
using PensionFund.Infrastructure.Utils;
using Serilog;

namespace PensionFund.Business.Services
{
    public class PensionFundService
    {
        private readonly IDynamoRepository _cacheRepository;
        private readonly IRdsRepository _rdsRepository;
        private readonly NotificationService _notificationService;
        public PensionFundService(IDynamoRepository cacheRepository,
            IRdsRepository rdsRepository, NotificationService notificationService)
        {
            _cacheRepository = cacheRepository;
            _rdsRepository = rdsRepository;
            _notificationService = notificationService;
        }

        public virtual async Task<TransactionResponse> GetTransactions(string date)
        {
            try
            {
                var transactions = await _cacheRepository.GetTransactionByDate(date);
                return new TransactionResponse(transactions, ResponseConstants.SUCCESS_PROCESS);
            }
            catch (Exception)
            {
                Log.Error("There was an error when trying to transactions");
                throw;
            }
        }

        public virtual async Task<TransactionResponse> SubcribeFund(SubscribeFundRequest transactionRequest)
        {
            try
            {
                var transaction = TransactionUtil.BuildObjectTransaction(transactionRequest);
                var subscribed = await _cacheRepository.GetTransactionByClientName(transaction.ClientName);

                if (TransactionUtil.ValidateExist(transaction, subscribed))
                    return new TransactionResponse(ResponseConstants.FAILED_PROCESS, ExceptionConstants.NOT_VALID_CLIENT);

                var fundConfiguration = await _cacheRepository.GetFundConfigurationByName(transaction.ProductName);
                var client = await _cacheRepository.GetClient(transaction.ClientName);

                if (fundConfiguration != null && client == null)
                {
                    client = TransactionUtil.BuildClient(transaction);
                    await _cacheRepository.SaveClient(client);
                }
                if (fundConfiguration != null && fundConfiguration.MinimumCost <= transaction.Value)
                {
                    client.InitialValue = client.InitialValue - transaction.Value;

                    if (client.InitialValue <= 0)
                        return new TransactionResponse(ResponseConstants.FAILED_PROCESS, ExceptionConstants.NOT_VALID_AMOUNT);

                    await _cacheRepository.SaveClient(client);
                    await _cacheRepository.SaveTransaction(transaction);
                    //await _notificationService.SendNotification(transactionRequest);

                    return new TransactionResponse(transaction, ResponseConstants.SUCCESS_PROCESS);
                }
                return new TransactionResponse(ResponseConstants.FAILED_PROCESS, $"{ExceptionConstants.NOT_VALID_VALUE} <{transaction.ProductName}>");
            }
            catch (Exception)
            {
                Log.Error("There was an error when trying to subcribe");
                throw;
            }
        }

        public virtual async Task<TransactionResponse> UnsubscribeFund(UnsubscribeFundRequest unsubscribeFundRequest)
        {
            try
            {
                var transaction = TransactionUtil.BuildObjectUnsubscribe(unsubscribeFundRequest);
                var Unsubscribe = await _cacheRepository.GetTransactionByClientName(transaction.ClientName);
                if (TransactionUtil.ValidateExist(transaction, Unsubscribe))
                {
                    transaction.Id = Unsubscribe.Id;
                    await _cacheRepository.SaveTransaction(transaction);
                    var client = await _cacheRepository.GetClient(transaction.ClientName);
                    client.InitialValue = client.InitialValue + Unsubscribe.Value;
                    await _cacheRepository.SaveClient(client);
                    return new TransactionResponse(transaction, ResponseConstants.SUCCESS_PROCESS);
                }
                return new TransactionResponse(ResponseConstants.FAILED_PROCESS, ExceptionConstants.NOT_EXIST_CLIENT);
            }
            catch (Exception)
            {
                Log.Error("There was an error when trying to Unsubscribe");
                throw;
            }
        }

        public virtual async Task<List<string>> GetClients(string city)
        {
            try
            {
                string cityToUpper = string.Empty;
                if (!string.IsNullOrEmpty(city))
                    cityToUpper = char.ToUpper(city[0]) + city.Substring(1);
                return await _rdsRepository.GetClients(cityToUpper);
            }
            catch (Exception)
            {
                Log.Error("There was an error when trying clients by city");
                throw;
            }
        }

        public virtual async Task<TransactionResponse> GetFundconfiguration()
        {
            try
            {
                var fundConfiguration = await _cacheRepository.GetFundConfigurations();
                if (fundConfiguration.Count == 0)
                    return new TransactionResponse(ResponseConstants.FAILED_PROCESS, ExceptionConstants.NOT_EXIST_PRODUCTS);
                return new TransactionResponse(fundConfiguration, ResponseConstants.SUCCESS_PROCESS);
            }
            catch (Exception)
            {
                Log.Error("There was an error when trying clients by city");
                throw;
            }
        }
    }
}
