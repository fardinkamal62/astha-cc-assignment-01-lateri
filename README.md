# LateRi Bus Booking System

A C# console application for bus ticket booking and billing, built as **Assignment 1** of the Astha.IT CodeCamp — ServerCamp.

---

## Requirements

The system covers 6 functional areas:

| # | Module | Description |
|---|--------|-------------|
| 1 | **User Management** | Create users (ID, name, mobile, email). One user can book many tickets. |
| 2 | **Bus Management** | Manage fleet — each bus has an ID, coach number, classification (Business/Economy), and seat capacity. |
| 3 | **Schedule Management** | Each bus can have multiple schedules (departure/arrival city, datetime, price). |
| 4 | **Ticket Booking** | Browse schedules → pick a seat → book. Validates seat against capacity, prevents double-booking. |
| 5 | **Invoice & Payment** | Auto-generate invoice per booking. Users can view invoices and pay outstanding ones. |
| 6 | **Viewing & Operations** | CRUD operations: users, buses, schedules, bookings, invoices, payments. |

---

## Functional Operations

- Create User
- Display All Users
- Create Bus
- Display All Buses
- Create Schedule
- Display All Schedules
- Display Schedule Details
- Book Ticket
- Display User Invoices
- Process Invoice Payment
- Display User Tickets

---

## Project Structure

```
LateRi.BusBookingSystem/
├── LateRi.BusBookingSystem.ConsoleUI/    # Console app entry point
│   ├── Program.cs
│   └── LateRi.BusBookingSystem.ConsoleUI.csproj
├── LateRi.BusBookingSystem.sln
├── Assignment 01.pdf
└── README.md
```

---

## Technical Requirements

- **Language:** C# (.NET 10.0)
- **Type:** Console Application
- **OOP Pillars:** Encapsulation, Inheritance, Polymorphism, Abstraction
- **Design Principles:** SOLID
- **All entities modeled as classes**

---

## Run

```bash
dotnet run --project LateRi.BusBookingSystem.ConsoleUI
```
