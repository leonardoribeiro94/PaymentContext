using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.ValueObjects
{
    [Trait("Category", "ValueObject")]
    public class NameTests
    {
        [Fact]
        public void ShouldBeValidWhenFirstAndLastNameAreCorrect()
        {
            var name = new Name("John", "Doe");

            Assert.True(name.IsValid);
            Assert.Equal("John", name.FirstName);
            Assert.Equal("Doe", name.LastName);
        }

        // [MemberData] é usado quando o dado do Theory não é um literal simples
        // (o InlineData só aceita constantes de compilação: string, número, enum,
        // etc). Aqui não precisaríamos dele ainda, já que first/last são strings,
        // mas ele fica útil quando o "esperado" também é uma coleção, um objeto,
        // ou quando os casos de teste vêm de uma lógica compartilhada.
        public static IEnumerable<object[]> InvalidNames =>
            new List<object[]>
            {
                new object[] { "", "Doe" },      // first name vazio
                new object[] { "Al", "Doe" },     // first name com menos de 3 caracteres
                new object[] { "John", "" },      // last name vazio
                new object[] { "John", "Do" },    // last name com menos de 3 caracteres
            };

        [Theory]
        [MemberData(nameof(InvalidNames))]
        public void ShouldBeInvalidWhenNameDoesNotMeetMinimumLength(string firstName, string lastName)
        {
            var name = new Name(firstName, lastName);

            Assert.False(name.IsValid);
        }

        [Fact]
        public void ShouldNotifyBothFieldsWhenBothAreInvalid()
        {
            // Diferente do teste acima (que só confirma "deu inválido"), aqui
            // verificamos QUANTAS notificações saem quando as duas regras
            // quebram ao mesmo tempo — o Contract não para no primeiro erro,
            // ele acumula todos.
            var name = new Name("Al", "Do");

            Assert.Equal(2, name.Notifications.Count);
        }
    }
}
