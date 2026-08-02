using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.ValueObjects
{
    // [Trait] não afeta a execução, só adiciona metadado pra filtrar depois:
    // dotnet test --filter Category=ValueObject
    [Trait("Category", "ValueObject")]
    public class DocumentTests
    {
        // [Theory] + [InlineData] roda o MESMO método uma vez por linha de dado.
        // Cada InlineData aparece como um caso separado no Test Explorer, então
        // se um número falhar você sabe exatamente qual, sem duplicar código
        // (era isso que os 4 [Fact] originais faziam de forma repetitiva).
        [Theory]
        [InlineData("95034048000", EDocumentType.CPF, true)]      // CPF válido
        [InlineData("11144477735", EDocumentType.CPF, true)]      // outro CPF válido (dígitos diferentes)
        [InlineData("123456789", EDocumentType.CPF, false)]       // dígito verificador errado
        [InlineData("11111111111", EDocumentType.CPF, false)]     // todos os dígitos iguais
        [InlineData("19714010000117", EDocumentType.CNPJ, true)]  // CNPJ válido
        [InlineData("11222333000181", EDocumentType.CNPJ, true)]  // outro CNPJ válido
        [InlineData("123456", EDocumentType.CNPJ, false)]         // tamanho errado
        [InlineData("11111111111111", EDocumentType.CNPJ, false)] // todos os dígitos iguais
        public void ShouldValidateDocumentAccordingToType(string documentNumber, EDocumentType type, bool expected)
        {
            // Arrange + Act (a validação roda dentro do construtor, via Contract)
            var document = new Document(documentNumber, type);

            // Assert
            Assert.Equal(expected, document.IsValid);
        }

        // Testes de borda (boundary tests): o Contract.HasMinLength/HasMaxLength
        // barra tamanho fora de 11-14 caracteres ANTES de rodar o validador de
        // CPF/CNPJ. É uma regra diferente da regra de dígito verificador acima,
        // então vale isolar e nomear o motivo específico da falha.
        [Fact]
        public void ShouldNotifyWhenDocumentNumberIsShorterThanMinLength()
        {
            // "123456789" tem 9 caracteres, abaixo do mínimo de 11
            var document = new Document("123456789", EDocumentType.CPF);

            Assert.False(document.IsValid);
            // Em vez de só checar IsValid, aqui confirmamos QUAL notificação
            // disparou — isso é o valor real de testar um padrão Notification.
            Assert.Contains(document.Notifications, n => n.Key == "documentNumber");
        }

        [Fact]
        public void ShouldNotifyWhenDocumentNumberIsLongerThanMaxLength()
        {
            // 15 dígitos, acima do máximo de 14
            var document = new Document("123456789012345", EDocumentType.CNPJ);

            Assert.False(document.IsValid);
            Assert.Contains(document.Notifications, n => n.Key == "documentNumber");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldNotifyWhenDocumentNumberIsNullOrEmpty(string? number)
        {
            var document = new Document(number!, EDocumentType.CPF);

            Assert.False(document.IsValid);
        }
    }
}
