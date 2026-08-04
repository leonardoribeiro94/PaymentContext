using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;

namespace PaymentContext.Domain.Commands
{
    public record CreateBoletoSubscriptionCommand(string FirstName,
    string LastName,
    string Document,
    string Email,

    string BarCode,
    string BoletoNumber,

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