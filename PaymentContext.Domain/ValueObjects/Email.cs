

using PaymentContext.Shared.Notifications;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        public Email(string address)
        {
            Address = address;

            Contract.New(this)
                .IsNotNullOrEmpty(address, nameof(address), "Email address is required")
                .IsEmail(address, nameof(address), "Invalid email address");
        }

        public string Address { get; private set; }
    }
}
