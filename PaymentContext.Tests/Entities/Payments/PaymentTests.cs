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
        public void ShouldNotifyWhenTotalPaidExceedsTotal()
        {
            // Olhando a implementação:
            //   IsGreaterOrEqualsThan(value: total, comparer: totalPaid, ...)
            //   { if (value < comparer) AddNotification(...); }
            // Ou seja, notifica quando total < totalPaid — isto é, quando
            // pagaram A MAIS do que o total devido.
            var payment = BuildPayment(total: 100, totalPaid: 150);

            Assert.False(payment.IsValid);
            Assert.Contains(payment.Notifications, n => n.Key == "Payment.TotalPaid");
        }

        [Fact]
        public void ShouldCurrentlyAcceptTotalPaidLowerThanTotal()
        {
            // Isto aqui é o achado mais importante deste arquivo: a mensagem
            // da regra diz "Total paid must be greater than or equal to
            // total", mas a CONDIÇÃO implementada só dispara quando
            // total < totalPaid (pagou a mais). Pagar MENOS do que o total
            // (totalPaid = 50, total = 100) não é pego por nenhuma regra —
            // mesmo pagando bem menos, o Payment sai válido. Documentando o
            // comportamento atual; a mensagem sugere que a intenção original
            // era o contrário (validar SUBPAGAMENTO, não sobrepagamento).
            var payment = BuildPayment(total: 100, totalPaid: 50);

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
