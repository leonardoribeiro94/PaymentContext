using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities.Payments
{
    public class PayPalPayment : Payment
    {
        public PayPalPayment(string transactionCode,
            DateTime paidDate, 
            DateTime expireDate, 
            decimal total, 
            decimal totalPaid, 
            string owner, 
            Document document,
            Email email,
            Address address)
            : base(paidDate, expireDate, total, totalPaid, owner, document, email, address)
        {
            TransactionCode = transactionCode;
        }

        public string TransactionCode { get; private set; }
    }
}
