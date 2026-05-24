namespace LateRi.BusBookingSystem.ConsoleUI.Models;

public class User
{
    private static int _nextId = 1;

    public int Id { get; }
    public string Name { get; }
    public string Mobile { get; }
    public string Email { get; }

    public User(string name, string mobile, string email)
    {
        Id = _nextId++;
        Name = name;
        Mobile = mobile;
        Email = email;
    }

    public override string ToString() =>
        $"[{Id}] {Name} | {Mobile} | {Email}";
}
