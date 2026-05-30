namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public abstract class BaseEntity
{
    public string Id { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid().ToString("N")[..8].ToUpper();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    // Abstract method - forces all entities to define their display format
    public abstract string GetSummary();

    // Virtual method - can be overridden
    public virtual void Display() => Console.WriteLine(GetSummary());
}
