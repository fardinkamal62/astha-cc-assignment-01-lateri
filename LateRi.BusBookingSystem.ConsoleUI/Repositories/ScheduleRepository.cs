using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly Dictionary<string, Schedule> _store = new();

    public void Add(Schedule schedule) => _store.Add(schedule.Id, schedule);

    public IReadOnlyList<Schedule> GetAll() => _store.Values.ToList();

    public Schedule? GetById(string userId) => _store.TryGetValue(userId, out var schedule) ? schedule : null;

    public IReadOnlyList<Schedule> GetByBusId(string busId) => _store.Values.Where(s => s.BusId == busId).ToList();

    public bool Remove(string id) => _store.Remove(id);
}
