namespace RentalSystem.States
{
    public enum PropertyStatusValue
    {
        Available,
        Rented,
        UnderModeration
    }

    public class PropertyStatus : IState
    {
        public PropertyStatusValue Value { get; }

        public PropertyStatus(PropertyStatusValue value) => Value = value;

        public string GetName() => Value switch
        {
            PropertyStatusValue.Available => "Available",
            PropertyStatusValue.Rented => "Rented",
            PropertyStatusValue.UnderModeration => "Under Moderation",
            _ => "Unknown"
        };

        public bool IsFinal() => Value == PropertyStatusValue.Rented;
    }
}