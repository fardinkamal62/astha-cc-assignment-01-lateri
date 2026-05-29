using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class UserService(IUserRepository userRepository)
{
    public User Create(string name, string mobile, string email)
    {
        var user = new User(name, mobile, email);
        userRepository.Add(user);
        return user;
    }

    public IReadOnlyList<User> GetAll() => userRepository.GetAll();

    public User? GetById(string id) => userRepository.GetById(id);
}
