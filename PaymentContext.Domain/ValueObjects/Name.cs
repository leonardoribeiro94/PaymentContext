

using PaymentContext.Shared.Notifications;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public sealed class Name : ValueObject
    {
      
        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

           Contract.New(this)
                .IsNotNullOrEmpty(FirstName, "Name.FirstName", "First name cannot be null or empty")
                .HasMinLength(FirstName, 3, "Name.FirstName", "First name must have at least 3 characters")
                .IsNotNullOrEmpty(LastName, "Name.LastName", "Last name cannot be null or empty")
                .HasMinLength(LastName, 3, "Name.LastName", "Last name must have at least 3 characters");
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
    }
}
