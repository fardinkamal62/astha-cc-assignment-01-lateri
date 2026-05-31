using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class CashPaymentProcessor : IPaymentProcessor
{
    public string ProcessorName => "Cash";

    public bool ProcessPayment(Invoice invoice)
    {
        invoice.MarkPaid();
        return true;
    }
}
