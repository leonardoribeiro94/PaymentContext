# PaymentContext

Projeto de estudo em .NET 10 explorando Domain-Driven Design (DDD): entidades, value objects, e um sistema de notificação (notification pattern) para validação sem uso de exceptions.

O domínio modela um sistema de assinaturas de curso: um `Student` possui `Subscription`s, e cada assinatura possui `Payment`s (Boleto, Cartão de Crédito ou PayPal).

## Estrutura da solution

- **PaymentContext.Domain** — entidades e value objects do domínio (`Student`, `Subscription`, `Payment` e suas variações, `Address`, `Document`, `Email`, `Name`).
- **PaymentContext.Shared** — infraestrutura reutilizável: `Entity` base, `ValueObject` base, e o notification pattern (`Notifiable`, `Notification`, `Contract`) usado para validação de invariantes.
- **PaymentContext.Tests** — testes unitários (xUnit).

## Requisitos

- .NET SDK 10.0

## Como rodar

```bash
dotnet restore
dotnet build
dotnet test
```

## Conceitos aplicados

- **Value Objects** (`Address`, `Document`, `Email`, `Name`) — imutáveis, validados na construção via `Contract`.
- **Notification Pattern** — em vez de lançar exceptions, objetos herdam de `Notifiable` e acumulam `Notification`s, permitindo checar `IsValid`/`IsInvalid` após a validação.
- **Herança de pagamentos** — `Payment` é abstrata; `BoletoPayment`, `CreditCardPayment` e `PayPalPayment` especializam o comportamento comum.
