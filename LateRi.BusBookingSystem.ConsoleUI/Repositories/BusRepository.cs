using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Repositories;

public class BusRepository: IBusRepository
{
    private readonly Dictionary<string, Bus> _store = new();

    public void Add(Bus bus) => _store.Add(bus.Id, bus);

    public IReadOnlyList<Bus> GetAll() => _store.Values.ToList();

    public Bus GetById(string userId) => _store.TryGetValue(userId, out var bus) ? bus : null;
}
