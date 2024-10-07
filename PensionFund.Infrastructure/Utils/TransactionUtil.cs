using PensionFund.Domain.Constants;
using PensionFund.Domain.Entities;
using PensionFund.Domain.Entities.Requests;

namespace PensionFund.Infrastructure.Utils
{
    public static class TransactionUtil
    {
        public static ClientInformation BuildClient(Transaction transaction)
        {
            return new ClientInformation
            {
                ClientName = transaction.ClientName,
                City = transaction.ProductCity,
                InitialValue = 500000
            };
        }

        public static Transaction BuildObjectTransaction(SubscribeFundRequest transactionRequest)
        {
            return new Transaction
            {
                ClientName = $"{transactionRequest.ClientName} {transactionRequest.ClientLastName}",
                ModificationDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Id = Guid.NewGuid().ToString(),
                ProductCity = transactionRequest.ProductCity,
                ProductName = transactionRequest.ProductName,
                ProductType = transactionRequest.ProductType,
                State = TransactionConstants.ACTIVATED,
                Value = transactionRequest.Value
            };
        }

        public static Transaction BuildObjectUnsubscribe(UnsubscribeFundRequest unsubscribeFundRequest)
        {
            return new Transaction
            {
                ClientName = $"{unsubscribeFundRequest.ClientName} {unsubscribeFundRequest.ClientLastName}",
                ModificationDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                ProductCity = unsubscribeFundRequest.City,
                ProductName = unsubscribeFundRequest.ProductName,
                ProductType = unsubscribeFundRequest.ProductType,
                State = TransactionConstants.REMOVED,
                Value = 0
            };
        }

        public static bool ValidateExist(Transaction transaction, Transaction subscribed)
        {
            if (subscribed == null)
                return false;
            if (subscribed.ClientName.Equals(transaction.ClientName) &&
                subscribed.ProductName.Equals(transaction.ProductName) &&
                subscribed.ProductType.Equals(transaction.ProductType) &&
                subscribed.ProductCity.Equals(transaction.ProductCity))
                return true;
            return false;
        }
    }
}
