// C:\pj\Services\PaymentService.cs
using RentalSystem.Models;      // ← ЭТО ОБЯЗАТЕЛЬНО!
using RentalSystem.Services;    // для IPaymentStrategy

namespace RentalSystem.Services
{
    public class PaymentService
    {
        private readonly IPaymentStrategy _strategy;

        public PaymentService(IPaymentStrategy strategy)
        {
            _strategy = strategy;
        }

        public PaymentResult ExecutePayment(Payment payment)
        {
            return _strategy.ProcessPayment(payment);
        }
    }
}