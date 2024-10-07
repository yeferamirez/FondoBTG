namespace PensionFund.Domain.Entities.Requests
{
    public class UnsubscribeFundRequest
    {
        public string ClientName { get; set; }
        public string ClientLastName { get; set; }
        public string City { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
    }
}
