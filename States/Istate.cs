namespace RentalSystem.States
{
    public interface IState
    {
        string GetName();
        bool IsFinal();
    }
}