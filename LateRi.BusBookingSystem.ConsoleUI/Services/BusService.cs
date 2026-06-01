using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
using LateRi.BusBookingSystem.ConsoleUI.Enums;
using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class BusService(IBusRepository iBusRepository) : IBusService
{
    public Result<Bus> Create(string coachName, BusClassification classification, int totalSeats = 0)
    {
        if (string.IsNullOrWhiteSpace(coachName))
            return Result<Bus>.Failure("CoachName is required.");

        var bus = new Bus(coachName, classification, totalSeats);
        iBusRepository.Add(bus);
        return Result<Bus>.Success("Bus created.", bus);
    }

    public IReadOnlyList<Bus> GetAll() => iBusRepository.GetAll();

    public Bus? GetById(string id) => iBusRepository.GetById(id);
}
