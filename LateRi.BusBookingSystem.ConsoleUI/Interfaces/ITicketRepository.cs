using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Interfaces;

public interface ITicketRepository : IRepository<Ticket>
{
    IReadOnlyList<Ticket> GetByUserId(string userId);
    bool IsSeatBooked(string scheduleId, string seatNumber);
}
