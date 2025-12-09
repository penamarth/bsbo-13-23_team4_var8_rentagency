// Models/Owner.cs
using RentalSystem.States; // ← ДОБАВЬ ЭТУ СТРОКУ

namespace RentalSystem.Models
{
    public class Owner : User
    {
        public double Rating { get; set; } = 5.0;

        public Property AddProperty(string address, double price, PropertyType type)
        {
            var property = new Property
            {
                Id = Guid.NewGuid().ToString(),
                Address = address,
                Price = price,
                Type = type,
                Status = new PropertyStatus(PropertyStatusValue.Available) // ← теперь видно
            };

            Properties.Add(property);
            return property;
        }

        public List<Property> Properties { get; set; } = new();
        // ❌ УБЕРИ ЭТО: public List<Comment> Comments { get; set; } = new();
        // (у Owner есть Properties, но Comments — у Property и User)
    }
}