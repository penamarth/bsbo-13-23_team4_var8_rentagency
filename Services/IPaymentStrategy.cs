// Services/IPaymentStrategy.cs
using RentalSystem.Models; // нужно для Payment

namespace RentalSystem.Services
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public interface IPaymentStrategy
    {
        PaymentResult ProcessPayment(Payment payment);
    }
}