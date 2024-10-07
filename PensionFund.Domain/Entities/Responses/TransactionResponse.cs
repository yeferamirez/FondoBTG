namespace PensionFund.Domain.Entities.Responses
{
    public class TransactionResponse
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public Transaction Transaction { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<FundConfigurationResponse> FundConfigurations { get; set; }

        public TransactionResponse(string status, string message)
        {
            Message = message;
            Status = status;
        }

        public TransactionResponse(Transaction transaction, string status)
        {
            Transaction = transaction;
            Status = status;
        }

        public TransactionResponse(List<Transaction> transactions, string status)
        {
            Transactions = transactions;
            Status = status;
        }

        public TransactionResponse(List<FundConfigurationResponse> fundConfigurations, string status)
        {
            FundConfigurations = fundConfigurations;
            Status = status;
        }
    }
}
