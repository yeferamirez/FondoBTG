using Newtonsoft.Json;
using PensionFund.Domain.Entities;
using PensionFund.Test.Util;

namespace PensionFund.Test.Mock.Entities
{
    public class LoaderTransaction
    {
        public Transaction GetTransaction()
        {
            return JsonConvert.DeserializeObject<Transaction>(JsonLoaderUtil.LoadJsonFile("./JsonFiles/Transaction/Transaction.json"));
        }

        public async Task<Transaction> GetTransactionAsync()
        {
            return JsonConvert.DeserializeObject<Transaction>(JsonLoaderUtil.LoadJsonFile("./JsonFiles/Transaction/Transaction.json"));
        }

        public async Task<List<Transaction>> GetListTransactionsAsync()
        {
            return new List<Transaction>
            {
                await GetTransactionAsync()
            };
        }

        public List<Transaction> GetListTransactions()
        {
            return new List<Transaction>
            {
                GetTransaction()
            };
        }
    }
}
