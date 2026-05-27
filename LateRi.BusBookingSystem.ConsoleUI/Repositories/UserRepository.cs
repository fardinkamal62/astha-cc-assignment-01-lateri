using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Repositories;

public class UserRepository: IUserRepository
{
    private readonly Dictionary<string, User> _store = new();

    public void Add(User user) => _store.Add(user.Id, user);

    public IReadOnlyList<User> GetAll() => _store.Values.ToList();

    public User GetById(string userId) => _store.TryGetValue(userId, out var user) ? user : null;
}
