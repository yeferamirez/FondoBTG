using System.Runtime.Serialization;

namespace PensionFund.Domain.Exceptions
{
    [Serializable]
    public class NotificationException : Exception
    {
        public NotificationException(string message) : base(message)
        {
        }
        public NotificationException(string message, Exception exception) : base(message, exception)
        {
        }
        protected NotificationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
