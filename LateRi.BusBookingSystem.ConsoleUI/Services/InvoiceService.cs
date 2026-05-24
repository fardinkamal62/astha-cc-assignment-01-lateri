using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class InvoiceService
{
    private readonly List<Invoice> _invoices = [];

    public Invoice Create(int ticketId, int userId, decimal amount)
    {
        var invoice = new Invoice(ticketId, userId, amount);
        _invoices.Add(invoice);
        return invoice;
    }

    public List<Invoice> GetByUser(int userId) =>
        _invoices.Where(i => i.UserId == userId).ToList();

    public List<Invoice> GetUnpaidByUser(int userId) =>
        _invoices.Where(i => i.UserId == userId && !i.IsPaid).ToList();

    public Invoice? GetById(int id) =>
        _invoices.FirstOrDefault(i => i.Id == id);

    public bool Pay(int invoiceId)
    {
        var invoice = GetById(invoiceId);
        if (invoice == null || invoice.IsPaid) return false;
        invoice.MarkPaid();
        return true;
    }
}
