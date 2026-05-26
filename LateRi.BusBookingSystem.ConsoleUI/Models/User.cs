namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class User : BaseEntity
{
    public string UserId => Id;
    public string Name { get; private set; }
    public string Mobile { get; private set; }
    public string Email { get; private set; }

    private readonly List<string> _ticketIds = new();

    public IReadOnlyList<string> TicketIds => _ticketIds.AsReadOnly();

    public User(string name, string mobile, string email)
    {
        Name = name;
        Mobile = mobile;
        Email = email;
    }

    public void AddTicket(string ticketId) => _ticketIds.Add(ticketId);

    public override string GetSummary() =>
        $"[{UserId}] {Name} | {Mobile} | {Email}";

    public override string ToString() => GetSummary();
}
