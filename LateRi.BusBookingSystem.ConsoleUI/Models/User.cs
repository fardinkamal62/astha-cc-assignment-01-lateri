namespace LateRi.BusBookingSystem.ConsoleUI.Models;

// INHERITANCE: User extends BaseEntity, inheriting Id and CreatedAt.
// POLYMORPHISM: Overrides GetSummary() to provide User-specific display.
public class User(string name, string mobile, string email) : BaseEntity
{
    public string UserId => Id;
    public string Name { get; } = name;
    public string Mobile { get; } = mobile;
    public string Email { get; } = email;

    // POLYMORPHISM: Override of abstract GetSummary in BaseEntity.
    public override string GetSummary() =>
        $"[{UserId}] {Name} | {Mobile} | {Email}";

    public override string ToString() => GetSummary();
}
