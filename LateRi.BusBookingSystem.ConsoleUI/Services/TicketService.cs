using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Services;

public class TicketService(ITicketRepository ticketRepository) : ITicketService
{
    public void Add(Ticket ticket) => ticketRepository.Add(ticket);

    public Ticket? GetById(string id) => ticketRepository.GetById(id);

    public IReadOnlyList<Ticket> GetByUserId(string userId) => ticketRepository.GetByUserId(userId);

    public bool Remove(string id) => ticketRepository.Remove(id);
}
