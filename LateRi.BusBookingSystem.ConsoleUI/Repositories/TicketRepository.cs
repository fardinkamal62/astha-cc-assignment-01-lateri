using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;

namespace LateRi.BusBookingSystem.ConsoleUI.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly Dictionary<string, Ticket> _store = new();

    public void Add(Ticket ticket) => _store.Add(ticket.Id, ticket);

    public IReadOnlyList<Ticket> GetAll() => _store.Values.ToList();

    public Ticket? GetById(string userId) => _store.TryGetValue(userId, out var ticket) ? ticket : null;

    public IReadOnlyList<Ticket> GetByUserId(string userId) => _store.Values.Where(t => t.UserId == userId).ToList();

    public bool Remove(string id) => _store.Remove(id);
}
