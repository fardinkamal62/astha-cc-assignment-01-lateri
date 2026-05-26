using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class BookingService
{
    private readonly List<Ticket> _tickets = [];

    public Ticket Book(string userId, string scheduleId, string seatNumber, int maxSeats, string busId, decimal price)
    {
        if (string.IsNullOrWhiteSpace(seatNumber))
            throw new ArgumentException("Seat number is required.", nameof(seatNumber));

        if (!int.TryParse(seatNumber, out var seatIndex) || seatIndex < 1 || seatIndex > maxSeats)
            throw new ArgumentOutOfRangeException(nameof(seatNumber), $"Seat number must be between 1 and {maxSeats}.");

        seatNumber = seatIndex.ToString();

        if (_tickets.Any(t => t.ScheduleId == scheduleId && t.SeatNumber == seatNumber))
            throw new InvalidOperationException($"Seat {seatNumber} is already booked for this schedule.");

        var ticket = new Ticket(userId, scheduleId, busId, seatNumber, price);
        _tickets.Add(ticket);
        return ticket;
    }

    public List<Ticket> GetByUser(string userId) =>
        _tickets.Where(t => t.UserId == userId).ToList();

    public List<string> GetTakenSeats(string scheduleId) =>
        _tickets.Where(t => t.ScheduleId == scheduleId).Select(t => t.SeatNumber).ToList();

    public List<string> GetAvailableSeats(string scheduleId, int maxSeats)
    {
        var taken = GetTakenSeats(scheduleId);
        return Enumerable.Range(1, maxSeats)
            .Select(s => s.ToString())
            .Where(s => !taken.Contains(s))
            .ToList();
    }

    public Ticket? GetById(string id) => _tickets.FirstOrDefault(t => t.Id == id);
}
