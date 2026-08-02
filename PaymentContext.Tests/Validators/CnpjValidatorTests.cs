using PaymentContext.Domain.Validators;

namespace PaymentContext.Tests.Validators
{
    [Trait("Category", "Validator")]
    public class CnpjValidatorTests
    {
        [Theory]
        [InlineData("12345678000195")]
        [InlineData("11222333000181")]
        [InlineData("04252011000110")]
        public void ShouldReturnTrueForValidCnpj(string cnpj)
        {
            Assert.True(CnpjValidator.IsValid(cnpj));
        }

        [Theory]
        [InlineData("11111111111111")] // todos os dígitos iguais
        [InlineData("12345678000190")] // dígito verificador errado
        [InlineData("123456")]          // tamanho errado
        public void ShouldReturnFalseForInvalidCnpj(string cnpj)
        {
            Assert.False(CnpjValidator.IsValid(cnpj));
        }

        // Este validador (diferente do CpfValidator) trata null/whitespace
        // explicitamente com um IsNullOrWhiteSpace no início. Vale um teste
        // dedicado pra esse caminho, já que o outro validador não tem essa
        // proteção e quebraria com null.
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldReturnFalseForNullOrWhitespace(string? cnpj)
        {
            Assert.False(CnpjValidator.IsValid(cnpj!));
        }

        [Fact]
        public void ShouldIgnoreNonDigitCharactersLikeDotsAndDashes()
        {
            Assert.True(CnpjValidator.IsValid("12.345.678/0001-95"));
        }
    }
}
