namespace LateRi.BusBookingSystem.ConsoleUI.Interfaces;

public interface IRepository<T>
{
    void Add(T entity);
    T? GetById(string id);
    IReadOnlyList<T> GetAll();
    bool Remove(string id);
}
