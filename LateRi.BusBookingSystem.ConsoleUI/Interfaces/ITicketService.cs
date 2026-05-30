using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Interfaces;

public interface ITicketService
{
    void Add(Ticket ticket);
    Ticket? GetById(string id);
    IReadOnlyList<Ticket> GetByUserId(string userId);
    bool Remove(string id);
}
