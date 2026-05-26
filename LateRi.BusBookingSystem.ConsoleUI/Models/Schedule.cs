namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class Schedule : BaseEntity
{
    public string ScheduleId => Id;
    public string BusId { get; private set; }
    public string DepartureCity { get; private set; }
    public string ArrivalCity { get; private set; }
    public DateTime DepartureDateTime { get; private set; }
    public decimal TicketPrice { get; private set; }

    public Schedule(string busId, string departureCity, string arrivalCity,
        DateTime departureDateTime, decimal ticketPrice)
    {
        BusId = busId;
        DepartureCity = departureCity;
        ArrivalCity = arrivalCity;
        DepartureDateTime = departureDateTime;
        TicketPrice = ticketPrice;
    }

    public override string GetSummary() =>
        $"[{ScheduleId}] {DepartureCity} → {ArrivalCity} | " +
        $"{DepartureDateTime:dd MMM yyyy HH:mm} | BDT {TicketPrice:F2}";

    public override string ToString() => GetSummary();
}
