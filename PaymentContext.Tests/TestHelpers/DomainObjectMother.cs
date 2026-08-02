using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Entities.Payments;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.TestHelpers
{
    // "Object Mother" é um padrão de teste: uma classe estática que sabe montar
    // objetos de domínio válidos e prontos pra usar no Arrange.
    //
    // Por que usar isso em vez de repetir "new Name(...)" em cada teste?
    // 1) Se o construtor de Name/Document/Email mudar, você ajusta em UM lugar.
    // 2) Deixa o Arrange dos testes com uma linha só, focando a leitura no que
    //    o teste realmente quer verificar (Act/Assert).
    // 3) Documenta, por si só, "o que é um Student/Document válido" pro projeto.
    public static class DomainObjectMother
    {
        public static Name ValidName()
            => new("John", "Doe");

        public static Document ValidCpf()
            => new("95034048000", EDocumentType.CPF);

        public static Document ValidCnpj()
            => new("95353084000153", EDocumentType.CNPJ);

        public static Email ValidEmail()
            => new("john.doe@example.com");

        public static Address ValidAddress()
            => new("123 Main St", "1001", "Downtown", "New York", "NY", "USA", "10001");

        public static Student ValidStudent()
            => new(ValidName(), ValidCpf(), ValidEmail());

        // Payment concreto qualquer (BoletoPayment), só como veículo pra
        // preencher a lista de pagamentos de uma Subscription. Usa "ontem"
        // como data de pagamento pra passar na regra (buggy, comparação só
        // do dia) de Subscription.AddPayment — ver comentário em
        // SubscriptionTests sobre a limitação disso no dia 1 do mês.
        public static Payment ValidPayment(DateTime? paidDate = null)
            => new BoletoPayment(
                barCode: "123456",
                boletoNumber: "789",
                paidDate: paidDate ?? DateTime.Now.AddDays(-1),
                expireDate: DateTime.Now.AddDays(5),
                total: 100,
                totalPaid: 100,
                owner: "John Doe",
                document: ValidCpf(),
                email: ValidEmail(),
                address: ValidAddress());

        // Subscription já com um pagamento anexado — hoje é pré-requisito
        // pra Student.AddSubscription não notificar "This subscription has
        // no payments".
        public static Subscription ValidSubscriptionWithPayment()
        {
            var subscription = new Subscription(null);
            subscription.AddPayment(ValidPayment());
            return subscription;
        }
    }
}
