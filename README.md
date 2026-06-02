<div align="center"><h1>LateRi</h1></div>
<div align="center">CLI Bus Booking System</div>
<div align="center" style="color: grey"><sub>Version: 1.3.0</sub></div>


A C# console application for bus ticket booking and billing, built as **Assignment 1** of the Astha.IT CodeCamp — ServerCamp.

---

## Build & Run Instructions

### Clone the repository and navigate to the project directory:
```shell
git clone https://github.com/fardinkamal62/astha-cc-assignment-01-lateri.git
cd astha-cc-assignment-01-lateri/LateRi.BusBookingSystem
```

### Run the Application
```shell
dotnet run --project LateRi.BusBookingSystem.ConsoleUI
```

### Run Tests (Optional)

```shell
dotnet run --project LateRi.BusBookingSystem.ConsoleUI -- test
```

---

# Project Overview

## 1. What Was Required

Design and implement a Bus Ticket Booking & Billing System using C# and OOP, covering:

| Module | Key Requirements                                                                                                             |
|--------|------------------------------------------------------------------------------------------------------------------------------|
| **User Management** | Create users (ID, name, mobile, email); one user may book many tickets                                                       |
| **Bus Management** | Fleet with ID, coach number, classification (Business/Economy), seat capacity by class                                       |
| **Schedule Management** | Multiple schedules per bus; departure/arrival city, datetime, price; linked to a bus                                         |
| **Ticket Booking** | Browse schedules → pick a seat → book; validate seat range; prevent duplicate reservations; auto-generate invoice on payment |
| **Invoice & Payment** | Invoice with ID, ticket ID, user ID, amount, date, status; view invoices; pay outstanding ones                               |
| **Viewing & Operations** | Users, buses, schedules, schedule details, tickets, invoices, payments                                                       |

**Technical requirements:** C# Console Application, all 4 OOP pillars, SOLID principles, entity classes with properties and methods.

---

## 2. What Was Done

### Architecture

```
LateRi.BusBookingSystem.ConsoleUI/
├── Abstractions/          Result.cs
├── Enums/                 BusClassification.cs, PaymentStatus.cs
├── Interfaces/            IRepository<T>, IUserService, IBusService, IScheduleService,
│                          ITicketService, IInvoiceService, IPaymentProcessor + repos
├── Models/                BaseEntity (abstract), User, Bus, Schedule, Ticket, Invoice
├── Repositories/          UserRepository, BusRepository, ScheduleRepository,
│                          TicketRepository, InvoiceRepository
├── Services/              UserService, BusService, ScheduleService, BookingService,
│                          InvoiceService, TicketService, CashPaymentProcessor
├── UI/                    ConsoleHelper, ConsoleMenu, SeatLayoutRenderer
├── Program.cs             Entry point, DI wiring, seed data
├── TestRunner.cs          223 automated tests across 10 domains
```

### OOP Pillars Applied

- **Encapsulation** — Private `_reservedSeats` (HashSet), `_store` (Dictionary) in repos, controlled access via methods (`ReserveSeat`, `IsSeatAvailable`) and `IReadOnlyList<T>`
- **Inheritance** — `BaseEntity` (abstract) → User, Bus, Schedule, Ticket, Invoice. Each inherits `Id` + `CreatedAt`
- **Polymorphism** — `GetSummary()` overridden in every entity; `IPaymentProcessor` with `CashPaymentProcessor`; `BaseEntity.Display()` virtual method
- **Abstraction** — `BaseEntity` abstract class; 12 interfaces (`IRepository<T>`, service interfaces, `IPaymentProcessor`); `Result<T>` pattern

### SOLID Principles Applied

| Principle | Implementation |
|-----------|---------------|
| **SRP** | Models / Repositories / Services / UI — completely separated concerns |
| **OCP** | `IPaymentProcessor` allows adding Bkash/Nagad without changing `InvoiceService` |
| **LSP** | All repositories fully satisfy their interface contracts |
| **ISP** | Generic `IRepository<T>` base + specialized interfaces add only what's needed |
| **DIP** | Services receive interfaces via constructor injection; no `new Repository()` inside services |

### Extra Features

- **223 automated tests** across 10 domains (BaseEntity, User, Bus, Seat Ops, Schedule, Booking, Invoice, Cancellation, OOP Verification, E2E)
- **Batch booking** — multi-seat booking in one operation with a single invoice
- **Booking cancellation** — cancels invoice, removes ticket, frees the seat
- **SeatLayoutRenderer** — visual bus layout (green = available, red = taken)
- **Result<T> pattern** — consistent success/failure propagation through the entire stack
- **Seed data** — 3 users, 2 buses (Business + Economy), 3 schedules preloaded

### All 11 Required Operations

| # | Operation | Status |
|---|-----------|--------|
| 1 | Create User | ✅ |
| 2 | Display All Users | ✅ |
| 3 | Create Bus | ✅ |
| 4 | Display All Buses | ✅ |
| 5 | Create Schedule | ✅ |
| 6 | Display All Schedules | ✅ |
| 7 | Display Schedule Details | ✅ |
| 8 | Book Ticket | ✅ |
| 9 | Display User Invoices | ✅ |
| 10 | Process Invoice Payment | ✅ |
| 11 | Display User Tickets | ✅ |

---

## 3. Learnings

### C# & Language Features
- Primary constructors — concise class declarations with constructor params auto-captured
- `IReadOnlyList<T>` / `IReadOnlyCollection<T>` for safe internal-data exposure
- `Guid.NewGuid().ToString("N")[..8]` for 8-char unique IDs
- Generic interfaces (`IRepository<T>`) for reusable data access contracts

### OOP & Design
- Abstract classes define common behavior + force subclasses to implement specifics
- Polymorphic `GetSummary()` + `Display()` lets callers treat all entities uniformly
- Interface segregation prevents fat interfaces — each repo interface adds only relevant methods
- Dependency injection through constructor params (no service locator, no `new` in services)
- Strategy pattern via `IPaymentProcessor` — closed for modification, open for extension
- Result object pattern over exceptions for expected business-rule failures

### SOLID in Practice
- SRP becomes natural when you separate: Model (data) → Repository (storage) → Service (logic) → UI (presentation)
- DIP forces you to think about what each component actually needs — leads to cleaner boundaries
- ISP naturally emerges from SRP — small focused interfaces are easier to name, test, and swap

---

## 4. Shortcomings & Future Improvements

### Current Limitations

| Area | Issue |
|------|-------|
| **Persistence** | All data is in-memory (`Dictionary<T>`) — lost on app restart. No file/DB storage. |
| **Payment Status** | `PaymentStatus` only has `Paid`/`Unpaid`. Cancelled invoices are indistinguishable from unpaid ones (both = `Unpaid`). A `Cancelled` enum value is needed. |
| **Schedule Validation** | No check that departure < arrival datetime. Past-dated schedules are accepted. |
| **Bus Validation** | Duplicate coach names are allowed (no uniqueness constraint). |
| **User Validation** | Duplicate emails/mobiles are not checked. |
| **Seat Format** | Seat codes are tightly coupled to letter+number format (`A1`, `D10`). Adding new bus types with different layouts requires changes to `Bus.GenerateAllSeats()`. |
| **Concurrency** | No thread safety — `HashSet`/`Dictionary` ops are not locked. Multi-user scenarios could cause race conditions. |
| **UI Input** | Console input is not validated for type mismatches (e.g., letter in price field — handled via `TryParse` but no retry loop). |
| **Test Coverage** | No edge-case tests for concurrent operations, null arguments, or extreme values. |
