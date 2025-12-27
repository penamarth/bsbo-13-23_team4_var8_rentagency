namespace RentalSystem.States
{
    public class UserStatus : IState
    {
        public UserStatusValue Value { get; }

        public UserStatus(UserStatusValue value) => Value = value;

        public string GetName() => Value switch
        {
            UserStatusValue.Active => "Active",
            UserStatusValue.Blocked => "Blocked",
            UserStatusValue.Deleted => "Deleted",
            _ => "Unknown"
        };

        public bool IsFinal() => Value == UserStatusValue.Deleted;

        public bool CanTransitionTo(UserStatusValue targetState) => Value switch
        {
            UserStatusValue.Active => targetState == UserStatusValue.Blocked || targetState == UserStatusValue.Deleted,
            UserStatusValue.Blocked => targetState == UserStatusValue.Active || targetState == UserStatusValue.Deleted,
            UserStatusValue.Deleted => false, // Deleted is final state
            _ => false
        };
    }

    // Конкретные состояния для создания через фабрику
    public class UserActive : UserStatus
    {
        public UserActive() : base(UserStatusValue.Active) { }
    }

    public class UserBlocked : UserStatus
    {
        public UserBlocked() : base(UserStatusValue.Blocked) { }
    }

    public class UserDeleted : UserStatus
    {
        public UserDeleted() : base(UserStatusValue.Deleted) { }
    }
}