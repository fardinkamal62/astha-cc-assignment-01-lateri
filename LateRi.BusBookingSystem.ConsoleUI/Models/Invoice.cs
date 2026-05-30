using LateRi.BusBookingSystem.ConsoleUI.Enums;

namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class Invoice(string ticketId, string userId, decimal amountDue) : BaseEntity
{
    public string InvoiceId => Id;
    public string TicketId { get; private set; } = ticketId;
    public string UserId { get; private set; } = userId;
    public decimal AmountDue { get; private set; } = amountDue;
    public DateTimeOffset GeneratedDate { get; private set; } = DateTimeOffset.UtcNow;
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;

    public bool IsPaid => Status == PaymentStatus.Paid;

    private void MarkAsPaid() => Status = PaymentStatus.Paid;
    private void MarkAsCancelled() => Status = PaymentStatus.Cancelled;

    public void MarkPaid() => MarkAsPaid();
    public void MarkCancelled() => MarkAsCancelled();

    public override string GetSummary() =>
        $"[{InvoiceId}] Ticket: {TicketId} | BDT {AmountDue:F2} | " +
        $"Status: {Status} | Date: {GeneratedDate:dd MMM yyyy}";

    public override string ToString() => GetSummary();
}
