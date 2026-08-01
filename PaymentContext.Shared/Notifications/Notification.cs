namespace PaymentContext.Shared.Notifications
{
    public class Notification
    {
        public Notification(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; private set; }
        public string Value { get; private set; }
    }
}
