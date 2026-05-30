namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class Schedule(string busId, string departureCity, string arrivalCity,
    DateTime departureDateTime, decimal ticketPrice) : BaseEntity
{
    public string ScheduleId => Id;
    public string BusId { get; private set; } = busId;
    public string DepartureCity { get; private set; } = departureCity;
    public string ArrivalCity { get; private set; } = arrivalCity;
    public DateTime DepartureDateTime { get; private set; } = departureDateTime;
    public decimal TicketPrice { get; private set; } = ticketPrice;

    public override string GetSummary() =>
        $"[{ScheduleId}] {DepartureCity} → {ArrivalCity} | " +
        $"{DepartureDateTime:dd MMM yyyy HH:mm} | BDT {TicketPrice:F2}";

    public override string ToString() => GetSummary();
}
