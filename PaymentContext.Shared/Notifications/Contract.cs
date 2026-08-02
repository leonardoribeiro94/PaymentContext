namespace PaymentContext.Shared.Notifications
{
    public static class Contract
    {
        public static ContractBuilder<T> New<T>(T notifiable) where T : Notifiable
            => new ContractBuilder<T>(notifiable);
    }

    public class ContractBuilder<T> where T : Notifiable
    {
        private readonly T _notifiable;
        public ContractBuilder(T notifiable)
        {
            _notifiable = notifiable;
        }

        public ContractBuilder<T> Requires()
        {
            return this;
        }

        public ContractBuilder<T> IsGreaterThan(int value, int comparer, string key, string message)
        {
            if (value <= comparer)
            {
                _notifiable.AddNotification(key, message);
            }
            return this;
        }

        public ContractBuilder<T> IsGreaterThan(DateTime value, DateTime comparer, string key, string message)
        {
            if (value <= comparer)
            {
                _notifiable.AddNotification(key, message);
            }
            return this;
        }

        public ContractBuilder<T> IsGreaterThan(Decimal value, Decimal comparer, string key, string message)
        {
            if (value <= comparer)
            {
                _notifiable.AddNotification(key, message);
            }

            return this;
        }

        public ContractBuilder<T> IsGreaterOrEqualsThan(int value, int comparer, string key, string message)
        {
            if (value < comparer)
            {
                _notifiable.AddNotification(key, message);
            }
            return this;
        }

        public ContractBuilder<T> IsGreaterOrEqualsThan(Decimal value, Decimal comparer, string key, string message)
        {
            if (value < comparer)
            {
                _notifiable.AddNotification(key, message);
            }
            return this;
        }

        public ContractBuilder<T> IsLowerThan(int value, int comparer, string key, string message)
        {
            if (value >= comparer)
            {
                _notifiable.AddNotification(key, message);
            }
            return this;
        }

        public ContractBuilder<T> IsNotNullOrEmpty(string value, string key, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                _notifiable.AddNotification(key, message);
            }
            return this;
        }

        public ContractBuilder<T> IsEmail(string value, string key, string message)
        {
            if (!IsValidEmail(value))
            {
                _notifiable.AddNotification(key, message);
            }
            return this;
        }

        public ContractBuilder<T> HasMinLength(string value, int minLength, string key, string message)
        {
            if (value == null || value.Length < minLength)
            {
                _notifiable.AddNotification(key, message);
            }

            return this;
        }

        public ContractBuilder<T> HasMaxLength(string value, int maxLength, string key, string message)
        {
            if (value != null && value.Length > maxLength)
            {
                _notifiable.AddNotification(key, message);
            }
            return this;
        }

        public ContractBuilder<T> IsTrue(bool value, string key, string message)
        {
            if (value != true)
            {
                _notifiable.AddNotification(key, message);

            }

            return this;
        }

        public ContractBuilder<T> IsFalse(bool value, string key, string message)
        {
            if (value != false)
            {
                _notifiable.AddNotification(key, message);
            }

            return this;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

