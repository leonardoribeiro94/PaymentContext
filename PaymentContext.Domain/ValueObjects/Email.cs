

using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        public Email(string address)
        {
            Address = address;
        }

        public string Address { get; private set; }
    }
}
