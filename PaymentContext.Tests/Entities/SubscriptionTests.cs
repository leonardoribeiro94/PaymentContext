using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Entities.Payments;
using PaymentContext.Tests.TestHelpers;

namespace PaymentContext.Tests.Entities
{
    [Trait("Category", "Entity")]
    public class SubscriptionTests
    {
        [Fact]
        public void ShouldStartActiveWhenCreated()
        {
            var subscription = new Subscription(null);

            Assert.True(subscription.Active);
            Assert.Null(subscription.ExpireDate);
            Assert.Empty(subscription.Payments);
        }

        [Fact]
        public void ShouldBecomeInactiveWhenInactivateIsCalled()
        {
            var subscription = new Subscription(null);
            var lastUpdateBefore = subscription.LastUpdateDate;

            subscription.Inactivate();

            Assert.False(subscription.Active);
            // >= porque em máquinas rápidas o relógio pode não ter avançado
            // entre os dois DateTime.Now — evita teste "flaky" por causa de
            // precisão de tempo.
            Assert.True(subscription.LastUpdateDate >= lastUpdateBefore);
        }

        [Fact]
        public void ShouldBecomeActiveAgainWhenActivateIsCalled()
        {
            var subscription = new Subscription(null);
            subscription.Inactivate();

            subscription.Activate();

            Assert.True(subscription.Active);
        }

 

        [Fact]
        public void AddPayment_NotifiesWhenPaymentDayIsNotBeforeTodayDay()
        {
            // A regra de validação compara só o NÚMERO do dia (payment.PaidDate.Day)
            // com DateTime.Now.Day — ignorando mês e ano. Isso por si só já é
            // um bug de modelagem (dia 31/01 "conta" como maior que dia 05/12,
            // por exemplo). Só de conhecer a regra dá pra provocar o caso de
            // notificação de forma 100% determinística: usar a MESMA data de
            // hoje sempre bate "dia <= dia" e sempre notifica, não importa
            // quando o teste rodar.
            var subscription = new Subscription(null);
            var payment = BuildPayment(paidDate: DateTime.Now);

            subscription.AddPayment(payment);

            Assert.False(subscription.IsValid);
            Assert.Contains(subscription.Notifications, n => n.Key == "Subscription.Payments");
        }

        [Fact]
        public void AddPayment_DoesNotNotifyWhenPaymentDayIsBeforeTodayDay()
        {
            // Nota sobre testabilidade: como a regra lê DateTime.Now
            // diretamente (em vez de receber um relógio injetado, ex.:
            // IDateTimeProvider), não dá pra controlar "hoje" no teste — o
            // teste fica refém do dia real em que ele roda. O ideal a longo
            // prazo é abstrair o "agora" atrás de uma interface para o teste
            // poder fixar uma data qualquer. Por enquanto, contornamos usando
            // "ontem", que garante Day = hoje.Day - 1 em qualquer mês —
            // exceto no dia 1, onde a própria regra não tem uma resposta
            // válida (não existe "dia menor que 1"). Nesse caso raro, o teste
            // é pulado conscientemente.
            if (DateTime.Now.Day == 1)
                return;

            var subscription = new Subscription(null);
            var payment = BuildPayment(paidDate: DateTime.Now.AddDays(-1));

            subscription.AddPayment(payment);

            Assert.True(subscription.IsValid);
            Assert.Single(subscription.Payments);
        }

        // Helper local só pra este arquivo: monta um Payment concreto
        // (BoletoPayment) válido, variando apenas a data de pagamento — que é
        // o único dado que os testes acima precisam controlar.
        private static Payment BuildPayment(DateTime paidDate)
        {
            return new BoletoPayment(
                barCode: "123456",
                boletoNumber: "789",
                paidDate: paidDate,
                expireDate: DateTime.Now.AddDays(5),
                total: 100,
                totalPaid: 100,
                owner: "John Doe",
                document: DomainObjectMother.ValidCpf(),
                email: DomainObjectMother.ValidEmail(),
                address: DomainObjectMother.ValidAddress());
        }
    }
}
