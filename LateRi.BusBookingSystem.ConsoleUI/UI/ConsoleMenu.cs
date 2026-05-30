using LateRi.BusBookingSystem.ConsoleUI.Enums;
using LateRi.BusBookingSystem.ConsoleUI.Models;
using LateRi.BusBookingSystem.ConsoleUI.Services;

namespace LateRi.BusBookingSystem.ConsoleUI.UI;

public class ConsoleMenu(
    UserService userService,
    BusService busService,
    ScheduleService scheduleService,
    BookingService bookingService,
    InvoiceService invoiceService)
{
    public void Run()
    {
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
            Console.WriteLine("0/q. Exit");
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
                case "q": return;
                case "Q": return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void CreateUser()
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
        Console.WriteLine($"User created: {UserLabel(user)}");
        Pause();
    }

    private void DisplayAllUsers()
    {
        Console.Clear();
        Console.WriteLine("--- ALL USERS ---");
        var users = userService.GetAll();
        if (users.Count == 0)
            Console.WriteLine("No users registered.");
        else
            PrintList(users, UserLabel);
        Pause();
    }

    private void CreateBus()
    {
        Console.Clear();
        Console.WriteLine("--- CREATE BUS ---");
        Console.Write("Coach Number : ");
        var coach = Console.ReadLine()!;
        Console.Write("Classification (1 = Business, 2 = Economy) : ");
        var clsInput = Console.ReadLine();
        var classification = clsInput == "1" ? BusClassification.Business : BusClassification.Economy;

        var bus = busService.Create(coach, classification);
        Console.WriteLine($"Bus created: {BusLabel(bus)}");
        Pause();
    }

    private void DisplayAllBuses()
    {
        Console.Clear();
        Console.WriteLine("--- ALL BUSES ---");
        var buses = busService.GetAll();
        if (buses.Count == 0)
            Console.WriteLine("No buses in fleet.");
        else
            PrintList(buses, BusLabel);
        Pause();
    }

    private void CreateSchedule()
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
        var selectedBus = SelectFromList(buses, BusLabel, "Select bus number: ");
        if (selectedBus == null)
        {
            Console.WriteLine("Invalid selection.");
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

        var schedule = scheduleService.Create(selectedBus.BusId, from, to, departure, price);
        Console.WriteLine($"Schedule created: {ScheduleLabel(schedule)}");
        Pause();
    }

    private void DisplayAllSchedules()
    {
        Console.Clear();
        Console.WriteLine("--- ALL SCHEDULES ---");
        var schedules = scheduleService.GetAll();
        if (schedules.Count == 0)
            Console.WriteLine("No schedules.");
        else
            PrintList(schedules, ScheduleLabel);
        Pause();
    }

    private void DisplayScheduleDetails()
    {
        Console.Clear();
        Console.WriteLine("--- SCHEDULE DETAILS ---");

        var schedules = scheduleService.GetAll();
        if (schedules.Count == 0)
        {
            Console.WriteLine("No schedules.");
            Pause();
            return;
        }

        var schedule = SelectFromList(schedules, ScheduleLabel, "Select schedule number: ");
        if (schedule == null)
        {
            Console.WriteLine("Invalid selection.");
            Pause();
            return;
        }

        var bus = busService.GetById(schedule.BusId);
        Console.WriteLine(ScheduleLabel(schedule));
        if (bus != null)
            Console.WriteLine($"Bus: {BusLabel(bus)}");
        if (bus != null)
            Console.WriteLine(
                $"Available seats: {bookingService.GetAvailableSeats(schedule.ScheduleId).Count}/{bus.TotalSeats}");
        Pause();
    }

    private void BookTicket()
    {
        Console.Clear();
        Console.WriteLine("--- BOOK TICKET ---");

        var users = userService.GetAll();
        var schedules = scheduleService.GetAll();

        if (users.Count == 0)
        {
            Console.WriteLine("No users. Create a user first.");
            Pause();
            return;
        }

        if (schedules.Count == 0)
        {
            Console.WriteLine("No schedules. Create a schedule first.");
            Pause();
            return;
        }

        Console.WriteLine("Users:");
        var user = SelectFromList(users, UserLabel, "Select user number: ");
        if (user == null)
        {
            Console.WriteLine("Invalid selection.");
            Pause();
            return;
        }

        Console.WriteLine("\nSchedules:");
        var schedule = SelectFromList(schedules, ScheduleLabel, "Select schedule number: ");
        if (schedule == null)
        {
            Console.WriteLine("Invalid selection.");
            Pause();
            return;
        }

        var bus = busService.GetById(schedule.BusId);
        if (bus == null)
        {
            Console.WriteLine("Bus not found for this schedule.");
            Pause();
            return;
        }

        var available = bookingService.GetAvailableSeats(schedule.ScheduleId);

        Console.WriteLine($"\nAvailable seats ({available.Count}/{bus.TotalSeats}): {string.Join(", ", available)}");
        Console.Write($"Choose a seat number (1–{bus.TotalSeats}) : ");
        if (!int.TryParse(Console.ReadLine(), out var seat))
        {
            Console.WriteLine("Invalid seat number.");
            Pause();
            return;
        }

        var result = bookingService.Book(user.UserId, schedule.ScheduleId, seat);
        if (result.Success)
        {
            Console.WriteLine(result.Message);
            Console.WriteLine($"Ticket booked: {TicketLabel(result.Ticket!)}");
            Console.WriteLine("Invoice auto-generated.");
        }
        else
        {
            Console.WriteLine($"Error: {result.Message}");
        }

        Pause();
    }

    private void DisplayUserInvoices()
    {
        Console.Clear();
        Console.WriteLine("--- USER INVOICES ---");
        var users = userService.GetAll();
        if (users.Count == 0)
        {
            Console.WriteLine("No users registered.");
            Pause();
            return;
        }

        var user = SelectFromList(users, UserLabel, "Select user number: ");
        if (user == null)
        {
            Console.WriteLine("Invalid selection.");
            Pause();
            return;
        }

        var invoices = invoiceService.GetByUser(user.UserId);
        if (invoices.Count == 0)
            Console.WriteLine("No invoices for this user.");
        else
            PrintList(invoices, InvoiceLabel);

        Pause();
    }

    private void ProcessInvoicePayment()
    {
        Console.Clear();
        Console.WriteLine("--- PROCESS PAYMENT ---");
        var users = userService.GetAll();
        if (users.Count == 0)
        {
            Console.WriteLine("No users registered.");
            Pause();
            return;
        }

        var user = SelectFromList(users, UserLabel, "Select user number: ");
        if (user == null)
        {
            Console.WriteLine("Invalid selection.");
            Pause();
            return;
        }

        var unpaid = invoiceService.GetUnpaidByUser(user.UserId);
        if (unpaid.Count == 0)
        {
            Console.WriteLine("No unpaid invoices for this user.");
            Pause();
            return;
        }

        Console.WriteLine("Unpaid invoices:");
        var invoice = SelectFromList(unpaid, InvoiceLabel, "Select invoice number: ");
        if (invoice == null)
        {
            Console.WriteLine("Invalid selection.");
            Pause();
            return;
        }

        if (invoiceService.Pay(invoice.Id))
            Console.WriteLine("Payment successful.");
        else
            Console.WriteLine("Payment failed. Check the Invoice ID.");

        Pause();
    }

    private void DisplayUserTickets()
    {
        Console.Clear();
        Console.WriteLine("--- USER TICKETS ---");
        var users = userService.GetAll();
        if (users.Count == 0)
        {
            Console.WriteLine("No users registered.");
            Pause();
            return;
        }

        var user = SelectFromList(users, UserLabel, "Select user number: ");
        if (user == null)
        {
            Console.WriteLine("Invalid selection.");
            Pause();
            return;
        }

        var tickets = bookingService.GetByUser(user.UserId);
        if (tickets.Count == 0)
            Console.WriteLine("No tickets for this user.");
        else
            PrintList(tickets, TicketLabel);

        Pause();
    }
    private static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void PrintList<T>(IReadOnlyList<T> items, Func<T, string> label)
    {
        for (var i = 0; i < items.Count; i++)
            Console.WriteLine($"{i + 1}. {label(items[i])}");
    }

    private static T? SelectFromList<T>(IReadOnlyList<T> items, Func<T, string> label, string prompt)
        where T : class
    {
        if (items.Count == 0) return null;
        PrintList(items, label);
        Console.Write(prompt);
        if (!int.TryParse(Console.ReadLine(), out var choice)) return null;
        if (choice < 1 || choice > items.Count) return null;
        return items[choice - 1];
    }

    private static string UserLabel(User user) =>
        $"{user.Name} | {user.Mobile} | {user.Email}";

    private static string BusLabel(Bus bus) =>
        $"Coach: {bus.CoachNumber} | Class: {bus.Classification} | " +
        $"Seats: {bus.TotalSeats}";

    private static string ScheduleLabel(Schedule schedule) =>
        $"{schedule.DepartureCity} -> {schedule.ArrivalCity} | " +
        $"{schedule.DepartureDateTime:dd MMM yyyy HH:mm} | BDT {schedule.TicketPrice:F2}";

    private static string TicketLabel(Ticket ticket) =>
        $"Seat {ticket.SeatNumber} | BDT {ticket.Price:F2} | Booked {ticket.BookingDateTime:dd MMM yyyy}";

    private static string InvoiceLabel(Invoice invoice) =>
        $"BDT {invoice.AmountDue:F2} | {invoice.Status} | {invoice.GeneratedDate:dd MMM yyyy}";
}
