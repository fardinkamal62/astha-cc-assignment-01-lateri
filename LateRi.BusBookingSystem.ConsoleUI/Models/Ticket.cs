namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class Ticket(string userId, string scheduleId, string busId, string seatNumber, decimal price) : BaseEntity
{
    public string TicketId => Id;
    public string UserId { get; private set; } = userId;
    public string ScheduleId { get; private set; } = scheduleId;
    public string BusId { get; private set; } = busId;
    public string SeatNumber { get; private set; } = seatNumber;
    public decimal Price { get; private set; } = price;
    public DateTimeOffset BookingDateTime { get; private set; } = DateTimeOffset.UtcNow;


    public override string GetSummary() =>
        $"[{TicketId}] Seat: {SeatNumber} | Schedule: {ScheduleId} | " +
        $"BDT {Price:F2} | Booked: {BookingDateTime:dd MMM yyyy}";

    public override string ToString() => GetSummary();
}
