using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class UserService
{
    private readonly List<User> _users = [];

    public User Create(string name, string mobile, string email)
    {
        var user = new User(name, mobile, email);
        _users.Add(user);
        return user;
    }

    public List<User> GetAll() => [.. _users];

    public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);
}
