using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Interfaces;

public interface IScheduleService
{
    Result<Schedule> Create(string busId, string from, string to, DateTime departure, decimal price);
    IReadOnlyList<Schedule> GetAll();
    Schedule? GetById(string id);
}
