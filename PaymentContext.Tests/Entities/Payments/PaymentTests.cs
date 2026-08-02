using PaymentContext.Domain.Entities.Payments;
using PaymentContext.Tests.TestHelpers;

namespace PaymentContext.Tests.Entities.Payments
{
    // Payment é abstract — não dá pra instanciar direto. Pra testar as
    // regras que moram na classe base (Contract no construtor de Payment),
    // usamos uma implementação concreta qualquer (BoletoPayment) só como
    // veículo. O teste é sobre o comportamento herdado, não sobre boleto
    // em si — por isso o nome da classe/testes fala de "Payment", não
    // de "BoletoPayment".
    [Trait("Category", "Entity")]
    public class PaymentTests
    {
        [Fact]
        public void ShouldBeValidWhenTotalIsGreaterThanZeroAndTotalPaidCoversTotal()
        {
            var payment = BuildPayment(total: 100, totalPaid: 100);

            Assert.True(payment.IsValid);
        }

        [Fact]
        public void ShouldGenerateAnUppercaseTenCharacterNumber()
        {
            // Regra implícita do construtor: Number vem de um Guid, cortado
            // pros primeiros 10 caracteres e em maiúsculo. Vale travar esse
            // formato em teste pra pegar regressão se alguém mudar a lógica.
            var payment = BuildPayment(total: 100, totalPaid: 100);

            Assert.Equal(10, payment.Number.Length);
            Assert.Equal(payment.Number.ToUpper(), payment.Number);
            Assert.DoesNotContain("-", payment.Number);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void ShouldNotifyWhenTotalIsNotGreaterThanZero(decimal total)
        {
            var payment = BuildPayment(total: total, totalPaid: total);

            Assert.False(payment.IsValid);
            Assert.Contains(payment.Notifications, n => n.Key == "Payment.Total");
        }

        [Fact]
        public void ShouldNotifyWhenTotalPaidIsLessThanTotal()
        {
            // Bug corrigido: a chamada virou
            //   IsGreaterOrEqualsThan(value: totalPaid, comparer: total, ...)
            //   { if (value < comparer) AddNotification(...); }
            // Agora notifica quando totalPaid < total — ou seja, quando
            // pagaram MENOS do que o total devido (subpagamento), que é o
            // que a mensagem de erro sempre sugeriu.
            var payment = BuildPayment(total: 100, totalPaid: 50);

            Assert.False(payment.IsValid);
            Assert.Contains(payment.Notifications, n => n.Key == "Payment.TotalPaid");
        }

        [Fact]
        public void ShouldAcceptTotalPaidGreaterThanTotal()
        {
            // Com a ordem corrigida, pagar A MAIS que o total (totalPaid =
            // 150, total = 100) passa a ser aceito — só subpagamento é
            // bloqueado agora. Se a regra de negócio real também deve barrar
            // sobrepagamento, falta uma checagem extra própria pra isso; a
            // atual (IsGreaterOrEqualsThan) só cobre uma direção por vez.
            var payment = BuildPayment(total: 100, totalPaid: 150);

            Assert.True(payment.IsValid);
        }

        private static Payment BuildPayment(decimal total, decimal totalPaid)
        {
            return new BoletoPayment(
                barCode: "123456",
                boletoNumber: "789",
                paidDate: DateTime.Now,
                expireDate: DateTime.Now.AddDays(5),
                total: total,
                totalPaid: totalPaid,
                owner: "John Doe",
                document: DomainObjectMother.ValidCpf(),
                email: DomainObjectMother.ValidEmail(),
                address: DomainObjectMother.ValidAddress());
        }
    }
}
