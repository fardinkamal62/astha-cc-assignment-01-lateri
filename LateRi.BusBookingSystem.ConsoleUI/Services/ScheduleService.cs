using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class ScheduleService (IScheduleRepository scheduleRepository)
{
    public Schedule Create(string busId, string from, string to, DateTime departure, decimal price)
    {
        var schedule = new Schedule(busId, from, to, departure, price);
        scheduleRepository.Add(schedule);
        return schedule;
    }

    public IReadOnlyList<Schedule> GetAll() => scheduleRepository.GetAll();

    public Schedule? GetById(string id) => scheduleRepository.GetById(id);
}
