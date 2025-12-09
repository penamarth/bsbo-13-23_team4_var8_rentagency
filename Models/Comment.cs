namespace RentalSystem.Models
{
    public class Comment
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public User Author { get; set; } = null!;
        public Property Property { get; set; } = null!;

        public bool AddComment() => !string.IsNullOrWhiteSpace(Text);
        public bool DeleteComment() => true;
    }
}