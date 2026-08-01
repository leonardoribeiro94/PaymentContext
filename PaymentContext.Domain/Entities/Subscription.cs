using PaymentContext.Domain.Entities.Payments;

namespace PaymentContext.Domain.Entities
{

    public class Subscription
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
            foreach (var item in _payments)
                _payments.Remove(item);

            _payments.Add(payment);
        }

        public void Inactivate()
        {
            Active = false; LastUpdateDate = DateTime.Now;
        }

        public void Activate()
        {
            Active = true;
            LastUpdateDate = DateTime.Now;
        }
    }
}