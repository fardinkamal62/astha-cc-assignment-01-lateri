using LateRi.BusBookingSystem.ConsoleUI.Enums;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class BusService
{
    private readonly List<Bus> _buses = [];

    public Bus Create(string coachNumber, BusClassification classification)
    {
        var bus = new Bus(coachNumber, classification);
        _buses.Add(bus);
        return bus;
    }

    public List<Bus> GetAll() => [.. _buses];

    public Bus? GetById(string id) => _buses.FirstOrDefault(b => b.Id == id);
}
