using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class BookingService
{
    private readonly List<Ticket> _tickets = [];
    private readonly UserService _userService;
    private readonly BusService _busService;
    private readonly ScheduleService _scheduleService;
    private readonly InvoiceService _invoiceService;

    public BookingService(UserService userService, BusService busService,
        ScheduleService scheduleService, InvoiceService invoiceService)
    {
        _userService = userService;
        _busService = busService;
        _scheduleService = scheduleService;
        _invoiceService = invoiceService;
    }

    public (bool Success, string Message, Ticket? Ticket) Book(
        string userId, string scheduleId, int seatNumber)
    {
        var user = _userService.GetById(userId);
        if (user == null) return (false, "User not found.", null);

        var schedule = _scheduleService.GetById(scheduleId);
        if (schedule == null) return (false, "Schedule not found.", null);

        var bus = _busService.GetById(schedule.BusId);
        if (bus == null) return (false, "Bus not found.", null);

        if (seatNumber < 1 || seatNumber > bus.TotalSeats)
            return (false, $"Invalid seat. Must be between 1 and {bus.TotalSeats}.", null);

        var seatCode = $"S{seatNumber:D2}";

        if (!bus.IsSeatAvailable(seatCode))
            return (false, $"Seat {seatCode} is already reserved.", null);

        bus.ReserveSeat(seatCode);

        var ticket = new Ticket(userId, scheduleId, bus.BusId, seatCode, schedule.TicketPrice);
        _tickets.Add(ticket);
        user.AddTicket(ticket.TicketId);

        _invoiceService.Create(ticket.TicketId, userId, ticket.Price);

        return (true, "Booking successful!", ticket);
    }

    public List<Ticket> GetByUser(string userId) =>
        _tickets.Where(t => t.UserId == userId).ToList();

    public List<string> GetAvailableSeats(string scheduleId)
    {
        var schedule = _scheduleService.GetById(scheduleId);
        if (schedule == null) return [];
        var bus = _busService.GetById(schedule.BusId);
        return bus?.GetAvailableSeats() ?? [];
    }

    public Ticket? GetById(string id) => _tickets.FirstOrDefault(t => t.Id == id);
}
