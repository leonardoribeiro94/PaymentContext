using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.ValueObjects
{
    [Trait("Category", "ValueObject")]
    public class AddressTests
    {
        [Fact]
        public void ShouldBeValidWhenAllFieldsAreFilled()
        {
            var address = new Address("123 Main St", "1001", "Downtown", "New York", "NY", "USA", "10001");

            Assert.True(address.IsValid);
        }

        // Address tem 7 campos obrigatórios e todos usam a MESMA regra
        // (IsNotNullOrEmpty). Em vez de escrever 7 métodos de teste quase
        // idênticos, usamos Theory + MemberData passando "qual posição fica
        // vazia" e montamos o Address dentro do teste.
        public static IEnumerable<object[]> MissingFieldCases =>
            new List<object[]>
            {
                new object[] { "street" },
                new object[] { "number" },
                new object[] { "neighborhood" },
                new object[] { "city" },
                new object[] { "state" },
                new object[] { "country" },
                new object[] { "zipCode" },
            };

        [Theory]
        [MemberData(nameof(MissingFieldCases))]
        public void ShouldNotifyMissingFieldWhenAnyRequiredFieldIsEmpty(string missingField)
        {
            var fields = new Dictionary<string, string>
            {
                ["street"] = "123 Main St",
                ["number"] = "1001",
                ["neighborhood"] = "Downtown",
                ["city"] = "New York",
                ["state"] = "NY",
                ["country"] = "USA",
                ["zipCode"] = "10001",
            };
            fields[missingField] = string.Empty;

            var address = new Address(
                fields["street"], fields["number"], fields["neighborhood"],
                fields["city"], fields["state"], fields["country"], fields["zipCode"]);

            Assert.False(address.IsValid);
            // Confirma que a notificação aponta exatamente pro campo vazio,
            // não só "algo deu errado em algum lugar".
            Assert.Contains(address.Notifications, n => n.Key == missingField);
        }
    }
}
