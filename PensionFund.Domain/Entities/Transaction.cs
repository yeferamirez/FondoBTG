namespace PensionFund.Domain.Entities
{
    public class Transaction
    {
        public string Id { get; set; }
        public string ClientName { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string ProductCity { get; set; }
        public int Value { get; set; }
        public string State { get; set; }
        public string ModificationDate { get; set; }
    }
}
