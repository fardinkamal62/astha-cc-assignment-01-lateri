using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class ScheduleService
{
    private readonly List<Schedule> _schedules = [];

    public Schedule Create(int busId, string from, string to, DateTime departure, decimal price)
    {
        var schedule = new Schedule(busId, from, to, departure, price);
        _schedules.Add(schedule);
        return schedule;
    }

    public List<Schedule> GetAll() => [.. _schedules];

    public Schedule? GetById(int id) => _schedules.FirstOrDefault(s => s.Id == id);
}
