using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
using LateRi.BusBookingSystem.ConsoleUI.Enums;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Interfaces;

public interface IBusService
{
    Result<Bus> Create(string coachNumber, BusClassification classification);
    IReadOnlyList<Bus> GetAll();
    Bus? GetById(string id);
}
