namespace RentalSystem.States
{
    public enum ApplicationStatusValue
    {
        Pending,
        Approved,
        Rejected,
        Paid
    }

    public class ApplicationStatus : IState
    {
        public ApplicationStatusValue Value { get; }

        public ApplicationStatus(ApplicationStatusValue value) => Value = value;

        public string GetName() => Value switch
        {
            ApplicationStatusValue.Pending => "Pending",
            ApplicationStatusValue.Approved => "Approved",
            ApplicationStatusValue.Rejected => "Rejected",
            ApplicationStatusValue.Paid => "Paid",
            _ => "Unknown"
        };

        public bool IsFinal() => Value is ApplicationStatusValue.Approved or ApplicationStatusValue.Rejected or ApplicationStatusValue.Paid;
    }
}