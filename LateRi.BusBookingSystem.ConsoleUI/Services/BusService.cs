using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
using LateRi.BusBookingSystem.ConsoleUI.Enums;
using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class BusService(IBusRepository iBusRepository) : IBusService
{
    public Result<Bus> Create(string coachNumber, BusClassification classification)
    {
        if (string.IsNullOrWhiteSpace(coachNumber))
            return Result<Bus>.Failure("CoachNumber is required.");

        var bus = new Bus(coachNumber, classification);
        iBusRepository.Add(bus);
        return Result<Bus>.Success("Bus created.", bus);
    }

    public IReadOnlyList<Bus> GetAll() => iBusRepository.GetAll();

    public Bus? GetById(string id) => iBusRepository.GetById(id);
}
