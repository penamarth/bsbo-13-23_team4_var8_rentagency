namespace RentalSystem.Models
{
    public class UserActionLog
    {
        public string Id { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public int AdminId { get; set; }
        public int TargetUserId { get; set; }
        public DateTime Date { get; set; }

        public UserActionLog(string action, string reason, int adminId, int targetUserId)
        {
            Id = Guid.NewGuid().ToString();
            Action = action;
            Reason = reason;
            AdminId = adminId;
            TargetUserId = targetUserId;
            Date = DateTime.Now;
        }

        public string GetDetails() => 
            $"[{Date:yyyy-MM-dd HH:mm}] Admin#{AdminId} -> User#{TargetUserId}: {Action} (Reason: {Reason})";
    }
}