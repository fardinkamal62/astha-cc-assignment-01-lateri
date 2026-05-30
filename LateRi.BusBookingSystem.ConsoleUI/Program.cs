using LateRi.BusBookingSystem.ConsoleUI.Enums;
using LateRi.BusBookingSystem.ConsoleUI.Repositories;
using LateRi.BusBookingSystem.ConsoleUI.Services;
using LateRi.BusBookingSystem.ConsoleUI.UI;

var userRepo = new UserRepository();
var busRepo = new BusRepository();
var scheduleRepo = new ScheduleRepository();
var ticketRepo = new TicketRepository();
var invoiceRepo = new InvoiceRepository();

var userService = new UserService(userRepo);
var busService = new BusService(busRepo);
var scheduleService = new ScheduleService(scheduleRepo);
var invoiceService = new InvoiceService(invoiceRepo);
var ticketService = new TicketService(ticketRepo);
var bookingService = new BookingService(ticketService, userService, scheduleService, busService, invoiceService);

SeedData();

var menu = new ConsoleMenu(userService, busService, scheduleService, bookingService, invoiceService);
menu.Run();

void SeedData()
{
    userService.Create("Fardin Kamal", "01711000001", "fardin@kamal.com");
    userService.Create("Abdullah Rayed", "01811000001", "rayed@ait.com");
    userService.Create("Rahat Khan Pathan", "01912000002", "rahat@ait.com");

    var bus1 = busService.Create("Shohagh Prestige", BusClassification.Business).Data!;
    var bus2 = busService.Create("Shyamoli NR Travels", BusClassification.Economy).Data!;

    scheduleService.Create(bus1.BusId, "Dhaka", "Chittagong", DateTime.Today.AddHours(10), 650m);
    scheduleService.Create(bus1.BusId, "Dhaka", "Sylhet", DateTime.Today.AddHours(15), 700m);
    scheduleService.Create(bus2.BusId, "Chittagong", "Cox's Bazar", DateTime.Today.AddDays(1).AddHours(9), 1200m);
}
