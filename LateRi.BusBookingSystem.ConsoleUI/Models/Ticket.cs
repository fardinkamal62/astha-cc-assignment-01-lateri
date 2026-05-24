namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class Ticket
{
    private static int _nextId = 1;

    public int Id { get; }
    public int UserId { get; }
    public int ScheduleId { get; }
    public int SeatNumber { get; }
    public DateTime BookedAt { get; }

    public Ticket(int userId, int scheduleId, int seatNumber)
    {
        Id = _nextId++;
        UserId = userId;
        ScheduleId = scheduleId;
        SeatNumber = seatNumber;
        BookedAt = DateTime.Now;
    }

    public override string ToString() =>
        $"[{Id}] User #{UserId} | Schedule #{ScheduleId} | Seat {SeatNumber} | {BookedAt:g}";
}
