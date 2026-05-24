using LateRi.BusBookingSystem.ConsoleUI.Models;
using LateRi.BusBookingSystem.ConsoleUI.Services;

var userService = new UserService();
var busService = new BusService();
var scheduleService = new ScheduleService();
var bookingService = new BookingService();
var invoiceService = new InvoiceService();

while (true)
{
    Console.Clear();
    Console.WriteLine("=== BUS TICKET BOOKING SYSTEM ===");
    Console.WriteLine();
    Console.WriteLine("1. Create User");
    Console.WriteLine("2. Display All Users");
    Console.WriteLine("3. Create Bus");
    Console.WriteLine("4. Display All Buses");
    Console.WriteLine("5. Create Schedule");
    Console.WriteLine("6. Display All Schedules");
    Console.WriteLine("7. Display Schedule Details");
    Console.WriteLine("8. Book Ticket");
    Console.WriteLine("9. Display User Invoices");
    Console.WriteLine("10. Process Invoice Payment");
    Console.WriteLine("11. Display User Tickets");
    Console.WriteLine("0. Exit");
    Console.WriteLine();
    Console.Write("Choose an option: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1": CreateUser(); break;
        case "2": DisplayAllUsers(); break;
        case "3": CreateBus(); break;
        case "4": DisplayAllBuses(); break;
        case "5": CreateSchedule(); break;
        case "6": DisplayAllSchedules(); break;
        case "7": DisplayScheduleDetails(); break;
        case "8": BookTicket(); break;
        case "9": DisplayUserInvoices(); break;
        case "10": ProcessInvoicePayment(); break;
        case "11": DisplayUserTickets(); break;
        case "0": return;
        default:
            Console.WriteLine("Invalid option. Press any key to try again.");
            Console.ReadKey();
            break;
    }
}

// ============================================================
// 1. Create User
// ============================================================
void CreateUser()
{
    Console.Clear();
    Console.WriteLine("--- CREATE USER ---");
    Console.Write("Name : ");
    var name = Console.ReadLine()!;
    Console.Write("Mobile : ");
    var mobile = Console.ReadLine()!;
    Console.Write("Email : ");
    var email = Console.ReadLine()!;

    var user = userService.Create(name, mobile, email);
    Console.WriteLine($"User created: {user}");
    Pause();
}

// ============================================================
// 2. Display All Users
// ============================================================
void DisplayAllUsers()
{
    Console.Clear();
    Console.WriteLine("--- ALL USERS ---");
    var users = userService.GetAll();
    if (users.Count == 0)
        Console.WriteLine("No users registered.");
    else
        users.ForEach(u => Console.WriteLine(u));
    Pause();
}

// ============================================================
// 3. Create Bus
// ============================================================
void CreateBus()
{
    Console.Clear();
    Console.WriteLine("--- CREATE BUS ---");
    Console.Write("Coach Number : ");
    var coach = Console.ReadLine()!;
    Console.Write("Classification (1 = Business, 2 = Economy) : ");
    var clsInput = Console.ReadLine();
    var classification = clsInput == "1" ? BusClassification.Business : BusClassification.Economy;

    var bus = busService.Create(coach, classification);
    Console.WriteLine($"Bus created: {bus}");
    Pause();
}

// ============================================================
// 4. Display All Buses
// ============================================================
void DisplayAllBuses()
{
    Console.Clear();
    Console.WriteLine("--- ALL BUSES ---");
    var buses = busService.GetAll();
    if (buses.Count == 0)
        Console.WriteLine("No buses in fleet.");
    else
        buses.ForEach(b => Console.WriteLine(b));
    Pause();
}

// ============================================================
// 5. Create Schedule
// ============================================================
void CreateSchedule()
{
    Console.Clear();
    Console.WriteLine("--- CREATE SCHEDULE ---");

    var buses = busService.GetAll();
    if (buses.Count == 0)
    {
        Console.WriteLine("No buses available. Create a bus first.");
        Pause();
        return;
    }

    Console.WriteLine("Available buses:");
    buses.ForEach(b => Console.WriteLine($"  {b}"));
    Console.Write("Bus ID : ");
    if (!int.TryParse(Console.ReadLine(), out var busId) || busService.GetById(busId) == null)
    {
        Console.WriteLine("Invalid Bus ID.");
        Pause();
        return;
    }

    Console.Write("Departure City : ");
    var from = Console.ReadLine()!;
    Console.Write("Arrival City : ");
    var to = Console.ReadLine()!;
    Console.Write("Departure (yyyy-MM-dd HH:mm) : ");
    if (!DateTime.TryParse(Console.ReadLine(), out var departure))
    {
        Console.WriteLine("Invalid date/time.");
        Pause();
        return;
    }
    Console.Write("Ticket Price : ");
    if (!decimal.TryParse(Console.ReadLine(), out var price))
    {
        Console.WriteLine("Invalid price.");
        Pause();
        return;
    }

    var schedule = scheduleService.Create(busId, from, to, departure, price);
    Console.WriteLine($"Schedule created: {schedule}");
    Pause();
}

// ============================================================
// 6. Display All Schedules
// ============================================================
void DisplayAllSchedules()
{
    Console.Clear();
    Console.WriteLine("--- ALL SCHEDULES ---");
    var schedules = scheduleService.GetAll();
    if (schedules.Count == 0)
        Console.WriteLine("No schedules.");
    else
        schedules.ForEach(s => Console.WriteLine(s));
    Pause();
}

