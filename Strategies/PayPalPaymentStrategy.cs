using RentalSystem.Models;
using RentalSystem.Services;

namespace RentalSystem.Strategies
{
    public class PayPalPaymentStrategy : IPaymentStrategy
    {
        public PaymentResult ProcessPayment(Payment payment)
        {
            // Имитация вызова PayPal API
            Console.WriteLine($"[PayPal] Initiating payment of ${payment.Amount:F2} for application {payment.Application.Id}");
            
            // В реальном проекте здесь был бы вызов PayPal REST SDK
            // Например: var createdPayment = await payment.Create(apiContext, ...);

            return new PaymentResult
            {
                Success = true,
                TransactionId = $"paypal_{Guid.NewGuid().ToString("N")[..10]}",
                Message = "Payment approved by PayPal"
            };
        }
    }
}