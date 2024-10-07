using Amazon.DynamoDBv2.Model;
using System.ComponentModel;

namespace PensionFund.Infrastructure.Utils
{
    public static class DynamoUtil
    {
        public static Dictionary<string, AttributeValue> CreateItemRequest(object? delivery)
        {
            var deliveryDictionary = new Dictionary<string, AttributeValue>();
            foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(delivery))
            {
                if (item.GetValue(delivery) != null)
                {
                    var type = item.PropertyType.Name;
                    if (type.ToLower().Contains("int") || type.ToLower().Contains("double"))
                    {
                        var value = item.GetValue(delivery).ToString();
                        deliveryDictionary.Add(item.Name, new AttributeValue { N = value });
                    }
                    else if (type.ToLower().Contains("list"))
                    {
                        var value = item.GetValue(delivery).ToString();
                        deliveryDictionary.Add(item.Name, new AttributeValue { S = value });
                    }
                    else
                    {
                        var value = item.GetValue(delivery).ToString();
                        if (item.GetValue(delivery).GetType().Name == "DateTime")
                            value = DateTime.Parse(item.GetValue(delivery).ToString()).ToString("yyyy-MM-dd");
                        deliveryDictionary.Add(item.Name, new AttributeValue { S = value });
                    }
                }
            }
            return deliveryDictionary;
        }
    }
}
