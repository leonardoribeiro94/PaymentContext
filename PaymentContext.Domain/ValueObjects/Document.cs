using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Validators;
using PaymentContext.Shared.Notifications;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public sealed class Document : ValueObject
    {
        public Document(string documentNumber, EDocumentType type)
        {
            DocumentNumber = documentNumber;
            Type = type;

            Contract.New(this)
                .IsNotNullOrEmpty(documentNumber, nameof(documentNumber), "Document number is required")
                .HasMinLength(documentNumber, 11, nameof(documentNumber), "Document number must have at least 11 characters")
                .HasMaxLength(documentNumber, 14, nameof(documentNumber), "Document number cannot exceed 14 characters")
                .IsTrue(IsValidDocument(), nameof(documentNumber), "Document number is invalid");
        }

        public string DocumentNumber { get; private set; }
        public EDocumentType Type { get; private set; }

        private bool IsValidDocument() => Type switch
        {
            EDocumentType.CPF => CpfValidator.IsValid(DocumentNumber),
            EDocumentType.CNPJ => CnpjValidator.IsValid(DocumentNumber),
            _ => false
        };
    }
}
