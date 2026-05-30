namespace LateRi.BusBookingSystem.ConsoleUI.Abstractions;

public class Result<T>(bool isSuccess, string message, T? data)
{
    public bool IsSuccess { get; } = isSuccess;
    public string Message { get; } = message;
    public T? Data { get; } = data;
    public bool IsFailure => !IsSuccess;

    public static Result<T> Success(string message, T data) => new(true, message, data);
    public static Result<T> Failure(string message) => new(false, message, default);
}

public class Result(bool isSuccess, string message)
{
    public bool IsSuccess { get; } = isSuccess;
    public string Message { get; } = message;
    public bool IsFailure => !IsSuccess;

    public static Result Success(string message) => new(true, message);
    public static Result Failure(string message) => new(false, message);
}
