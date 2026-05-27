using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly Dictionary<string, Invoice> _store = new();

    public void Add(Invoice invoice) => _store.Add(invoice.Id, invoice);

    public IReadOnlyList<Invoice> GetAll() => _store.Values.ToList();

    public Invoice GetById(string userId) => _store.TryGetValue(userId, out var invoice) ? invoice : null;

    public IReadOnlyList<Invoice> GetByUserId(string userId) => _store.Values.Where(i => i.UserId == userId).ToList();
}
