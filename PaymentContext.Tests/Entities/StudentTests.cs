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
        public void AddSubscription_StillDoesNotAddTheSubscriptionToTheCollection()
        {
            // Este teste passa hoje, mas documenta um bug que AINDA não foi
            // corrigido (independente da regra de pagamentos acima): o
            // método nunca faz "_subscriptions.Add(subscription)", então
            // Student.Subscriptions continua vazio mesmo numa chamada
            // "bem-sucedida" (válida). Quando você adicionar o Add que
            // falta, troque o Assert.Empty abaixo pelas duas linhas
            // comentadas.
            var student = DomainObjectMother.ValidStudent();
            var subscription = DomainObjectMother.ValidSubscriptionWithPayment();

            student.AddSubscription(subscription);

            Assert.Empty(student.Subscriptions);

            // Assert.Single(student.Subscriptions);
            // Assert.Same(subscription, student.Subscriptions.First());
        }

        [Fact]
        public void AddSubscription_SecondCallStillDoesNotDetectAnActiveSubscription()
        {
            // Consequência direta do bug acima: como nenhuma assinatura
            // chega a entrar em _subscriptions, "hassubscriptionActive"
            // nunca fica true via API pública — a regra "Student.Subscriptions"
            // (já tem assinatura ativa) está com o caminho de acesso quebrado
            // (unreachable), mesmo a lógica dela estando correta agora.
            var student = DomainObjectMother.ValidStudent();
            var first = DomainObjectMother.ValidSubscriptionWithPayment();
            var second = DomainObjectMother.ValidSubscriptionWithPayment();

            student.AddSubscription(first);
            student.AddSubscription(second);

            Assert.True(student.IsValid);
            Assert.Empty(student.Notifications);
        }
    }
}
