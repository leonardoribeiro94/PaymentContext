using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentContext.Tests.Entities
{
    public class StudentTests
    {
        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var document = new Document("123456789", EDocumentType.CPF);
            var name = new Name("John", "Doe");
            var email = new Email("john.doe@example.com");
            var address = new Address("123 Main St", "1001", "Downtown", "New York", "NY", "USA", "10001");
            var student = new Student(name, document, email);

            // Act 

            // Assert
        }
    }
}
