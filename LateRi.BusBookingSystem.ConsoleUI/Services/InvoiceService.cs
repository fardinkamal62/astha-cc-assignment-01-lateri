using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class InvoiceService
{
    private readonly List<Invoice> _invoices = [];

    public Invoice Create(string ticketId, string userId, decimal amount)
    {
        var invoice = new Invoice(ticketId, userId, amount);
        _invoices.Add(invoice);
        return invoice;
    }

    public List<Invoice> GetByUser(string userId) =>
        _invoices.Where(i => i.UserId == userId).ToList();

    public List<Invoice> GetUnpaidByUser(string userId) =>
        _invoices.Where(i => i.UserId == userId && !i.IsPaid).ToList();

    public Invoice? GetById(string id) =>
        _invoices.FirstOrDefault(i => i.Id == id);

    public bool Pay(string invoiceId)
    {
        var invoice = GetById(invoiceId);
        if (invoice == null || invoice.IsPaid) return false;
        invoice.MarkPaid();
        return true;
    }

    public bool CancelPay(string invoiceId)
    {
        var invoice = GetById(invoiceId);
        if (invoice == null || invoice.IsPaid) return false;
        invoice.MarkCancelled();
        return true;
    }
}
