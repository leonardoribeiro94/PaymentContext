using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Entities;
using PaymentContext.Shared.Notifications;

namespace PaymentContext.Domain.Entities.Payments
{
    public abstract class Payment : Entity
    {
        protected Payment(DateTime paidDate, DateTime expireDate, decimal total, decimal totalPaid, string owner, Document document, Email email, Address address)
        {
            Number = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper();
            PaidDate = paidDate;
            ExpireDate = expireDate;
            Total = total;
            TotalPaid = totalPaid;
            Owner = owner;
            Document = document;
            Email = email;
            Address = address;

            Contract.New(this)
                .IsGreaterThan(total, 0, "Payment.Total", "Total must be greater than zero")
                .IsGreaterOrEqualsThan(total, totalPaid, "Payment.TotalPaid", "Total paid must be greater than or equal to total");
        }

        public string Number { get; private set; }
        public DateTime PaidDate { get; private set; }
        public DateTime ExpireDate { get; private set; }
        public decimal Total { get; private set; }
        public decimal TotalPaid { get; private set; }
        public string Owner { get; private set; }
        public Document Document { get; private set; }
        public Email Email { get; private set; }
        public Address Address { get; private set; }
    }
}