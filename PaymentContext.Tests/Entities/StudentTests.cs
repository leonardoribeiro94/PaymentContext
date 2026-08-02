using PaymentContext.Domain.Entities;
using PaymentContext.Tests.TestHelpers;

namespace PaymentContext.Tests.Entities
{
    [Trait("Category", "Entity")]
    public class StudentTests
    {
        [Fact]
        public void ShouldBeValidWhenBuiltWithValidValueObjects()
        {
            // Arrange + Act: usando o Object Mother em vez de montar
            // Name/Document/Email/Address na mão em cada teste.
            var student = DomainObjectMother.ValidStudent();

            // Assert
            Assert.True(student.IsValid);
            Assert.Equal("John", student.Name.FirstName);
            Assert.Equal("Doe", student.Name.LastName);
            Assert.Equal("95034048000", student.Document.DocumentNumber);
        }

        // --- AddSubscription -------------------------------------------
        //
        // Você mudou a regra: agora, além de checar se já existe uma
        // assinatura ativa, o método também exige que a NOVA assinatura já
        // tenha pelo menos um pagamento:
        //
        //   Contract.New(this)
        //       .IsFalse(hassubscriptionActive, "Student.Subscriptions", "...")
        //       .IsGreaterThan(subscription.Payments.Count, 0, "Student.Subscription.Payments", "...");
        //
        // As duas regras são independentes (o Contract não para na
        // primeira que falhar), então testei cada uma isoladamente.

        [Fact]
        public void ShouldNotNotifyWhenSubscriptionHasAtLeastOnePaymentAndNoneIsActive()
        {
            var student = DomainObjectMother.ValidStudent();
            var subscription = DomainObjectMother.ValidSubscriptionWithPayment();

            student.AddSubscription(subscription);

            Assert.True(student.IsValid);
            Assert.Empty(student.Notifications);
        }

        [Fact]
        public void ShouldNotifyWhenSubscriptionHasNoPayments()
        {
            var student = DomainObjectMother.ValidStudent();
            var subscriptionWithoutPayments = new Subscription(null); // nenhum AddPayment chamado

            student.AddSubscription(subscriptionWithoutPayments);

            Assert.False(student.IsValid);
            Assert.Contains(student.Notifications, n => n.Key == "Student.Subscription.Payments");
        }

        [Fact]
        public void ShouldAddTheSubscriptionToTheCollectionWhenItIsValid()
        {
            var student = DomainObjectMother.ValidStudent();
            var subscription = DomainObjectMother.ValidSubscriptionWithPayment();

            student.AddSubscription(subscription);

            Assert.Single(student.Subscriptions);
            Assert.Same(subscription, student.Subscriptions.First());
        }

        [Fact]
        public void ShouldNotifyAndNotAddWhenStudentAlreadyHasAnActiveSubscription()
        {
            // Consequência boa do fix acima: a regra "Student.Subscriptions"
            // (já tem assinatura ativa) era código morto antes — agora que a
            // primeira assinatura fica de fato registrada em _subscriptions,
            // esse caminho é alcançável de verdade pela API pública.
            var student = DomainObjectMother.ValidStudent();
            var first = DomainObjectMother.ValidSubscriptionWithPayment();
            var second = DomainObjectMother.ValidSubscriptionWithPayment();

            student.AddSubscription(first);   // vira a assinatura ativa
            student.AddSubscription(second);  // deve ser rejeitada

            Assert.False(student.IsValid);
            Assert.Contains(student.Notifications, n => n.Key == "Student.Subscriptions");

            // A segunda assinatura não deve ter sido adicionada: só a
            // primeira permanece na coleção.
            Assert.Single(student.Subscriptions);
            Assert.Same(first, student.Subscriptions.First());
        }
    }
}
