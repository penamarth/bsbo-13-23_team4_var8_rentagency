// Strategies/StripePaymentStrategy.cs
using RentalSystem.Services; // для IPaymentStrategy и PaymentResult
using RentalSystem.Models;   // для Payment

namespace RentalSystem.Strategies
{
    public class StripePaymentStrategy : IPaymentStrategy
    {
        public PaymentResult ProcessPayment(Payment payment)
        {
            Console.WriteLine($"[Stripe] Processing ${payment.Amount:F2} for application {payment.Application.Id}");
            return new PaymentResult
            {
                Success = true,
                TransactionId = $"stripe_{Guid.NewGuid().ToString("N")[..8]}",
                Message = "Success"
            };
        }
    }
}