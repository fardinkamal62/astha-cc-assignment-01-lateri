using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;
using System.Text.RegularExpressions;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public Result<User> Create(string name, string mobile, string email)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 3)
            return Result<User>.Failure("Name is required and should be at least 3 characters long.");

        if (!IsValidEmail(email))
            return Result<User>.Failure("Invalid email format.");

        if (!IsValidMobile(mobile))
            return Result<User>.Failure("Invalid mobile number. It should be 11 digits (e.g., 01711223344).");

        var user = new User(name, mobile, email);
        userRepository.Add(user);
        return Result<User>.Success("User created.", user);
    }

    public IReadOnlyList<User> GetAll() => userRepository.GetAll();

    public User? GetById(string id) => userRepository.GetById(id);

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegex);
    }

    private bool IsValidMobile(string mobile)
    {
        if (string.IsNullOrWhiteSpace(mobile)) return false;
        var mobileRegex = @"^01[3-9]\d{8}$";
        return Regex.IsMatch(mobile, mobileRegex);
    }
}
