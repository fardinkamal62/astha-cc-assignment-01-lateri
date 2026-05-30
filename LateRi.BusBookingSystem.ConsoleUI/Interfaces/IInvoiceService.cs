using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Interfaces;

public interface IInvoiceService
{
    Invoice Create(string ticketId, string userId, decimal amount);
    IReadOnlyList<Invoice> GetByUser(string userId);
    List<Invoice> GetUnpaidByUser(string userId);
    Invoice? GetById(string id);
    Result Pay(string invoiceId);
    Result CancelPay(string invoiceId);
}
