using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Interfaces;

public interface IPaymentProcessor
{
    string ProcessorName { get; }
    bool ProcessPayment(Invoice invoice);
}
