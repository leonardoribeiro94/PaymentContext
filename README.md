# PaymentContext

A .NET 10 study project exploring Domain-Driven Design (DDD): entities, value objects, and a notification pattern for validation without exceptions.

The domain models a course subscription system: a `Student` has `Subscription`s, and each subscription has `Payment`s (Boleto, Credit Card, or PayPal).

## Solution structure

- **PaymentContext.Domain** — domain entities and value objects (`Student`, `Subscription`, `Payment` and its variants, `Address`, `Document`, `Email`, `Name`).
- **PaymentContext.Shared** — reusable infrastructure: base `Entity`, base `ValueObject`, and the notification pattern (`Notifiable`, `Notification`, `Contract`) used for invariant validation.
- **PaymentContext.Tests** — unit tests (xUnit).

## Requirements

- .NET SDK 10.0

## Getting started

```bash
dotnet restore
dotnet build
dotnet test
```

## Concepts applied

- **Value Objects** (`Address`, `Document`, `Email`, `Name`) — immutable, validated on construction via `Contract`.
- **Notification Pattern** — instead of throwing exceptions, objects inherit from `Notifiable` and accumulate `Notification`s, allowing `IsValid`/`IsInvalid` checks after validation.
- **Payment inheritance** — `Payment` is abstract; `BoletoPayment`, `CreditCardPayment`, and `PayPalPayment` specialize the common behavior.
