namespace LateRi.BusBookingSystem.ConsoleUI.Models;

// ABSTRACTION: Abstract base class for all domain entities.
// Provides common Id generation, CreatedAt timestamp, and polymorphic behaviour.
// INHERITANCE: All domain models (User, Bus, Schedule, Ticket, Invoice) extend this.
public abstract class BaseEntity
{
    public string Id { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid().ToString("N")[..8].ToUpper();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    // ABSTRACTION: Forces every entity to define its own display format.
    public abstract string GetSummary();
}
