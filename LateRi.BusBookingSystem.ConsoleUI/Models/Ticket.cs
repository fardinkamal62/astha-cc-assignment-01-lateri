namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class Ticket : BaseEntity
{
    public string TicketId => Id;
    public string UserId { get; private set; }
    public string ScheduleId { get; private set; }
    public string BusId { get; private set; }
    public string SeatNumber { get; private set; }
    public decimal Price { get; private set; }
    public DateTime BookingDateTime { get; private set; }

    public Ticket(string userId, string scheduleId, string busId, string seatNumber, decimal price)
    {
        UserId = userId;
        ScheduleId = scheduleId;
        BusId = busId;
        SeatNumber = seatNumber;
        Price = price;
        BookingDateTime = DateTime.Now;
    }

    public override string GetSummary() =>
        $"[{TicketId}] Seat: {SeatNumber} | Schedule: {ScheduleId} | " +
        $"BDT {Price:F2} | Booked: {BookingDateTime:dd MMM yyyy}";

    public override string ToString() => GetSummary();
}
