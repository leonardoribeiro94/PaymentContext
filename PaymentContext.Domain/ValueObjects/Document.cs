using PaymentContext.Domain.Enums;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public sealed class Document : ValueObject
    {
        public Document(string documentNumber, EDocumentType type)
        {
            DocumentNumber = documentNumber;
            Type = type;
        }

        public string DocumentNumber { get; private set; }
        public EDocumentType Type { get; private set; }
    }
}
