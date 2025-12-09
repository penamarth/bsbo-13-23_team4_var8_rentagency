namespace RentalSystem.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        public virtual bool Login() => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        public virtual void Logout() => Console.WriteLine($"{Name} logged out.");
    }
}