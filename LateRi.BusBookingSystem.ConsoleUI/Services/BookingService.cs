using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class BookingService
{
    private readonly List<Ticket> _tickets = [];

    public Ticket Book(int userId, int scheduleId, int seatNumber, int maxSeats)
    {
        if (seatNumber < 1 || seatNumber > maxSeats)
            throw new ArgumentException($"Seat must be between 1 and {maxSeats}.");

        if (_tickets.Any(t => t.ScheduleId == scheduleId && t.SeatNumber == seatNumber))
            throw new InvalidOperationException($"Seat {seatNumber} is already booked for this schedule.");

        var ticket = new Ticket(userId, scheduleId, seatNumber);
        _tickets.Add(ticket);
        return ticket;
    }

    public List<Ticket> GetByUser(int userId) =>
        _tickets.Where(t => t.UserId == userId).ToList();

    public List<int> GetTakenSeats(int scheduleId) =>
        _tickets.Where(t => t.ScheduleId == scheduleId).Select(t => t.SeatNumber).ToList();

    public List<int> GetAvailableSeats(int scheduleId, int maxSeats)
    {
        var taken = GetTakenSeats(scheduleId);
        return Enumerable.Range(1, maxSeats).Where(s => !taken.Contains(s)).ToList();
    }

    public Ticket? GetById(int id) => _tickets.FirstOrDefault(t => t.Id == id);
}
