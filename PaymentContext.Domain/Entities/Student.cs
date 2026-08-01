

using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Entities;

namespace PaymentContext.Domain.Entities
{
    public class Student : Entity
    {
        private IList<Subscription> _subscriptions;

        public Student(Name name, Document document, Email email)
        {
            Name = name;
            Document = document;
            Email = email;
            _subscriptions = new List<Subscription>();
        }

        public Name Name { get; private set; }
        public Document Document { get; }
        public Email Email { get; }
        public Address Address { get; }
        public IReadOnlyCollection<Subscription> Subscriptions { get => _subscriptions.ToArray(); }

        public void AddSubscription(Subscription subscription)
        {

            // cencela todas as outras assinaturas, e coloca esta como principal
            foreach (var sub in Subscriptions)
            {
                sub.Inactivate();
            }

            _subscriptions.Add(subscription);
        }

    }
}