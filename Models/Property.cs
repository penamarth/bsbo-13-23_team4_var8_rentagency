using RentalSystem.States;

namespace RentalSystem.Models
{
    public class Property
    {
        public string Id { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Price { get; set; }
        public double Area { get; set; }
        public string Description { get; set; } = string.Empty;
        public PropertyType Type { get; set; }
        public IState Status { get; set; } = new PropertyStatus(PropertyStatusValue.UnderModeration);
        public List<Application> Applications { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public Owner? Owner { get; set; }

        public string GetDetails() =>
            $"{Type}: {Address}, {Area} m², {Price:C}";
    }
}