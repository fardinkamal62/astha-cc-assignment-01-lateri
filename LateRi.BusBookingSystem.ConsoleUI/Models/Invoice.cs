namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class Invoice
{
    private static int _nextId = 1;

    public int Id { get; }
    public int TicketId { get; }
    public int UserId { get; }
    public decimal Amount { get; }
    public DateTime IssuedAt { get; }
    public bool IsPaid { get; private set; }

    public Invoice(int ticketId, int userId, decimal amount)
    {
        Id = _nextId++;
        TicketId = ticketId;
        UserId = userId;
        Amount = amount;
        IssuedAt = DateTime.Now;
    }

    public void MarkPaid() => IsPaid = true;

    public override string ToString() =>
        $"[{Id}] User #{UserId} | Ticket #{TicketId} | ${Amount:F2} | {(IsPaid ? "PAID" : "UNPAID")} | {IssuedAt:d}";
}
