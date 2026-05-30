using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
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
    public Result<Ticket> Book(string userId, string scheduleId, int seatNumber)
    {
        var user = userRepo.GetById(userId);
        if (user == null) return Result<Ticket>.Failure("User not found.");

        var schedule = scheduleRepo.GetById(scheduleId);
        if (schedule == null) return Result<Ticket>.Failure("Schedule not found.");

        var bus = busRepo.GetById(schedule.BusId);
        if (bus == null) return Result<Ticket>.Failure("Bus not found.");

        if (seatNumber < 1 || seatNumber > bus.TotalSeats)
            return Result<Ticket>.Failure($"Invalid seat. Must be between 1 and {bus.TotalSeats}.");

        var seatCode = $"S{seatNumber:D2}";

        if (!bus.IsSeatAvailable(seatCode))
            return Result<Ticket>.Failure($"Seat {seatCode} is already reserved.");

        bus.ReserveSeat(seatCode);

        var ticket = new Ticket(userId, scheduleId, bus.BusId, seatCode, schedule.TicketPrice);
        ticketRepo.Add(ticket);

        invoiceService.Create(ticket.TicketId, userId, ticket.Price);

        return Result<Ticket>.Success("Booking successful!", ticket);
    }

    public List<Ticket> GetByUser(string userId) => ticketRepo.GetByUserId(userId).ToList();

    public List<string> GetAvailableSeats(string scheduleId)
    {
        var schedule = scheduleRepo.GetById(scheduleId);
        if (schedule == null) return [];
        return busRepo.GetById(schedule.BusId)?.GetAvailableSeats() ?? [];
    }

    public Ticket? GetById(string id) => ticketRepo.GetById(id);

    public Result CancelBooking(string userId, string invoiceId)
    {
        var user = userRepo.GetById(userId);
        if (user == null) return Result.Failure("User not found.");

        var invoice = invoiceService.GetById(invoiceId);
        if (invoice == null) return Result.Failure("Invoice not found.");
        if (invoice.UserId != userId)
            return Result.Failure("Cancellation failed. Invoice does not belong to this user.");

        var ticket = GetById(invoice.TicketId);
        if (ticket == null) return Result.Failure("Ticket not found.");
        if (ticket.UserId != userId)
            return Result.Failure("Cancellation failed. Ticket does not belong to this user.");

        var cancelResult = invoiceService.CancelPay(invoiceId);
        if (cancelResult.IsFailure)
            return cancelResult;

        var schedule = scheduleRepo.GetById(ticket.ScheduleId);
        if (schedule == null) return Result.Failure("Schedule not found.");

        var bus = busRepo.GetById(schedule.BusId);
        if (bus == null) return Result.Failure("Bus not found.");

        var seatFreed = bus.ClearReservedSeat(ticket.SeatNumber);
        if (!seatFreed)
            return Result.Failure($"Failed to clear reserved seat {ticket.SeatNumber}. It may not be reserved.");

        ticketRepo.Remove(ticket.TicketId);

        return Result.Success("Cancellation successful!");
    }
}
