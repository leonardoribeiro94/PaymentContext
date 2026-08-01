using PaymentContext.Shared.Notifications;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public sealed class Address : ValueObject
    {
        public Address(string street, string number, string neighborhood, string city, string state, string country, string zipCode)
        {
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;

            Contract.New(this)
                .IsNotNullOrEmpty(street, nameof(street), "Street is required")
                .IsNotNullOrEmpty(number, nameof(number), "Number is required")
                .IsNotNullOrEmpty(neighborhood, nameof(neighborhood), "Neighborhood is required")
                .IsNotNullOrEmpty(city, nameof(city), "City is required")
                .IsNotNullOrEmpty(state, nameof(state), "State is required")
                .IsNotNullOrEmpty(country, nameof(country), "Country is required")
                .IsNotNullOrEmpty(zipCode, nameof(zipCode), "Zip code is required");
        }

        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
    }
}
