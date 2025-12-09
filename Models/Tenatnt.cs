using RentalSystem.States;
using RentalSystem.Services;

namespace RentalSystem.Models
{
    public class Tenant : User
    {
        public List<string> Preferences { get; set; } = new();

        public List<Property> SearchApartments(string criteria)
        {
            // Заглушка: в реальном проекте — запрос к БД
            return new List<Property>();
        }

        public Application? SubmitApplication(Property property)
        {
            // Проверяем, сдан ли объект
            if (property.Status is PropertyStatus propStatus && 
                propStatus.Value == PropertyStatusValue.Rented)
            {
                Console.WriteLine("Property is already rented.");
                return null;
            }

            var application = new Application
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                Status = new ApplicationStatus(ApplicationStatusValue.Pending),
                Property = property,
                Tenant = this
            };

            property.Applications.Add(application);
            return application;
        }

        public bool MakePayment(Application application, IPaymentStrategy strategy)
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                Amount = application.Property.Price,
                Date = DateTime.Now,
                Status = new PaymentStatus(PaymentStatusValue.Success), // в демо — успех
                Application = application
            };

            var service = new PaymentService(strategy);
            var result = service.ExecutePayment(payment);

            if (result.Success)
            {
                application.Status = new ApplicationStatus(ApplicationStatusValue.Paid);
                Console.WriteLine("Payment successful!");
                return true;
            }

            Console.WriteLine("Payment failed.");
            return false;
        }
    }
}