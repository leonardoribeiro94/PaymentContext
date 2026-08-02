

using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Entities;
using PaymentContext.Shared.Notifications;

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
            var hassubscriptionActive = false;

            foreach (var sub in _subscriptions)
            {
                if (sub.Active)
                    hassubscriptionActive = true;
            }

            Contract.New(this)
                .IsFalse(hassubscriptionActive, "Student.Subscriptions", "You already have an active subscription")
                .IsGreaterThan(subscription.Payments.Count, 0, "Student.Subscription.Payments", "This subscription has no payments");

            if (IsValid)
                _subscriptions.Add(subscription);
        }

    }
}