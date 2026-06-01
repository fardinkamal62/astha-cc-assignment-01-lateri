using LateRi.BusBookingSystem.ConsoleUI.Enums;

namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class Invoice(IReadOnlyList<string> ticketIds, string userId, decimal amountDue) : BaseEntity
{
    public string InvoiceId => Id;
    public IReadOnlyList<string> TicketIds { get; private set; } = ticketIds;
    public string PrimaryTicketId => TicketIds.FirstOrDefault() ?? string.Empty;
    public string UserId { get; private set; } = userId;
    public decimal AmountDue { get; private set; } = amountDue;
    public DateTimeOffset GeneratedDate { get; private set; } = DateTimeOffset.UtcNow;
    public PaymentStatus Status { get; private set; } = PaymentStatus.Unpaid;

    public bool IsPaid => Status == PaymentStatus.Paid;

    public void MarkAsPaid() => Status = PaymentStatus.Paid;
    public void MarkAsCancelled() => Status = PaymentStatus.Unpaid;

    public override string GetSummary() =>
        $"[{InvoiceId}] Tickets: {string.Join(", ", TicketIds)} | BDT {AmountDue:F2} | " +
        $"Status: {Status} | Date: {GeneratedDate:dd MMM yyyy}";

    public override string ToString() => GetSummary();
}
