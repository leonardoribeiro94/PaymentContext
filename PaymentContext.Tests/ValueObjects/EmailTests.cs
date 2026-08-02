using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.ValueObjects
{
    [Trait("Category", "ValueObject")]
    public class EmailTests
    {
        [Theory]
        [InlineData("john.doe@example.com")]
        [InlineData("a@b.co")]
        [InlineData("first.last+tag@sub.domain.com")]
        public void ShouldBeValidWhenAddressIsWellFormed(string address)
        {
            var email = new Email(address);

            Assert.True(email.IsValid);
        }

        [Theory]
        [InlineData("not-an-email")]
        [InlineData("missing-domain@")]
        [InlineData("@missing-user.com")]
        [InlineData("")]
        public void ShouldBeInvalidWhenAddressIsMalformed(string address)
        {
            var email = new Email(address);

            Assert.False(email.IsValid);
        }

        // Assert.Throws é o jeito idiomático de testar que uma exceção específica
        // é lançada. Aqui não usamos porque Email não lança (ele acumula
        // notificação em vez de exception), mas fica documentado como
        // referência: se algum dia o construtor mudar pra lançar
        // ArgumentNullException em vez de notificar, o teste ficaria assim:
        //
        // [Fact]
        // public void ShouldThrowWhenAddressIsNull()
        // {
        //     Assert.Throws<ArgumentNullException>(() => new Email(null!));
        // }
    }
}
