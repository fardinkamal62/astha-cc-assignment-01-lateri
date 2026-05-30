using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class ScheduleService(IScheduleRepository scheduleRepository)
{
    public Result<Schedule> Create(string busId, string from, string to, DateTime departure, decimal price)
    {
        if (string.IsNullOrWhiteSpace(busId))
            return Result<Schedule>.Failure("BusId is required.");

        if (string.IsNullOrWhiteSpace(from))
            return Result<Schedule>.Failure("From is required.");

        if (string.IsNullOrWhiteSpace(to))
            return Result<Schedule>.Failure("To is required.");

        if (price <= 0)
            return Result<Schedule>.Failure("Price cannot be negative.");

        var schedule = new Schedule(busId, from, to, departure, price);
        scheduleRepository.Add(schedule);
        return Result<Schedule>.Success("Schedule created.", schedule);
    }

    public IReadOnlyList<Schedule> GetAll() => scheduleRepository.GetAll();

    public Schedule? GetById(string id) => scheduleRepository.GetById(id);
}
