using PaymentContext.Domain.Entities.Payments;
using PaymentContext.Shared.Entities;
using PaymentContext.Shared.Notifications;

namespace PaymentContext.Domain.Entities
{

    public class Subscription : Entity
    {
        private IList<Payment> _payments;

        public Subscription(DateTime? expireDate)
        {
            CreateDate = DateTime.Now;
            LastUpdateDate = DateTime.Now;
            Active = true;
            ExpireDate = expireDate;
            _payments = new List<Payment>();
        }

        public DateTime CreateDate { get; }
        public DateTime LastUpdateDate { get; private set; }
        public DateTime? ExpireDate { get; }
        public bool Active { get; private set; }
        public IReadOnlyCollection<Payment> Payments { get => _payments.ToArray(); }


        public void AddPayment(Payment payment)
        {
            Contract.New(this)
                .Requires()
                .IsGreaterThan(DateTime.Now, payment.PaidDate, "Subscription.Payments", "A data do pagamento deve ser futura");

            if (IsValid)
                _payments.Add(payment);
        }

        public void Inactivate()
        {
            Active = false;
            LastUpdateDate = DateTime.Now;
        }

        public void Activate()
        {
            Active = true;
            LastUpdateDate = DateTime.Now;
        }
    }
}