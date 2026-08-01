using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities.Payments
{
    public class BoletoPayment : Payment
    {
        public BoletoPayment(string barCode, string boletoNumber,
            DateTime paidDate,
            DateTime expireDate,
            decimal total,
            decimal totalPaid,
            string owner,
            Document document,
            Email email,
            Address address) :
            base(paidDate, expireDate, total, totalPaid, owner, document, email, address)
        {
            BarCode = barCode;
            BoletoNumber = boletoNumber;
        }

        public string BarCode { get; private set; }
        public string BoletoNumber { get; private set; }
    }
}