// ============================================================
// 7. Display Schedule Details
// ============================================================
void DisplayScheduleDetails()
{
    Console.Clear();
    Console.WriteLine("--- SCHEDULE DETAILS ---");
    Console.Write("Schedule ID : ");
    if (!int.TryParse(Console.ReadLine(), out var scheduleId))
    {
        Console.WriteLine("Invalid ID.");
        Pause();
        return;
    }

    var schedule = scheduleService.GetById(scheduleId);
    if (schedule == null)
    {
        Console.WriteLine("Schedule not found.");
        Pause();
        return;
    }

    var bus = busService.GetById(schedule.BusId);
    Console.WriteLine(schedule);
    Console.WriteLine($"Bus: {bus}");
    Console.WriteLine($"Available seats: {bookingService.GetAvailableSeats(scheduleId, bus!.TotalSeats).Count}/{bus.TotalSeats}");
    Pause();
}

// ============================================================
// 8. Book Ticket
// ============================================================
void BookTicket()
{
    Console.Clear();
    Console.WriteLine("--- BOOK TICKET ---");

    var users = userService.GetAll();
    var schedules = scheduleService.GetAll();

    if (users.Count == 0) { Console.WriteLine("No users. Create a user first."); Pause(); return; }
    if (schedules.Count == 0) { Console.WriteLine("No schedules. Create a schedule first."); Pause(); return; }

    Console.WriteLine("Users:");
    users.ForEach(u => Console.WriteLine($"  {u}"));
    Console.Write("User ID : ");
    if (!int.TryParse(Console.ReadLine(), out var userId) || userService.GetById(userId) == null)
    {
        Console.WriteLine("Invalid User ID.");
        Pause();
        return;
    }

    Console.WriteLine("\nSchedules:");
    schedules.ForEach(s => Console.WriteLine($"  {s}"));
    Console.Write("Schedule ID : ");
    if (!int.TryParse(Console.ReadLine(), out var scheduleId))
    {
        Console.WriteLine("Invalid Schedule ID.");
        Pause();
        return;
    }

    var schedule = scheduleService.GetById(scheduleId);
    if (schedule == null)
    {
        Console.WriteLine("Schedule not found.");
        Pause();
        return;
    }

    var bus = busService.GetById(schedule.BusId);
    var available = bookingService.GetAvailableSeats(scheduleId, bus!.TotalSeats);

    Console.WriteLine($"\nAvailable seats ({available.Count}/{bus.TotalSeats}): {string.Join(", ", available)}");
    Console.Write("Choose a seat : ");
    if (!int.TryParse(Console.ReadLine(), out var seat))
    {
        Console.WriteLine("Invalid seat number.");
        Pause();
        return;
    }

    try
    {
        var ticket = bookingService.Book(userId, scheduleId, seat, bus.TotalSeats);
        var invoice = invoiceService.Create(ticket.Id, userId, schedule.Price);
        Console.WriteLine($"Ticket booked: {ticket}");
        Console.WriteLine($"Invoice generated: {invoice}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    Pause();
}

// ============================================================
// 9. Display User Invoices
// ============================================================
void DisplayUserInvoices()
{
    Console.Clear();
    Console.WriteLine("--- USER INVOICES ---");
    Console.Write("User ID : ");
    if (!int.TryParse(Console.ReadLine(), out var userId))
    {
        Console.WriteLine("Invalid User ID.");
        Pause();
        return;
    }

    var invoices = invoiceService.GetByUser(userId);
    if (invoices.Count == 0)
        Console.WriteLine("No invoices for this user.");
    else
        invoices.ForEach(i => Console.WriteLine(i));

    Pause();
}

// ============================================================
// 10. Process Invoice Payment
// ============================================================
void ProcessInvoicePayment()
{
    Console.Clear();
    Console.WriteLine("--- PROCESS PAYMENT ---");
    Console.Write("User ID : ");
    if (!int.TryParse(Console.ReadLine(), out var userId))
    {
        Console.WriteLine("Invalid User ID.");
        Pause();
        return;
    }

    var unpaid = invoiceService.GetUnpaidByUser(userId);
    if (unpaid.Count == 0)
    {
        Console.WriteLine("No unpaid invoices for this user.");
        Pause();
        return;
    }

    Console.WriteLine("Unpaid invoices:");
    unpaid.ForEach(i => Console.WriteLine($"  {i}"));
    Console.Write("Invoice ID to pay : ");
    if (!int.TryParse(Console.ReadLine(), out var invoiceId))
    {
        Console.WriteLine("Invalid Invoice ID.");
        Pause();
        return;
    }

    if (invoiceService.Pay(invoiceId))
        Console.WriteLine("Payment successful.");
    else
        Console.WriteLine("Payment failed. Check the Invoice ID.");

    Pause();
}

// ============================================================
// 11. Display User Tickets
// ============================================================
void DisplayUserTickets()
{
    Console.Clear();
    Console.WriteLine("--- USER TICKETS ---");
    Console.Write("User ID : ");
    if (!int.TryParse(Console.ReadLine(), out var userId))
    {
        Console.WriteLine("Invalid User ID.");
        Pause();
        return;
    }

    var tickets = bookingService.GetByUser(userId);
    if (tickets.Count == 0)
        Console.WriteLine("No tickets for this user.");
    else
        tickets.ForEach(t => Console.WriteLine(t));

    Pause();
}

// ============================================================
// Utility
// ============================================================
void Pause()
{
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
}
