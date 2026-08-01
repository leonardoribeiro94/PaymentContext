namespace PaymentContext.Shared.Notifications
{
    public abstract class Notifiable
    {
        private readonly List<Notification> _notifications;

        protected Notifiable()
        {
            _notifications = [];
        }

        public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void AddNotification(string key, string value)
        {
            _notifications.Add(new Notification(key, value));
        }

        public void AddNotifications(IEnumerable<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        // útil pra propagar notificações de um ValueObject filho pra Entity pai
        public void AddNotifications(Notifiable notifiable)
        {
            _notifications.AddRange(notifiable.Notifications);
        }


        public bool IsValid => !_notifications.Any();
        public bool IsInvalid => !IsValid;
    }
}
