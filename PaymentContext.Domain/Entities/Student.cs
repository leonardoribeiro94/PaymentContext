

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

            // cencela todas as outras assinaturas, e coloca esta como principal
            foreach (var sub in _subscriptions)
            {
                if (sub.Active)
                    hassubscriptionActive = true;
            }

            Contract.New(this)
                .IsFalse(hassubscriptionActive, "student.subscription", "Você já tem uma assinatura ativa.")
                .IsTrue(hassubscriptionActive, "student.subscription", "Assinatura adicionada com sucesso.");


            
        }

    }
}