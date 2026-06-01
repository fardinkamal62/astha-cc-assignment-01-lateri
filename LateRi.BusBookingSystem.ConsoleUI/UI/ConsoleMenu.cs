using LateRi.BusBookingSystem.ConsoleUI.Enums;
using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;
using LateRi.BusBookingSystem.ConsoleUI.Services;

namespace LateRi.BusBookingSystem.ConsoleUI.UI;

public class ConsoleMenu(
    IUserService userService,
    IBusService busService,
    IScheduleService scheduleService,
    BookingService bookingService,
    IInvoiceService invoiceService)
{
    public void Run()
    {
        while (true)
        {
            Console.Clear();
            ConsoleHelper.WriteBanner("LateRi Bus Booking System");
            ConsoleHelper.WriteColor($"  Admin Menu\n", ConsoleColor.DarkYellow);
            ConsoleHelper.WriteInfo("  1. Create User");
            ConsoleHelper.WriteInfo("  2. Display All Users");
            ConsoleHelper.WriteInfo("  3. Create Bus");
            ConsoleHelper.WriteInfo("  4. Create Schedule");
            ConsoleHelper.WriteSeparator();
            ConsoleHelper.WriteColor($"  User Menu\n", ConsoleColor.DarkCyan);
            ConsoleHelper.WriteInfo("  5. Display All Buses");
            ConsoleHelper.WriteInfo("  6. Display All Schedules");
            ConsoleHelper.WriteInfo("  7. Display Schedule Details");
            ConsoleHelper.WriteInfo("  8. Book Ticket");
            ConsoleHelper.WriteInfo("  9. Display User Invoices");
            ConsoleHelper.WriteInfo("  10. Process Invoice Payment");
            ConsoleHelper.WriteInfo("  11. Display User Tickets");
            ConsoleHelper.WriteSeparator();
            ConsoleHelper.WriteInfo("  0/q. Exit");
            Console.WriteLine();
            ConsoleHelper.WritePrompt("Choose an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": CreateUser(); break;
                case "2": DisplayAllUsers(); break;
                case "3": CreateBus(); break;
                case "4": CreateSchedule(); break;
                case "5": DisplayAllBuses(); break;
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
                    ConsoleHelper.WriteError("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void CreateUser()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("CREATE USER");
        ConsoleHelper.WritePrompt("Name : ");
        var name = Console.ReadLine()!;
        ConsoleHelper.WritePrompt("Mobile : ");
        var mobile = Console.ReadLine()!;
        ConsoleHelper.WritePrompt("Email : ");
        var email = Console.ReadLine()!;

        var userResult = userService.Create(name, mobile, email);
        if (userResult.IsFailure)
        {
            ConsoleHelper.WriteError(userResult.Message);
            ConsoleHelper.Pause();
            return;
        }
        ConsoleHelper.WriteSuccess($"User created: {UserLabel(userResult.Data!)}");
        ConsoleHelper.Pause();
    }

    private void DisplayAllUsers()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("ALL USERS");
        var users = userService.GetAll();
        if (users.Count == 0)
            ConsoleHelper.WriteWarning("No users registered.");
        else
            ConsoleHelper.PrintList(users, UserLabel);
        ConsoleHelper.Pause();
    }

    private void CreateBus()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("CREATE BUS");
        ConsoleHelper.WritePrompt("Coach Name : ");
        var coach = Console.ReadLine()!;
        WriteClassificationPrompt();
        var clsInput = Console.ReadLine();
        var classification = clsInput == "1" ? BusClassification.Business : BusClassification.Economy;
        var defaultSeats = (int)classification;

        ConsoleHelper.WritePrompt($"Total seats (default {defaultSeats}, or press Enter to skip) : ");
        var seatsInput = Console.ReadLine();
        var totalSeats = 0;
        if (!string.IsNullOrWhiteSpace(seatsInput) && int.TryParse(seatsInput, out var parsed) && parsed > 0)
            totalSeats = parsed;

        var busResult = busService.Create(coach, classification, totalSeats);
        if (busResult.IsFailure)
        {
            ConsoleHelper.WriteError(busResult.Message);
            ConsoleHelper.Pause();
            return;
        }
        ConsoleHelper.WriteSuccess($"Bus created: {BusLabel(busResult.Data!)}");
        ConsoleHelper.Pause();
    }

    private static void WriteClassificationPrompt()
    {
        ConsoleHelper.WriteColor("Classification : ", ConsoleColor.White);
        ConsoleHelper.WriteColor("1", ConsoleColor.Cyan);
        ConsoleHelper.WriteColor(" = Business, ", ConsoleColor.Gray);
        ConsoleHelper.WriteColor("2", ConsoleColor.Cyan);
        ConsoleHelper.WriteLineColor(" = Economy", ConsoleColor.Gray);
    }

    private void DisplayAllBuses()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("ALL BUSES");
        var buses = busService.GetAll();
        if (buses.Count == 0)
            ConsoleHelper.WriteWarning("No buses in fleet.");
        else
            ConsoleHelper.PrintList(buses, BusLabel);
        ConsoleHelper.Pause();
    }

    private void CreateSchedule()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("CREATE SCHEDULE");

        var buses = busService.GetAll();
        if (buses.Count == 0)
        {
            ConsoleHelper.WriteWarning("No buses available. Create a bus first.");
            ConsoleHelper.Pause();
            return;
        }

        ConsoleHelper.WriteInfo("Available buses:");
        var selectedBus = ConsoleHelper.SelectFromList(buses, BusLabel, "Select bus number: ");
        if (selectedBus == null)
        {
            ConsoleHelper.WriteError("Invalid selection.");
            ConsoleHelper.Pause();
            return;
        }

        ConsoleHelper.WritePrompt("Departure City : ");
        var from = Console.ReadLine()!;
        ConsoleHelper.WritePrompt("Arrival City : ");
        var to = Console.ReadLine()!;
        ConsoleHelper.WritePrompt("Departure (yyyy-MM-dd HH:mm) : ");
        if (!DateTime.TryParse(Console.ReadLine(), out var departure))
        {
            ConsoleHelper.WriteError("Invalid date/time.");
            ConsoleHelper.Pause();
            return;
        }

        ConsoleHelper.WritePrompt("Ticket Price : ");
        if (!decimal.TryParse(Console.ReadLine(), out var price))
        {
            ConsoleHelper.WriteError("Invalid price.");
            ConsoleHelper.Pause();
            return;
        }

        var scheduleResult = scheduleService.Create(selectedBus.BusId, from, to, departure, price);
        if (scheduleResult.IsFailure)
        {
            ConsoleHelper.WriteError(scheduleResult.Message);
            ConsoleHelper.Pause();
            return;
        }
        ConsoleHelper.WriteSuccess($"Schedule created: {ScheduleLabel(scheduleResult.Data!, selectedBus)}");
        ConsoleHelper.Pause();
    }

    private void DisplayAllSchedules()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("ALL SCHEDULES");
        var schedules = scheduleService.GetAll();
        if (schedules.Count == 0)
            ConsoleHelper.WriteWarning("No schedules.");
        else
            ConsoleHelper.PrintList(schedules, s => ScheduleLabel(s, busService.GetById(s.BusId)!));
        ConsoleHelper.Pause();
    }

    private void DisplayScheduleDetails()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("SCHEDULE DETAILS");

        var schedules = scheduleService.GetAll();
        if (schedules.Count == 0)
        {
            ConsoleHelper.WriteWarning("No schedules.");
            ConsoleHelper.Pause();
            return;
        }

        var schedule = ConsoleHelper.SelectFromList(schedules, s => ScheduleLabel(s, busService.GetById(s.BusId)!), "Select schedule number: ");
        if (schedule == null)
        {
            ConsoleHelper.WriteError("Invalid selection.");
            ConsoleHelper.Pause();
            return;
        }

        var bus = busService.GetById(schedule.BusId);
        if (bus != null)
        {
            ConsoleHelper.WriteInfo(ScheduleLabel(schedule, bus));
            ConsoleHelper.WriteInfo($"Bus: {BusLabel(bus)}");
            ConsoleHelper.WriteInfo(
                $"Available seats: {bookingService.GetAvailableSeats(schedule.ScheduleId).Count}/{bus.TotalSeats}");
            SeatLayoutRenderer.Render(bus);
        }
        ConsoleHelper.Pause();
    }

    private void BookTicket()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("BOOK TICKET");

        var users = userService.GetAll();
        var schedules = scheduleService.GetAll();

        if (users.Count == 0)
        {
            ConsoleHelper.WriteWarning("No users. Create a user first.");
            ConsoleHelper.Pause();
            return;
        }

        if (schedules.Count == 0)
        {
            ConsoleHelper.WriteWarning("No schedules. Create a schedule first.");
            ConsoleHelper.Pause();
            return;
        }

        ConsoleHelper.WriteInfo("Users:");
        var user = ConsoleHelper.SelectFromList(users, UserLabel, "Select user number: ");
        if (user == null)
        {
            ConsoleHelper.WriteError("Invalid selection.");
            ConsoleHelper.Pause();
            return;
        }

        Console.WriteLine();
        ConsoleHelper.WriteInfo("Schedules:");
        var schedule = ConsoleHelper.SelectFromList(schedules, s => ScheduleLabel(s, busService.GetById(s.BusId)!), "Select schedule number: ");
        if (schedule == null)
        {
            ConsoleHelper.WriteError("Invalid selection.");
            ConsoleHelper.Pause();
            return;
        }

        var bus = busService.GetById(schedule.BusId);
        if (bus == null)
        {
            ConsoleHelper.WriteError("Bus not found for this schedule.");
            ConsoleHelper.Pause();
            return;
        }

        var available = bookingService.GetAvailableSeats(schedule.ScheduleId);

        ConsoleHelper.WriteInfo($"Available seats ({available.Count}/{bus.TotalSeats}):");
        SeatLayoutRenderer.Render(bus);
        ConsoleHelper.WritePrompt("Choose seat(s) (comma separated, e.g. A1, B3) : ");
        var seatInput = Console.ReadLine();
        var seatCodes = seatInput?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => s.ToUpper())
            .ToList() ?? [];

        if (seatCodes.Count == 0)
        {
            ConsoleHelper.WriteError("No seats specified.");
            ConsoleHelper.Pause();
            return;
        }

        var result = bookingService.BatchBook(user.UserId, schedule.ScheduleId, seatCodes);
        if (result.IsSuccess)
        {
            ConsoleHelper.WriteSuccess(result.Message);
            foreach (var ticket in result.Data!)
                ConsoleHelper.WriteSuccess($"  Seat {ticket.SeatNumber} — BDT {ticket.Price:F2} | Booked {ticket.BookingDateTime:dd MMM yyyy}");
            ConsoleHelper.WriteInfo("Single invoice auto-generated for all tickets.");
        }
        else
        {
            ConsoleHelper.WriteError(result.Message);
        }

        ConsoleHelper.Pause();
    }

    private void DisplayUserInvoices()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("USER INVOICES");
        var users = userService.GetAll();
        if (users.Count == 0)
        {
            ConsoleHelper.WriteWarning("No users registered.");
            ConsoleHelper.Pause();
            return;
        }

        var user = ConsoleHelper.SelectFromList(users, UserLabel, "Select user number: ");
        if (user == null)
        {
            ConsoleHelper.WriteError("Invalid selection.");
            ConsoleHelper.Pause();
            return;
        }

        var invoices = invoiceService.GetByUser(user.UserId);
        if (invoices.Count == 0)
            ConsoleHelper.WriteWarning("No invoices for this user.");
        else
            ConsoleHelper.PrintList(invoices, InvoiceLabel);

        ConsoleHelper.Pause();
    }

    private void ProcessInvoicePayment()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("PROCESS PAYMENT");
        var users = userService.GetAll();
        if (users.Count == 0)
        {
            ConsoleHelper.WriteWarning("No users registered.");
            ConsoleHelper.Pause();
            return;
        }

        var user = ConsoleHelper.SelectFromList(users, UserLabel, "Select user number: ");
        if (user == null)
        {
            ConsoleHelper.WriteError("Invalid selection.");
            ConsoleHelper.Pause();
            return;
        }

        var unpaid = invoiceService.GetUnpaidByUser(user.UserId);
        if (unpaid.Count == 0)
        {
            ConsoleHelper.WriteWarning("No unpaid invoices for this user.");
            ConsoleHelper.Pause();
            return;
        }

        ConsoleHelper.WriteInfo("Unpaid invoices:");
        var invoice = ConsoleHelper.SelectFromList(unpaid, InvoiceLabel, "Select invoice number: ");
        if (invoice == null)
        {
            ConsoleHelper.WriteError("Invalid selection.");
            ConsoleHelper.Pause();
            return;
        }

        IReadOnlyList<string> choices = ["Pay", "Cancel"];
        var choice = ConsoleHelper.SelectFromList(choices, c => c, "Choose action: ");
        if (choice == null)
        {
            ConsoleHelper.WriteError("Invalid selection.");
            ConsoleHelper.Pause();
            return;
        }

        if (choice == "Cancel")
        {
            var result = bookingService.CancelBooking(user.UserId, invoice.Id);
            ConsoleHelper.WriteInfo(result.Message);

            ConsoleHelper.Pause();
            return;
        }

        var payResult = invoiceService.Pay(invoice.Id);
        if (payResult.IsSuccess)
            ConsoleHelper.WriteSuccess(payResult.Message);
        else
            ConsoleHelper.WriteError(payResult.Message);

        ConsoleHelper.Pause();
    }

    private void DisplayUserTickets()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("USER TICKETS");
        var users = userService.GetAll();
        if (users.Count == 0)
        {
            ConsoleHelper.WriteWarning("No users registered.");
            ConsoleHelper.Pause();
            return;
        }

        var user = ConsoleHelper.SelectFromList(users, UserLabel, "Select user number: ");
        if (user == null)
        {
            ConsoleHelper.WriteError("Invalid selection.");
            ConsoleHelper.Pause();
            return;
        }

        var tickets = bookingService.GetByUser(user.UserId);
        if (tickets.Count == 0)
            ConsoleHelper.WriteWarning("No tickets for this user.");
        else
            ConsoleHelper.PrintList(tickets, TicketLabel);

        ConsoleHelper.Pause();
    }

    private static string UserLabel(User user) =>
        $"{user.Name} | {user.Mobile} | {user.Email}";

    private static string BusLabel(Bus bus) =>
        $"Coach: {bus.CoachName} | Class: {bus.Classification} | " +
        $"Seats: {bus.TotalSeats}";

    private static string ScheduleLabel(Schedule schedule, Bus bus) =>
        $"{schedule.DepartureCity} -> {schedule.ArrivalCity} | " +
        $"{schedule.DepartureDateTime:dd MMM yyyy HH:mm} | Bus Class: {bus.Classification} | BDT {schedule.TicketPrice:F2}";

    private static string TicketLabel(Ticket ticket) =>
        $"Seat {ticket.SeatNumber} | BDT {ticket.Price:F2} | Booked {ticket.BookingDateTime:dd MMM yyyy}";

    private static string InvoiceLabel(Invoice invoice) =>
        $"BDT {invoice.AmountDue:F2} | {invoice.Status} | {invoice.GeneratedDate:dd MMM yyyy}";
}
