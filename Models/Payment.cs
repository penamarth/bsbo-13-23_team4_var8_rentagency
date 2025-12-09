// C:\pj\Models\Payment.cs
using RentalSystem.States;

namespace RentalSystem.Models
{
    public class Payment
    {
        public string Id { get; set; } = string.Empty;
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public IState Status { get; set; } = new PaymentStatus(PaymentStatusValue.Success);
        public Application Application { get; set; } = null!;
    }
}