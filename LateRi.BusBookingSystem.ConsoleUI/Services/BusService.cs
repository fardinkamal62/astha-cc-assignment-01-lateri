using LateRi.BusBookingSystem.ConsoleUI.Enums;
using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class BusService(IBusRepository iBusRepository)
{
    public Bus Create(string coachNumber, BusClassification classification)
    {
        var bus = new Bus(coachNumber, classification);
        iBusRepository.Add(bus);
        return bus;
    }

    public IReadOnlyList<Bus> GetAll() => iBusRepository.GetAll();

    public Bus? GetById(string id) => iBusRepository.GetById(id);
}
