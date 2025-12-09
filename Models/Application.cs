using RentalSystem.States;

namespace RentalSystem.Models
{
    public class Application
    {
        public string Id { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public IState Status { get; set; } = new ApplicationStatus(ApplicationStatusValue.Pending);
        public Property Property { get; set; } = null!;
        public Tenant Tenant { get; set; } = null!;
        public Payment? Payment { get; set; }

        public bool ProcessApplication()
        {
            // Логика одобрения/отклонения
            return true;
        }
    }
}