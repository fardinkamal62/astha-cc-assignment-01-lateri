using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

// SRP: Orchestrates the booking workflow — validates user/schedule/bus/seat,
//      reserves seat, creates ticket, and triggers invoice generation.
// DIP: All dependencies are injected as interfaces, not concrete types.
public class BookingService(
    ITicketService ticketService,
    IUserService userService,
    IScheduleService scheduleService,
    IBusService busService,
    IInvoiceService invoiceService)
{
    public Result<Ticket> Book(string userId, string scheduleId, string seatCode)
    {
        var user = userService.GetById(userId);
        if (user == null) return Result<Ticket>.Failure("User not found.");

        var schedule = scheduleService.GetById(scheduleId);
        if (schedule == null) return Result<Ticket>.Failure("Schedule not found.");

        var bus = busService.GetById(schedule.BusId);
        if (bus == null) return Result<Ticket>.Failure("Bus not found.");

        if (!bus.IsValidSeat(seatCode))
            return Result<Ticket>.Failure($"Invalid seat code '{seatCode}'.");

        if (!bus.IsSeatAvailable(seatCode))
            return Result<Ticket>.Failure($"Seat {seatCode} is already reserved.");

        bus.ReserveSeat(seatCode);

        var ticket = new Ticket(userId, scheduleId, bus.BusId, seatCode, schedule.TicketPrice);
        ticketService.Add(ticket);

        invoiceService.Create(ticket.TicketId, userId, ticket.Price);

        return Result<Ticket>.Success("Booking successful!", ticket);
    }

    public List<Ticket> GetByUser(string userId) => ticketService.GetByUserId(userId).ToList();

    public List<string> GetAvailableSeats(string scheduleId)
    {
        var schedule = scheduleService.GetById(scheduleId);
        if (schedule == null) return [];
        return busService.GetById(schedule.BusId)?.GetAvailableSeats() ?? [];
    }

    public Ticket? GetById(string id) => ticketService.GetById(id);

    public Result CancelBooking(string userId, string invoiceId)
    {
        var user = userService.GetById(userId);
        if (user == null) return Result.Failure("User not found.");

        var invoice = invoiceService.GetById(invoiceId);
        if (invoice == null) return Result.Failure("Invoice not found.");
        if (invoice.UserId != userId)
            return Result.Failure("Cancellation failed. Invoice does not belong to this user.");

        var cancelResult = invoiceService.CancelPay(invoiceId);
        if (cancelResult.IsFailure)
            return cancelResult;

        var schedule = scheduleService.GetById(invoice.TicketIds.Count > 0
            ? ticketService.GetById(invoice.TicketIds[0])?.ScheduleId ?? "" : "");
        if (schedule == null) return Result.Failure("Schedule not found.");

        var bus = busService.GetById(schedule.BusId);
        if (bus == null) return Result.Failure("Bus not found.");

        foreach (var ticketId in invoice.TicketIds)
        {
            var ticket = GetById(ticketId);
            if (ticket == null) continue;
            bus.ClearReservedSeat(ticket.SeatNumber);
            ticketService.Remove(ticket.TicketId);
        }

        return Result.Success("Cancellation successful!");
    }

    public Result<IReadOnlyList<Ticket>> BatchBook(string userId, string scheduleId, IReadOnlyList<string> seatCodes)
    {
        var user = userService.GetById(userId);
        if (user == null) return Result<IReadOnlyList<Ticket>>.Failure("User not found.");

        var schedule = scheduleService.GetById(scheduleId);
        if (schedule == null) return Result<IReadOnlyList<Ticket>>.Failure("Schedule not found.");

        var bus = busService.GetById(schedule.BusId);
        if (bus == null) return Result<IReadOnlyList<Ticket>>.Failure("Bus not found.");

        foreach (var seatCode in seatCodes)
        {
            if (!bus.IsValidSeat(seatCode))
                return Result<IReadOnlyList<Ticket>>.Failure($"Invalid seat code '{seatCode}'.");
            if (!bus.IsSeatAvailable(seatCode))
                return Result<IReadOnlyList<Ticket>>.Failure($"Seat {seatCode} is already reserved.");
        }

        var tickets = new List<Ticket>();
        foreach (var seatCode in seatCodes)
        {
            bus.ReserveSeat(seatCode);
            var ticket = new Ticket(userId, scheduleId, bus.BusId, seatCode, schedule.TicketPrice);
            ticketService.Add(ticket);
            tickets.Add(ticket);
        }

        var totalAmount = tickets.Sum(t => t.Price);
        invoiceService.Create(tickets.Select(t => t.TicketId).ToList(), userId, totalAmount);

        return Result<IReadOnlyList<Ticket>>.Success($"{tickets.Count} ticket(s) booked successfully!", tickets);
    }
}
