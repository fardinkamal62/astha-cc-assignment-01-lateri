using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

// OCP: New payment processors (Bkash, Nagad, etc.) can be added
// without modifying existing code. IPaymentProcessor is the open
// extension point; this class is a closed implementation.
public class CashPaymentProcessor : IPaymentProcessor
{
    public string ProcessorName => "Cash";

    // SRP: Only responsibility is processing cash payments.
    // STRATEGY PATTERN: Concrete strategy implementing IPaymentProcessor.
    public bool ProcessPayment(Invoice invoice)
    {
        invoice.MarkAsPaid();
        return true;
    }
}
