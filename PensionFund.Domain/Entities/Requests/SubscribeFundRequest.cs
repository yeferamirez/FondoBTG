namespace PensionFund.Domain.Entities.Requests
{
    public class SubscribeFundRequest
    {
        public string ClientName { get; set; }
        public string ClientLastName { get; set; }
        public string ProductName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NotificationType { get; set; }
        public string ProductType { get; set; }
        public string ProductCity { get; set; }
        public int Value { get; set; }
    }
}
