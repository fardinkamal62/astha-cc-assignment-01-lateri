using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Interfaces;

public interface IUserService
{
    Result<User> Create(string name, string mobile, string email);
    IReadOnlyList<User> GetAll();
    User? GetById(string id);
}
