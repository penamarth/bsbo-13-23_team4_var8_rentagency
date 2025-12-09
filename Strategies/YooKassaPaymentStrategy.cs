using RentalSystem.Models;
using RentalSystem.Services;

namespace RentalSystem.Strategies
{
    public class YooKassaPaymentStrategy : IPaymentStrategy
    {
        public PaymentResult ProcessPayment(Payment payment)
        {
            // Имитация вызова YooKassa (ЮKassa) API
            Console.WriteLine($"[YooKassa] Обработка платежа на сумму {payment.Amount:F2} ₽ для заявки {payment.Application.Id}");
            
            // В реальном проекте: вызов YooKassa через HTTP-запрос к https://api.yookassa.ru/v3/payments
            // С использованием shopId и secretKey

            return new PaymentResult
            {
                Success = true,
                TransactionId = $"yookassa_{DateTime.Now:yyMMddHHmmss}_{new Random().Next(1000, 9999)}",
                Message = "Платёж принят ЮKassa"
            };
        }
    }
}