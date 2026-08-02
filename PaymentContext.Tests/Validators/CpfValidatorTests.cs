using PaymentContext.Domain.Validators;

namespace PaymentContext.Tests.Validators
{
    // Diferença pra DocumentTests: ali testamos Document (o value object,
    // que envolve tamanho mínimo/máximo + Contract + notificações). Aqui
    // testamos o CpfValidator isolado — só o algoritmo do dígito
    // verificador, sem passar pelo resto das regras. É a diferença entre
    // "unit test de um VO" e "unit test de uma função pura utilitária".
    [Trait("Category", "Validator")]
    public class CpfValidatorTests
    {
        [Theory]
        [InlineData("95034048000")]
        [InlineData("11144477735")]
        [InlineData("12345678909")]
        public void ShouldReturnTrueForValidCpf(string cpf)
        {
            Assert.True(CpfValidator.IsValid(cpf));
        }

        [Theory]
        [InlineData("11111111111")] // todos os dígitos iguais
        [InlineData("00000000000")]
        [InlineData("12345678901")] // dígito verificador errado
        [InlineData("123456789")]   // tamanho errado (validator espera 11 dígitos)
        public void ShouldReturnFalseForInvalidCpf(string cpf)
        {
            Assert.False(CpfValidator.IsValid(cpf));
        }

        [Fact]
        public void ShouldIgnoreNonDigitCharactersLikeDotsAndDashes()
        {
            // O validador filtra "cpf.Where(char.IsDigit)" antes de validar,
            // então o formato "950.340.480-00" deve validar igual a
            // "95034048000".
            Assert.True(CpfValidator.IsValid("950.340.480-00"));
        }
    }
}
