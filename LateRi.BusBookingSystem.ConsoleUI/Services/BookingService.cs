using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class BookingService(
    IBusRepository busRepo,
    IScheduleRepository scheduleRepo,
    ITicketRepository ticketRepo,
    IUserRepository userRepo,
    InvoiceService invoiceService)
{

    public (bool Success, string Message, Ticket? Ticket) Book(
        string userId, string scheduleId, int seatNumber)
    {
        var user = userRepo.GetById(userId);
        if (user == null) return (false, "User not found.", null);

        var schedule = scheduleRepo.GetById(scheduleId);
        if (schedule == null) return (false, "Schedule not found.", null);

        var bus = busRepo.GetById(schedule.BusId);
        if (bus == null) return (false, "Bus not found.", null);

        if (seatNumber < 1 || seatNumber > bus.TotalSeats)
            return (false, $"Invalid seat. Must be between 1 and {bus.TotalSeats}.", null);

        var seatCode = $"S{seatNumber:D2}";

        if (!bus.IsSeatAvailable(seatCode))
            return (false, $"Seat {seatCode} is already reserved.", null);

        bus.ReserveSeat(seatCode);

        var ticket = new Ticket(userId, scheduleId, bus.BusId, seatCode, schedule.TicketPrice);
        ticketRepo.Add(ticket);
        user.AddTicket(ticket.TicketId);

        invoiceService.Create(ticket.TicketId, userId, ticket.Price);

        return (true, "Booking successful!", ticket);
    }

    public List<Ticket> GetByUser(string userId) => ticketRepo.GetByUserId(userId).ToList();

    public List<string> GetAvailableSeats(string scheduleId)
    {
        var schedule = scheduleRepo.GetById(scheduleId);
        if (schedule == null) return [];
        return busRepo.GetById(schedule.BusId)?.GetAvailableSeats() ?? [];
    }

    public Ticket? GetById(string id) => ticketRepo.GetById(id);
}
