namespace RentalSystem.States
{
    public enum PaymentStatusValue
    {
        Success,
        Failed
    }

    public class PaymentStatus : IState
    {
        public PaymentStatusValue Value { get; }

        public PaymentStatus(PaymentStatusValue value) => Value = value;

        public string GetName() => Value switch
        {
            PaymentStatusValue.Success => "Success",
            PaymentStatusValue.Failed => "Failed",
            _ => "Unknown"
        };

        public bool IsFinal() => true; // Оплата всегда финальное состояние
    }
}