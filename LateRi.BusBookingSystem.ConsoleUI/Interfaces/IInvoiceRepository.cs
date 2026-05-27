using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Interfaces;

public interface IInvoiceRepository: IRepository<Invoice>
{
    IReadOnlyList<Invoice> GetByUserId(string userId);
}
