namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class User(string name, string mobile, string email) : BaseEntity
{
    public string UserId => Id;
    public string Name { get; } = name;
    public string Mobile { get; } = mobile;
    public string Email { get; } = email;

    public override string GetSummary() =>
        $"[{UserId}] {Name} | {Mobile} | {Email}";

    public override string ToString() => GetSummary();
}
