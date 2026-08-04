using System.Reflection.Metadata;
using PaymentContext.Domain.Enums;
using PaymentContext.Shared.Commands;

namespace PaymentContext.Domain.Commands
{
    public record CreateCreditCardSubscriptionCommand(string FirstName,
    string LastName,
    string Document,
    string Email,

    string CardHolderName,
    string CardNumber,

    string PaymentNumber,
    DateTime PaidDate,
    DateTime ExpireDate,
    decimal Total,
    decimal TotalPaid,
    string Payer,
    Document PayerDocument,
    EDocumentType PayerDocumentType,
    string PayerEmail,
    string Street,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string Country,
    string ZipCode) : ICommand;

}