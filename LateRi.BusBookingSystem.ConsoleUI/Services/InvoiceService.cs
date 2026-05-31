using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class InvoiceService(IInvoiceRepository invoiceRepository, IPaymentProcessor? paymentProcessor = null) : IInvoiceService
{
    private readonly IPaymentProcessor _paymentProcessor = paymentProcessor ?? new CashPaymentProcessor();

    public Invoice Create(string ticketId, string userId, decimal amount)
    {
        var invoice = new Invoice(ticketId, userId, amount);
        invoiceRepository.Add(invoice);
        return invoice;
    }

    public IReadOnlyList<Invoice> GetByUser(string userId) => invoiceRepository.GetByUserId(userId);

    public List<Invoice> GetUnpaidByUser(string userId)
    {
        var invoices = GetByUser(userId).ToList();
        return invoices.Where(i => !i.IsPaid).ToList();
    }

    public Invoice? GetById(string id) => invoiceRepository.GetById(id);

    public Result Pay(string invoiceId)
    {
        var invoice = GetById(invoiceId);
        if (invoice == null) return Result.Failure("Invoice not found.");
        if (invoice.IsPaid) return Result.Failure("Invoice is already paid.");
        var success = _paymentProcessor.ProcessPayment(invoice);
        return success
            ? Result.Success($"Payment successful via {_paymentProcessor.ProcessorName}.")
            : Result.Failure("Payment processing failed.");
    }

    public Result CancelPay(string invoiceId)
    {
        var invoice = GetById(invoiceId);
        if (invoice == null) return Result.Failure("Invoice not found.");
        if (invoice.IsPaid) return Result.Failure("Invoice is already paid.");
        invoice.MarkCancelled();
        return Result.Success("Invoice cancelled.");
    }
}
