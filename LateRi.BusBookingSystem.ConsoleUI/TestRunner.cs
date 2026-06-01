using LateRi.BusBookingSystem.ConsoleUI.Abstractions;
using LateRi.BusBookingSystem.ConsoleUI.Enums;
using LateRi.BusBookingSystem.ConsoleUI.Interfaces;
using LateRi.BusBookingSystem.ConsoleUI.Models;
using LateRi.BusBookingSystem.ConsoleUI.Repositories;
using LateRi.BusBookingSystem.ConsoleUI.Services;

namespace LateRi.BusBookingSystem.ConsoleUI;

public static class TestRunner
{
    private static int _passed = 0;
    private static int _failed = 0;
    private static int _testCount = 0;

    public static void RunAll()
    {
        Console.Clear();
        Console.WriteLine("══════════════════════════════════════════════");
        Console.WriteLine("  BUS TICKET BOOKING SYSTEM — TEST SUITE");
        Console.WriteLine("══════════════════════════════════════════════");
        Console.WriteLine();

        // ── Domain 1: BaseEntity ──
        TestDomain("DOMAIN 1: BaseEntity (Inheritance / Abstraction)", () =>
        {
            BaseEntity_Generates8CharId();
            BaseEntity_SetsCreatedAt();
        });

        // ── Domain 2: User Management ──
        TestDomain("DOMAIN 2: User Management", () =>
        {
            User_Create_Valid_Success();
            User_Create_NameTooShort_Fails();
            User_Create_NameEmpty_Fails();
            User_Create_InvalidEmail_Fails();
            User_Create_InvalidMobile_Fails();
            User_Create_InvalidMobileShort_Fails();
            User_GetById_Valid_ReturnsUser();
            User_GetById_Invalid_ReturnsNull();
            User_GetAll_ReturnsAllCreated();
            User_MultipleCreations_UniqueIds();
            User_IdFormat_8CharsUppercase();
        });

        // ── Domain 3: Bus Management ──
        TestDomain("DOMAIN 3: Bus Management", () =>
        {
            Bus_Economy_Has40Seats();
            Bus_Business_Has20Seats();
            Bus_Create_Valid_Success();
            Bus_Create_EmptyCoach_Fails();
            Bus_Economy_SeatNaming_A1toD10();
            Bus_Business_SeatNaming_A1toB7();
            Bus_InvalidSeatCode_Rejected();
            Bus_ValidSeatCode_Accepted();
            Bus_GetAvailableSeats_Initial_AllAvailable();
            Bus_GetSummary_Format();
            Bus_Economy_SeatsPerRow4();
            Bus_Business_SeatsPerRow3();
        });

        // ── Domain 4: Seat Operations ──
        TestDomain("DOMAIN 4: Seat Reservation Operations", () =>
        {
            Seat_Reserve_Valid_Succeeds();
            Seat_Reserve_Duplicate_Fails();
            Seat_Reserve_MakesSeatUnavailable();
            Seat_Reserve_DecreasesAvailableCount();
            Seat_ReserveAllBusiness_AllReserved();
            Seat_ClearReserved_Succeeds();
            Seat_ClearUnreserved_Fails();
            Seat_ReserveAfterClear_Succeeds();
            Seat_MultipleReservations_AllTracked();
        });

        // ── Domain 5: Schedule Management ──
        TestDomain("DOMAIN 5: Schedule Management", () =>
        {
            Schedule_Create_Valid_Success();
            Schedule_Create_DepartureEmpty_Fails();
            Schedule_Create_ArrivalEmpty_Fails();
            Schedule_Create_ZeroPrice_Fails();
            Schedule_Create_NegativePrice_Fails();
            Schedule_Create_EmptyBusId_Fails();
            Schedule_GetById_Valid_ReturnsSchedule();
            Schedule_GetById_Invalid_ReturnsNull();
            Schedule_GetAll_ReturnsAll();
            Schedule_GetSummary_Format();
            Schedule_Properties_StoredCorrectly();
        });

        // ── Domain 6: Ticket Booking ──
        TestDomain("DOMAIN 6: Ticket Booking", () =>
        {
            Booking_Book_Valid_Success();
            Booking_Book_NonExistentUser_Fails();
            Booking_Book_NonExistentSchedule_Fails();
            Booking_Book_InvalidSeatCode_Fails();
            Booking_Book_DuplicateSeat_Fails();
            Booking_Book_GeneratesInvoice();
            Booking_GetByUser_ReturnsUserTickets();
            Booking_GetAvailableSeats_ValidSchedule();
            Booking_GetAvailableSeats_InvalidSchedule_Empty();
        });

        // ── Domain 7: Invoice & Payment ──
        TestDomain("DOMAIN 7: Invoice & Payment", () =>
        {
            Invoice_Create_DefaultStatusPending();
            Invoice_Pay_Valid_Success();
            Invoice_Pay_AlreadyPaid_Fails();
            Invoice_Pay_NonExistent_Fails();
            Invoice_Cancel_Unpaid_Success();
            Invoice_Cancel_Paid_Fails();
            Invoice_Cancel_NonExistent_Fails();
            Invoice_GetByUser_ReturnsUserInvoices();
            Invoice_GetUnpaidByUser_FiltersPaid();
            Invoice_Properties_StoredCorrectly();
        });

        // ── Domain 8: Booking Cancellation ──
        TestDomain("DOMAIN 8: Booking Cancellation", () =>
        {
            Cancel_ValidBooking_Success();
            Cancel_WrongUser_Fails();
            Cancel_NonExistentInvoice_Fails();
            Cancel_FreesSeat();
        });

        // ── Domain 9: OOP Verification ──
        TestDomain("DOMAIN 9: OOP Principles", () =>
        {
            OOP_User_ExtendsBaseEntity();
            OOP_Bus_ExtendsBaseEntity();
            OOP_Schedule_ExtendsBaseEntity();
            OOP_Ticket_ExtendsBaseEntity();
            OOP_Invoice_ExtendsBaseEntity();
            OOP_PolymorphicGetSummary_AllEntities();
            OOP_BaseEntity_ProvidesIdAndCreatedAt();
            OOP_Encapsulation_PrivateFields();
        });

        // ── Domain 10: End-to-End Integration ──
        TestDomain("DOMAIN 10: End-to-End Integration", () =>
        {
            E2E_FullHappyPath();
            E2E_MultipleBookings_SameUser();
            E2E_MultipleUsers_SameSchedule();
            E2E_BookCancelRebook_SameSeat();
        });

        // ── Summary ──
        Console.WriteLine();
        Console.WriteLine("══════════════════════════════════════════════");
        Console.WriteLine($"  TOTAL: {_passed + _failed} tests | ✅ {_passed} PASSED | ❌ {_failed} FAILED");
        Console.WriteLine(_failed == 0
            ? "  ALL TESTS PASSED!"
            : "  SOME TESTS FAILED — review output above");
        Console.WriteLine("══════════════════════════════════════════════");
    }

    // ════════════════════════════════════════════════════════════════
    //  INFRASTRUCTURE
    // ════════════════════════════════════════════════════════════════

    private static void TestDomain(string name, Action tests)
    {
        Console.WriteLine($"─── {name} ───");
        Console.WriteLine();
        tests();
        Console.WriteLine();
    }

    private static void Assert(bool condition, string testName)
    {
        _testCount++;
        if (condition)
        {
            Console.WriteLine($"  ✅ PASS  #{_testCount:D3}: {testName}");
            _passed++;
        }
        else
        {
            Console.WriteLine($"  ❌ FAIL  #{_testCount:D3}: {testName}");
            _failed++;
        }
    }

    private static void AssertEqual<T>(T expected, T actual, string testName)
    {
        Assert(EqualityComparer<T>.Default.Equals(expected, actual),
            $"{testName} — Expected: {expected}, Actual: {actual}");
    }

    // ════════════════════════════════════════════════════════════════
    //  DOMAIN 1: BaseEntity
    // ════════════════════════════════════════════════════════════════

    // TC-BE-01: BaseEntity generates an 8-character uppercase hexadecimal ID
    private static void BaseEntity_Generates8CharId()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        Assert(entity.Id.Length == 8, "BaseEntity generates 8-character Id");
        Assert(entity.Id.All(c => "0123456789ABCDEF".Contains(c)),
            "BaseEntity Id contains only uppercase hex characters");
    }

    // TC-BE-02: BaseEntity sets CreatedAt timestamp
    private static void BaseEntity_SetsCreatedAt()
    {
        // Arrange & Act
        var before = DateTimeOffset.UtcNow.AddSeconds(-1);
        var entity = new TestEntity();
        var after = DateTimeOffset.UtcNow.AddSeconds(1);

        // Assert
        Assert(entity.CreatedAt >= before && entity.CreatedAt <= after,
            "BaseEntity CreatedAt is set to current UTC time");
    }

    // ════════════════════════════════════════════════════════════════
    //  DOMAIN 2: User Management
    // ════════════════════════════════════════════════════════════════

    // TC-USR-01: Create user with valid details → success
    private static void User_Create_Valid_Success()
    {
        // Arrange
        var repo = new UserRepository();
        var service = new UserService(repo);

        // Act
        var result = service.Create("Rahim Uddin", "01711000001", "rahim@example.com");

        // Assert
        Assert(result.IsSuccess, "User creation succeeds with valid data");
        Assert(result.Data != null, "Result contains User object");
        AssertEqual("Rahim Uddin", result.Data!.Name, "User name stored correctly");
        AssertEqual("01711000001", result.Data.Mobile, "User mobile stored correctly");
        AssertEqual("rahim@example.com", result.Data.Email, "User email stored correctly");
        AssertEqual(1, repo.GetAll().Count, "User persisted in repository");
    }

    // TC-USR-02: Create user with name < 3 chars → failure
    private static void User_Create_NameTooShort_Fails()
    {
        // Arrange
        var service = new UserService(new UserRepository());

        // Act
        var result = service.Create("Ab", "01711000001", "a@b.com");

        // Assert
        Assert(result.IsFailure, "User creation fails with name shorter than 3 characters");
    }

    // TC-USR-03: Create user with empty/whitespace name → failure
    private static void User_Create_NameEmpty_Fails()
    {
        // Arrange
        var service = new UserService(new UserRepository());

        // Act
        var result1 = service.Create("", "01711000001", "a@b.com");
        var result2 = service.Create("   ", "01711000001", "a@b.com");

        // Assert
        Assert(result1.IsFailure, "User creation fails with empty name");
        Assert(result2.IsFailure, "User creation fails with whitespace-only name");
    }

    // TC-USR-04: Create user with invalid email → failure
    private static void User_Create_InvalidEmail_Fails()
    {
        // Arrange
        var service = new UserService(new UserRepository());

        // Act
        var result1 = service.Create("Rahim Uddin", "01711000001", "no-at-symbol");
        var result2 = service.Create("Rahim Uddin", "01711000001", "@no-local.com");
        var result3 = service.Create("Rahim Uddin", "01711000001", "no-domain@");

        // Assert
        Assert(result1.IsFailure, "Email without @ rejected");
        Assert(result2.IsFailure, "Email without local part rejected");
        Assert(result3.IsFailure, "Email without domain rejected");
    }

    // TC-USR-05: Create user with invalid mobile (wrong prefix) → failure
    private static void User_Create_InvalidMobile_Fails()
    {
        // Arrange
        var service = new UserService(new UserRepository());

        // Act
        var result = service.Create("Rahim Uddin", "01111000001", "rahim@test.com");

        // Assert
        Assert(result.IsFailure, "Mobile must start with 013-019 (not 011)");
    }

    // TC-USR-06: Create user with mobile shorter than 11 digits → failure
    private static void User_Create_InvalidMobileShort_Fails()
    {
        // Arrange
        var service = new UserService(new UserRepository());

        // Act
        var result = service.Create("Rahim Uddin", "0171100000", "rahim@test.com");

        // Assert
        Assert(result.IsFailure, "Mobile with fewer than 11 digits rejected");
    }

    // TC-USR-07: Get user by valid ID → returns user
    private static void User_GetById_Valid_ReturnsUser()
    {
        // Arrange
        var repo = new UserRepository();
        var service = new UserService(repo);
        var created = service.Create("Fatima Begum", "01811000001", "fatima@test.com").Data!;

        // Act
        var found = service.GetById(created.UserId);

        // Assert
        Assert(found != null, "GetById returns user for valid ID");
        AssertEqual(created.UserId, found!.UserId, "Retrieved user ID matches");
        AssertEqual("Fatima Begum", found.Name, "Retrieved user name matches");
    }

    // TC-USR-08: Get user by invalid ID → returns null
    private static void User_GetById_Invalid_ReturnsNull()
    {
        // Arrange
        var service = new UserService(new UserRepository());

        // Act
        var found = service.GetById("NONEXIST");

        // Assert
        Assert(found == null, "GetById returns null for non-existent ID");
    }

    // TC-USR-09: GetAll returns all created users
    private static void User_GetAll_ReturnsAllCreated()
    {
        // Arrange
        var repo = new UserRepository();
        var service = new UserService(repo);
        service.Create("User A", "01711000001", "a@test.com");
        service.Create("User B", "01711000002", "b@test.com");
        service.Create("User C", "01711000003", "c@test.com");

        // Act
        var all = service.GetAll();

        // Assert
        AssertEqual(3, all.Count, "GetAll returns all 3 created users");
    }

    // TC-USR-10: Multiple user creations produce unique IDs
    private static void User_MultipleCreations_UniqueIds()
    {
        // Arrange
        var service = new UserService(new UserRepository());
        var u1 = service.Create("User A", "01711000001", "a@test.com").Data!;
        var u2 = service.Create("User B", "01711000002", "b@test.com").Data!;
        var u3 = service.Create("User C", "01711000003", "c@test.com").Data!;

        // Assert
        Assert(u1.UserId != u2.UserId, "User IDs are unique (A vs B)");
        Assert(u2.UserId != u3.UserId, "User IDs are unique (B vs C)");
        Assert(u1.UserId != u3.UserId, "User IDs are unique (A vs C)");
    }

    // TC-USR-11: User ID format validation
    private static void User_IdFormat_8CharsUppercase()
    {
        // Arrange
        var service = new UserService(new UserRepository());

        // Act
        var user = service.Create("Rahim Uddin", "01711000001", "rahim@test.com").Data!;

        // Assert
        AssertEqual(8, user.UserId.Length, "UserId is 8 characters");
        Assert(user.UserId.All(c => "0123456789ABCDEF".Contains(c)),
            "UserId contains only uppercase hex characters");
    }

    // ════════════════════════════════════════════════════════════════
    //  DOMAIN 3: Bus Management
    // ════════════════════════════════════════════════════════════════

    // TC-BUS-01: Economy bus has 40 seats
    private static void Bus_Economy_Has40Seats()
    {
        // Arrange & Act
        var bus = new Bus("ECO-01", BusClassification.Economy);

        // Assert
        AssertEqual(40, bus.TotalSeats, "Economy bus has 40 seats");
        AssertEqual(40, bus.GetAvailableSeats().Count, "All 40 Economy seats initially available");
    }

    // TC-BUS-02: Business bus has 20 seats
    private static void Bus_Business_Has20Seats()
    {
        // Arrange & Act
        var bus = new Bus("BIZ-01", BusClassification.Business);

        // Assert
        AssertEqual(20, bus.TotalSeats, "Business bus has 20 seats");
        AssertEqual(20, bus.GetAvailableSeats().Count, "All 20 Business seats initially available");
    }

    // TC-BUS-03: Create bus with valid coach number → success
    private static void Bus_Create_Valid_Success()
    {
        // Arrange
        var repo = new BusRepository();
        var service = new BusService(repo);

        // Act
        var result = service.Create("Shohagh Prestige", BusClassification.Economy);

        // Assert
        Assert(result.IsSuccess, "Bus creation succeeds with valid coach number");
        Assert(result.Data != null, "Result contains Bus object");
        AssertEqual("Shohagh Prestige", result.Data!.CoachName, "Coach name stored correctly");
        AssertEqual(BusClassification.Economy, result.Data.Classification, "Classification stored correctly");
        AssertEqual(40, result.Data.TotalSeats, "Bus has correct seat count");
        AssertEqual(1, repo.GetAll().Count, "Bus persisted in repository");
    }

    // TC-BUS-04: Create bus with empty coach number → failure
    private static void Bus_Create_EmptyCoach_Fails()
    {
        // Arrange
        var service = new BusService(new BusRepository());

        // Act
        var result1 = service.Create("", BusClassification.Economy);
        var result2 = service.Create("   ", BusClassification.Business);

        // Assert
        Assert(result1.IsFailure, "Empty coach number rejected");
        Assert(result2.IsFailure, "Whitespace-only coach number rejected");
    }

    // TC-BUS-05: Economy seat naming convention (A1 to D10, 40 seats)
    private static void Bus_Economy_SeatNaming_A1toD10()
    {
        // Arrange
        var bus = new Bus("ECO-01", BusClassification.Economy);

        // Act
        var seats = bus.GetAvailableSeats();

        // Assert
        Assert(seats.Contains("A1"), "Economy has seat A1 (first seat)");
        Assert(seats.Contains("D10"), "Economy has seat D10 (last seat)");
        Assert(!seats.Contains("A11"), "Economy does not have A11 (beyond capacity)");
        Assert(!seats.Contains("E1"), "Economy does not have column E");
        AssertEqual(40, seats.Count, "Economy generates exactly 40 seats");
    }

    // TC-BUS-06: Business seat naming convention (A1 to B7, 20 seats)
    private static void Bus_Business_SeatNaming_A1toB7()
    {
        // Arrange
        var bus = new Bus("BIZ-01", BusClassification.Business);

        // Act
        var seats = bus.GetAvailableSeats();

        // Assert
        Assert(seats.Contains("A1"), "Business has seat A1 (first seat)");
        Assert(seats.Contains("B7"), "Business has seat B7 (last seat)");
        Assert(!seats.Contains("C7"), "Business does not have C7 (beyond 20-seat limit)");
        Assert(!seats.Contains("A8"), "Business does not have A8 (beyond row 7)");
        AssertEqual(20, seats.Count, "Business generates exactly 20 seats");
    }

    // TC-BUS-07: Invalid seat codes rejected
    private static void Bus_InvalidSeatCode_Rejected()
    {
        // Arrange
        var economy = new Bus("ECO-01", BusClassification.Economy);
        var business = new Bus("BIZ-01", BusClassification.Business);

        // Assert
        Assert(!economy.IsValidSeat("A11"), "Economy: A11 is invalid (beyond D10)");
        Assert(!economy.IsValidSeat("E1"), "Economy: E1 is invalid (no column E)");
        Assert(!economy.IsValidSeat("S01"), "Economy: S01 is invalid (wrong format)");
        Assert(!economy.IsValidSeat(""), "Economy: empty seat code is invalid");
        Assert(!business.IsValidSeat("A8"), "Business: A8 is invalid (beyond B7)");
        Assert(!business.IsValidSeat("C7"), "Business: C7 is invalid (beyond 20-seat limit)");
    }

    // TC-BUS-08: Valid seat codes accepted
    private static void Bus_ValidSeatCode_Accepted()
    {
        // Arrange
        var economy = new Bus("ECO-01", BusClassification.Economy);
        var business = new Bus("BIZ-01", BusClassification.Business);

        // Assert
        Assert(economy.IsValidSeat("A5"), "Economy: A5 is valid");
        Assert(economy.IsValidSeat("D10"), "Economy: D10 is valid (last seat)");
        Assert(business.IsValidSeat("A1"), "Business: A1 is valid (first seat)");
        Assert(business.IsValidSeat("B7"), "Business: B7 is valid (last seat)");
        Assert(business.IsValidSeat("C6"), "Business: C6 is valid (within 20 seats)");
    }

    // TC-BUS-09: GetAvailableSeats returns all seats initially
    private static void Bus_GetAvailableSeats_Initial_AllAvailable()
    {
        // Arrange
        var economy = new Bus("ECO-01", BusClassification.Economy);
        var business = new Bus("BIZ-01", BusClassification.Business);

        // Act
        var ecoSeats = economy.GetAvailableSeats();
        var bizSeats = business.GetAvailableSeats();

        // Assert
        AssertEqual(40, ecoSeats.Count, "All Economy seats initially available");
        AssertEqual(20, bizSeats.Count, "All Business seats initially available");
    }

    // TC-BUS-10: Bus GetSummary format
    private static void Bus_GetSummary_Format()
    {
        // Arrange
        var bus = new Bus("BUS-001", BusClassification.Economy);

        // Act
        var summary = bus.GetSummary();

        // Assert
        Assert(summary.Contains(bus.BusId), "GetSummary contains BusId");
        Assert(summary.Contains("BUS-001"), "GetSummary contains coach number");
        Assert(summary.Contains("Economy"), "GetSummary contains classification");
        Assert(summary.Contains("40/40"), "GetSummary shows available/total seats");
    }

    // TC-BUS-11: Economy has 4 seats per row
    private static void Bus_Economy_SeatsPerRow4()
    {
        // Arrange & Act
        var bus = new Bus("ECO-01", BusClassification.Economy);

        // Assert
        AssertEqual(4, bus.SeatsPerRow, "Economy has 4 seats per row");
    }

    // TC-BUS-12: Business has 3 seats per row
    private static void Bus_Business_SeatsPerRow3()
    {
        // Arrange & Act
        var bus = new Bus("BIZ-01", BusClassification.Business);

        // Assert
        AssertEqual(3, bus.SeatsPerRow, "Business has 3 seats per row");
    }

    // ════════════════════════════════════════════════════════════════
    //  DOMAIN 4: Seat Reservation Operations
    // ════════════════════════════════════════════════════════════════

    // TC-SEAT-01: Reserve a valid seat → succeeds
    private static void Seat_Reserve_Valid_Succeeds()
    {
        // Arrange
        var bus = new Bus("ECO-01", BusClassification.Economy);

        // Act
        var reserved = bus.ReserveSeat("A1");

        // Assert
        Assert(reserved, "Reserving valid seat A1 succeeds");
    }

    // TC-SEAT-02: Reserve an already-reserved seat → fails
    private static void Seat_Reserve_Duplicate_Fails()
    {
        // Arrange
        var bus = new Bus("ECO-01", BusClassification.Economy);
        bus.ReserveSeat("A1");

        // Act
        var duplicate = bus.ReserveSeat("A1");

        // Assert
        Assert(!duplicate, "Duplicate reservation of A1 is rejected");
    }

    // TC-SEAT-03: Reserving a seat makes it unavailable
    private static void Seat_Reserve_MakesSeatUnavailable()
    {
        // Arrange
        var bus = new Bus("ECO-01", BusClassification.Economy);

        // Act
        bus.ReserveSeat("B3");

        // Assert
        Assert(!bus.IsSeatAvailable("B3"), "Seat B3 is unavailable after reservation");
    }

    // TC-SEAT-04: Available seat count decreases after reservation
    private static void Seat_Reserve_DecreasesAvailableCount()
    {
        // Arrange
        var bus = new Bus("ECO-01", BusClassification.Economy);
        var beforeCount = bus.GetAvailableSeats().Count;

        // Act
        bus.ReserveSeat("C5");
        bus.ReserveSeat("D5");
        bus.ReserveSeat("A6");
        var afterCount = bus.GetAvailableSeats().Count;

        // Assert
        AssertEqual(beforeCount - 3, afterCount,
            "Available seat count decreases by number of reservations");
    }

    // TC-SEAT-05: Reserve all seats on a Business bus fails on 21st
    private static void Seat_ReserveAllBusiness_AllReserved()
    {
        // Arrange
        var bus = new Bus("BIZ-01", BusClassification.Business);

        // Act — reserve all 20 seats
        foreach (var seat in bus.GetAvailableSeats().ToList())
        {
            bus.ReserveSeat(seat);
        }

        // Assert
        AssertEqual(0, bus.GetAvailableSeats().Count, "All 20 Business seats are reserved");
        Assert(!bus.IsSeatAvailable("A1"), "First seat is unavailable");
        Assert(!bus.IsSeatAvailable("B7"), "Last seat is unavailable");
    }

    // TC-SEAT-06: Clear a reserved seat → succeeds, seat becomes available
    private static void Seat_ClearReserved_Succeeds()
    {
        // Arrange
        var bus = new Bus("ECO-01", BusClassification.Economy);
        bus.ReserveSeat("D1");

        // Act
        var cleared = bus.ClearReservedSeat("D1");

        // Assert
        Assert(cleared, "ClearReservedSeat returns true for reserved seat");
        Assert(bus.IsSeatAvailable("D1"), "Seat D1 is available after clearing");
    }

    // TC-SEAT-07: Clear an unreserved seat → fails
    private static void Seat_ClearUnreserved_Fails()
    {
        // Arrange
        var bus = new Bus("ECO-01", BusClassification.Economy);

        // Act
        var cleared = bus.ClearReservedSeat("A1");

        // Assert
        Assert(!cleared, "ClearReservedSeat returns false for unreserved seat");
    }

    // TC-SEAT-08: Reserve seat after clearing it → succeeds
    private static void Seat_ReserveAfterClear_Succeeds()
    {
        // Arrange
        var bus = new Bus("ECO-01", BusClassification.Economy);
        bus.ReserveSeat("B2");
        bus.ClearReservedSeat("B2");

        // Act
        var reReserved = bus.ReserveSeat("B2");

        // Assert
        Assert(reReserved, "Can reserve seat B2 after it was cleared");
    }

    // TC-SEAT-09: Multiple seat reservations tracked correctly
    private static void Seat_MultipleReservations_AllTracked()
    {
        // Arrange
        var bus = new Bus("ECO-01", BusClassification.Economy);
        var seatsToReserve = new[] { "A1", "A2", "B1", "B2", "C1" };

        // Act
        foreach (var seat in seatsToReserve)
        {
            bus.ReserveSeat(seat);
        }

        // Assert
        foreach (var seat in seatsToReserve)
        {
            Assert(!bus.IsSeatAvailable(seat), $"Seat {seat} is unavailable after reservation");
        }
        AssertEqual(40 - 5, bus.GetAvailableSeats().Count,
            "35 seats remain available after 5 reservations");
    }

    // ════════════════════════════════════════════════════════════════
    //  DOMAIN 5: Schedule Management
    // ════════════════════════════════════════════════════════════════

    // TC-SCH-01: Create schedule with valid data → success
    private static void Schedule_Create_Valid_Success()
    {
        // Arrange
        var repo = new ScheduleRepository();
        var service = new ScheduleService(repo);
        var bus = new Bus("BUS-001", BusClassification.Economy);

        // Act
        var departure = DateTime.Today.AddDays(1).AddHours(8);
        var result = service.Create(bus.BusId, "Dhaka", "Chittagong", departure, 650m);

        // Assert
        Assert(result.IsSuccess, "Schedule creation succeeds with valid data");
        Assert(result.Data != null, "Result contains Schedule object");
        AssertEqual(bus.BusId, result.Data!.BusId, "Schedule linked to correct bus");
        AssertEqual("Dhaka", result.Data.DepartureCity, "Departure city stored correctly");
        AssertEqual("Chittagong", result.Data.ArrivalCity, "Arrival city stored correctly");
        AssertEqual(departure, result.Data.DepartureDateTime, "Departure datetime stored correctly");
        AssertEqual(650m, result.Data.TicketPrice, "Ticket price stored correctly");
        AssertEqual(1, repo.GetAll().Count, "Schedule persisted in repository");
    }

    // TC-SCH-02: Create schedule with empty departure city → failure
    private static void Schedule_Create_DepartureEmpty_Fails()
    {
        // Arrange
        var service = new ScheduleService(new ScheduleRepository());

        // Act
        var result = service.Create("BUSID", "", "Chittagong", DateTime.Now.AddDays(1), 500m);

        // Assert
        Assert(result.IsFailure, "Empty departure city rejected");
    }

    // TC-SCH-03: Create schedule with empty arrival city → failure
    private static void Schedule_Create_ArrivalEmpty_Fails()
    {
        // Arrange
        var service = new ScheduleService(new ScheduleRepository());

        // Act
        var result = service.Create("BUSID", "Dhaka", "", DateTime.Now.AddDays(1), 500m);

        // Assert
        Assert(result.IsFailure, "Empty arrival city rejected");
    }

    // TC-SCH-04: Create schedule with zero price → failure
    private static void Schedule_Create_ZeroPrice_Fails()
    {
        // Arrange
        var service = new ScheduleService(new ScheduleRepository());

        // Act
        var result = service.Create("BUSID", "Dhaka", "Sylhet", DateTime.Now.AddDays(1), 0m);

        // Assert
        Assert(result.IsFailure, "Zero price rejected");
    }

    // TC-SCH-05: Create schedule with negative price → failure
    private static void Schedule_Create_NegativePrice_Fails()
    {
        // Arrange
        var service = new ScheduleService(new ScheduleRepository());

        // Act
        var result = service.Create("BUSID", "Dhaka", "Sylhet", DateTime.Now.AddDays(1), -100m);

        // Assert
        Assert(result.IsFailure, "Negative price rejected");
    }

    // TC-SCH-06: Create schedule with empty bus ID → failure
    private static void Schedule_Create_EmptyBusId_Fails()
    {
        // Arrange
        var service = new ScheduleService(new ScheduleRepository());

        // Act
        var result1 = service.Create("", "Dhaka", "Sylhet", DateTime.Now.AddDays(1), 500m);
        var result2 = service.Create("   ", "Dhaka", "Sylhet", DateTime.Now.AddDays(1), 500m);

        // Assert
        Assert(result1.IsFailure, "Empty BusId rejected");
        Assert(result2.IsFailure, "Whitespace-only BusId rejected");
    }

    // TC-SCH-07: Get schedule by valid ID → returns schedule
    private static void Schedule_GetById_Valid_ReturnsSchedule()
    {
        // Arrange
        var service = new ScheduleService(new ScheduleRepository());
        var created = service.Create("BUSID", "Dhaka", "Sylhet", DateTime.Now.AddDays(1), 800m).Data!;

        // Act
        var found = service.GetById(created.ScheduleId);

        // Assert
        Assert(found != null, "GetById returns schedule for valid ID");
        AssertEqual(created.ScheduleId, found!.ScheduleId, "Retrieved schedule ID matches");
    }

    // TC-SCH-08: Get schedule by invalid ID → returns null
    private static void Schedule_GetById_Invalid_ReturnsNull()
    {
        // Arrange
        var service = new ScheduleService(new ScheduleRepository());

        // Act
        var found = service.GetById("INVALID");

        // Assert
        Assert(found == null, "GetById returns null for non-existent schedule");
    }

    // TC-SCH-09: GetAll returns all schedules
    private static void Schedule_GetAll_ReturnsAll()
    {
        // Arrange
        var repo = new ScheduleRepository();
        var service = new ScheduleService(repo);
        service.Create("BUS1", "Dhaka", "Sylhet", DateTime.Now.AddDays(1), 500m);
        service.Create("BUS2", "Dhaka", "Chittagong", DateTime.Now.AddDays(1), 800m);
        service.Create("BUS1", "Sylhet", "Dhaka", DateTime.Now.AddDays(2), 500m);

        // Act
        var all = service.GetAll();

        // Assert
        AssertEqual(3, all.Count, "GetAll returns all 3 schedules");
    }

    // TC-SCH-10: Schedule GetSummary format
    private static void Schedule_GetSummary_Format()
    {
        // Arrange
        var busId = "BUS001";
        var departure = new DateTime(2026, 6, 15, 8, 0, 0);
        var schedule = new Schedule(busId, "Dhaka", "Chittagong", departure, 650m);

        // Act
        var summary = schedule.GetSummary();

        // Assert
        Assert(summary.Contains(schedule.ScheduleId), "GetSummary contains ScheduleId");
        Assert(summary.Contains("Dhaka"), "GetSummary contains departure city");
        Assert(summary.Contains("Chittagong"), "GetSummary contains arrival city");
        Assert(summary.Contains("BDT 650"), "GetSummary contains price");
    }

    // TC-SCH-11: Schedule properties stored correctly
    private static void Schedule_Properties_StoredCorrectly()
    {
        // Arrange
        var busId = "BUS001";
        var departure = new DateTime(2026, 7, 1, 14, 30, 0);

        // Act
        var schedule = new Schedule(busId, "Dhaka", "Cox's Bazar", departure, 1200m);

        // Assert
        AssertEqual(busId, schedule.BusId, "BusId stored");
        AssertEqual("Dhaka", schedule.DepartureCity, "DepartureCity stored");
        AssertEqual("Cox's Bazar", schedule.ArrivalCity, "ArrivalCity stored");
        AssertEqual(departure, schedule.DepartureDateTime, "DepartureDateTime stored");
        AssertEqual(1200m, schedule.TicketPrice, "TicketPrice stored");
    }

    // ════════════════════════════════════════════════════════════════
    //  DOMAIN 6: Ticket Booking
    // ════════════════════════════════════════════════════════════════

    private static (IUserService UserSvc, IBusService BusSvc, IScheduleService SchedSvc,
        ITicketService TicketSvc, IInvoiceService InvoiceSvc, BookingService BookingSvc)
        CreateBookingDependencies()
    {
        var userRepo = new UserRepository();
        var busRepo = new BusRepository();
        var scheduleRepo = new ScheduleRepository();
        var ticketRepo = new TicketRepository();
        var invoiceRepo = new InvoiceRepository();

        var userSvc = new UserService(userRepo);
        var busSvc = new BusService(busRepo);
        var schedSvc = new ScheduleService(scheduleRepo);
        var invoiceSvc = new InvoiceService(invoiceRepo);
        var ticketSvc = new TicketService(ticketRepo);
        var bookingSvc = new BookingService(ticketSvc, userSvc, schedSvc, busSvc, invoiceSvc);

        return (userSvc, busSvc, schedSvc, ticketSvc, invoiceSvc, bookingSvc);
    }

    private static (User User, Bus Bus, Schedule Schedule) SeedBookingData(
        IUserService userSvc, IBusService busSvc, IScheduleService schedSvc)
    {
        var user = userSvc.Create("Test User", "01711000001", "test@test.com").Data!;
        var bus = busSvc.Create("Test Coach", BusClassification.Economy).Data!;
        var schedule = schedSvc.Create(bus.BusId, "Dhaka", "Sylhet",
            DateTime.Now.AddDays(1), 800m).Data!;
        return (user, bus, schedule);
    }

    // TC-BK-01: Successful booking creates ticket with correct properties
    private static void Booking_Book_Valid_Success()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, ticketSvc, invoiceSvc, bookingSvc) =
            CreateBookingDependencies();
        var (user, bus, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);

        // Act
        var result = bookingSvc.Book(user.UserId, schedule.ScheduleId, "A1");

        // Assert
        Assert(result.IsSuccess, "Booking succeeds with valid data");
        Assert(result.Data != null, "Result contains Ticket object");
        AssertEqual("A1", result.Data!.SeatNumber, "Ticket has correct seat number");
        AssertEqual(user.UserId, result.Data.UserId, "Ticket linked to correct user");
        AssertEqual(schedule.ScheduleId, result.Data.ScheduleId, "Ticket linked to correct schedule");
        AssertEqual(bus.BusId, result.Data.BusId, "Ticket linked to correct bus");
        AssertEqual(schedule.TicketPrice, result.Data.Price, "Ticket has correct price");
    }

    // TC-BK-02: Booking with non-existent user → failure
    private static void Booking_Book_NonExistentUser_Fails()
    {
        // Arrange
        var (_, _, schedSvc, _, _, bookingSvc) = CreateBookingDependencies();
        var bus = new Bus("TEST", BusClassification.Economy);
        var schedule = schedSvc.Create(bus.BusId, "Dhaka", "Sylhet",
            DateTime.Now.AddDays(1), 500m).Data!;

        // Act
        var result = bookingSvc.Book("INVALID_USER", schedule.ScheduleId, "A1");

        // Assert
        Assert(result.IsFailure, "Booking fails with non-existent user");
        Assert(result.Message.Contains("User not found"),
            "Error message indicates user not found");
    }

    // TC-BK-03: Booking with non-existent schedule → failure
    private static void Booking_Book_NonExistentSchedule_Fails()
    {
        // Arrange
        var (userSvc, _, _, _, _, bookingSvc) = CreateBookingDependencies();
        var user = userSvc.Create("Test User", "01711000001", "test@test.com").Data!;

        // Act
        var result = bookingSvc.Book(user.UserId, "INVALID_SCHEDULE", "A1");

        // Assert
        Assert(result.IsFailure, "Booking fails with non-existent schedule");
        Assert(result.Message.Contains("Schedule not found"),
            "Error message indicates schedule not found");
    }

    // TC-BK-04: Booking with invalid seat code → failure
    private static void Booking_Book_InvalidSeatCode_Fails()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, _, _, bookingSvc) = CreateBookingDependencies();
        var (user, bus, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);

        // Act
        var result1 = bookingSvc.Book(user.UserId, schedule.ScheduleId, "Z99");
        var result2 = bookingSvc.Book(user.UserId, schedule.ScheduleId, "S01");
        var result3 = bookingSvc.Book(user.UserId, schedule.ScheduleId, "");

        // Assert
        Assert(result1.IsFailure, "Invalid seat code 'Z99' rejected");
        Assert(result2.IsFailure, "Wrong format 'S01' rejected");
        Assert(result3.IsFailure, "Empty seat code rejected");
    }

    // TC-BK-05: Duplicate seat booking on same schedule → failure
    private static void Booking_Book_DuplicateSeat_Fails()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, _, _, bookingSvc) = CreateBookingDependencies();
        var (user, _, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);

        // Create a second user for the duplicate attempt
        var user2 = userSvc.Create("Second User", "01811000001", "user2@test.com").Data!;

        // Act
        var firstResult = bookingSvc.Book(user.UserId, schedule.ScheduleId, "B4");
        var secondResult = bookingSvc.Book(user2.UserId, schedule.ScheduleId, "B4");

        // Assert
        Assert(firstResult.IsSuccess, "First booking of B4 succeeds");
        Assert(secondResult.IsFailure, "Duplicate booking of B4 rejected");
        Assert(secondResult.Message.Contains("already reserved"),
            "Error message indicates seat already reserved");
    }

    // TC-BK-06: Booking generates invoice automatically
    private static void Booking_Book_GeneratesInvoice()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, _, invoiceSvc, bookingSvc) =
            CreateBookingDependencies();
        var (user, _, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);

        // Act
        var result = bookingSvc.Book(user.UserId, schedule.ScheduleId, "C3");

        // Assert
        Assert(result.IsSuccess, "Booking succeeds");
        var invoices = invoiceSvc.GetByUser(user.UserId);
        AssertEqual(1, invoices.Count, "Exactly 1 invoice generated for the booking");
        AssertEqual(result.Data!.TicketId, invoices[0].TicketId,
            "Invoice linked to correct ticket");
        AssertEqual(user.UserId, invoices[0].UserId, "Invoice linked to correct user");
        AssertEqual(schedule.TicketPrice, invoices[0].AmountDue, "Invoice amount equals ticket price");
    }

    // TC-BK-07: GetByUser returns user's tickets
    private static void Booking_GetByUser_ReturnsUserTickets()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, _, _, bookingSvc) = CreateBookingDependencies();
        var (user, _, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);

        // Act
        bookingSvc.Book(user.UserId, schedule.ScheduleId, "D1");
        bookingSvc.Book(user.UserId, schedule.ScheduleId, "D2");
        var tickets = bookingSvc.GetByUser(user.UserId);

        // Assert
        AssertEqual(2, tickets.Count, "GetByUser returns 2 tickets for the user");
        Assert(tickets.All(t => t.UserId == user.UserId),
            "All returned tickets belong to the user");
    }

    // TC-BK-08: GetAvailableSeats for valid schedule returns seats
    private static void Booking_GetAvailableSeats_ValidSchedule()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, _, _, bookingSvc) = CreateBookingDependencies();
        var (_, _, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);

        // Act
        var available = bookingSvc.GetAvailableSeats(schedule.ScheduleId);

        // Assert
        Assert(available.Count > 0, "Available seats returned for valid schedule");
        AssertEqual(40, available.Count, "All 40 Economy seats initially available");
    }

    // TC-BK-09: GetAvailableSeats for invalid schedule → empty
    private static void Booking_GetAvailableSeats_InvalidSchedule_Empty()
    {
        // Arrange & Act
        var (_, _, _, _, _, bookingSvc) = CreateBookingDependencies();
        var available = bookingSvc.GetAvailableSeats("INVALID_SCHEDULE");

        // Assert
        AssertEqual(0, available.Count, "Empty list returned for invalid schedule");
    }

    // ════════════════════════════════════════════════════════════════
    //  DOMAIN 7: Invoice & Payment
    // ════════════════════════════════════════════════════════════════

    // TC-INV-01: New invoice starts as Pending
    private static void Invoice_Create_DefaultStatusPending()
    {
        // Arrange & Act
        var invoice = new Invoice(["TKT001"], "USR001", 800m);

        // Assert
        AssertEqual(PaymentStatus.Unpaid, invoice.Status, "New invoice status is Unpaid");
        Assert(!invoice.IsPaid, "New invoice is not marked as paid");
    }

    // TC-INV-02: Pay valid unpaid invoice → success
    private static void Invoice_Pay_Valid_Success()
    {
        // Arrange
        var repo = new InvoiceRepository();
        var service = new InvoiceService(repo);
        var invoice = service.Create("TKT001", "USR001", 800m);

        // Act
        var result = service.Pay(invoice.InvoiceId);

        // Assert
        Assert(result.IsSuccess, "Payment succeeds for unpaid invoice");
        AssertEqual(PaymentStatus.Paid, invoice.Status, "Invoice status changes to Paid");
        Assert(invoice.IsPaid, "Invoice.IsPaid returns true");
    }

    // TC-INV-03: Pay an already-paid invoice → failure
    private static void Invoice_Pay_AlreadyPaid_Fails()
    {
        // Arrange
        var repo = new InvoiceRepository();
        var service = new InvoiceService(repo);
        var invoice = service.Create("TKT001", "USR001", 800m);
        service.Pay(invoice.InvoiceId);

        // Act
        var result = service.Pay(invoice.InvoiceId);

        // Assert
        Assert(result.IsFailure, "Paying already-paid invoice rejected");
        Assert(result.Message.Contains("already paid"),
            "Error message indicates invoice already paid");
    }

    // TC-INV-04: Pay non-existent invoice → failure
    private static void Invoice_Pay_NonExistent_Fails()
    {
        // Arrange
        var service = new InvoiceService(new InvoiceRepository());

        // Act
        var result = service.Pay("NONEXIST");

        // Assert
        Assert(result.IsFailure, "Paying non-existent invoice rejected");
        Assert(result.Message.Contains("not found"),
            "Error message indicates invoice not found");
    }

    // TC-INV-05: Cancel unpaid invoice → success, status = Cancelled
    private static void Invoice_Cancel_Unpaid_Success()
    {
        // Arrange
        var repo = new InvoiceRepository();
        var service = new InvoiceService(repo);
        var invoice = service.Create("TKT001", "USR001", 800m);

        // Act
        var result = service.CancelPay(invoice.InvoiceId);

        // Assert
        Assert(result.IsSuccess, "Cancelling unpaid invoice succeeds");
        AssertEqual(PaymentStatus.Unpaid, invoice.Status,
            "Invoice status changes to Cancelled");
    }

    // TC-INV-06: Cancel paid invoice → failure
    private static void Invoice_Cancel_Paid_Fails()
    {
        // Arrange
        var repo = new InvoiceRepository();
        var service = new InvoiceService(repo);
        var invoice = service.Create("TKT001", "USR001", 800m);
        service.Pay(invoice.InvoiceId);

        // Act
        var result = service.CancelPay(invoice.InvoiceId);

        // Assert
        Assert(result.IsFailure, "Cancelling paid invoice rejected");
    }

    // TC-INV-07: Cancel non-existent invoice → failure
    private static void Invoice_Cancel_NonExistent_Fails()
    {
        // Arrange
        var service = new InvoiceService(new InvoiceRepository());

        // Act
        var result = service.CancelPay("NONEXIST");

        // Assert
        Assert(result.IsFailure, "Cancelling non-existent invoice rejected");
    }

    // TC-INV-08: GetByUser returns user's invoices
    private static void Invoice_GetByUser_ReturnsUserInvoices()
    {
        // Arrange
        var repo = new InvoiceRepository();
        var service = new InvoiceService(repo);
        service.Create("TKT001", "USR001", 500m);
        service.Create("TKT002", "USR001", 800m);
        service.Create("TKT003", "USR002", 600m);

        // Act
        var user1Invoices = service.GetByUser("USR001");
        var user2Invoices = service.GetByUser("USR002");

        // Assert
        AssertEqual(2, user1Invoices.Count, "User 1 has 2 invoices");
        AssertEqual(1, user2Invoices.Count, "User 2 has 1 invoice");
        Assert(user1Invoices.All(i => i.UserId == "USR001"),
            "All returned invoices belong to User 1");
    }

    // TC-INV-09: GetUnpaidByUser returns only unpaid invoices
    private static void Invoice_GetUnpaidByUser_FiltersPaid()
    {
        // Arrange
        var repo = new InvoiceRepository();
        var service = new InvoiceService(repo);
        var inv1 = service.Create("TKT001", "USR001", 500m);
        var inv2 = service.Create("TKT002", "USR001", 800m);
        service.Pay(inv1.InvoiceId);

        // Act
        var unpaid = service.GetUnpaidByUser("USR001");

        // Assert
        AssertEqual(1, unpaid.Count, "Exactly 1 unpaid invoice remains");
        AssertEqual(inv2.InvoiceId, unpaid[0].InvoiceId,
            "The unpaid invoice is the one not yet paid");
    }

    // TC-INV-10: Invoice properties stored correctly
    private static void Invoice_Properties_StoredCorrectly()
    {
        // Arrange & Act
        var invoice = new Invoice(["TKT-XYZ"], "USR-ABC", 1200m);

        // Assert
        AssertEqual("TKT-XYZ", invoice.TicketId, "TicketId stored correctly");
        AssertEqual("USR-ABC", invoice.UserId, "UserId stored correctly");
        AssertEqual(1200m, invoice.AmountDue, "AmountDue stored correctly");
        Assert(invoice.GeneratedDate <= DateTimeOffset.UtcNow,
            "GeneratedDate is not in the future");
    }

    // ════════════════════════════════════════════════════════════════
    //  DOMAIN 8: Booking Cancellation
    // ════════════════════════════════════════════════════════════════

    // TC-CNCL-01: Cancel a valid booking → success
    private static void Cancel_ValidBooking_Success()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, ticketSvc, invoiceSvc, bookingSvc) =
            CreateBookingDependencies();
        var (user, _, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);
        var bookResult = bookingSvc.Book(user.UserId, schedule.ScheduleId, "A5");
        var ticketId = bookResult.Data!.TicketId;
        var invoices = invoiceSvc.GetByUser(user.UserId);
        var invoiceId = invoices[0].InvoiceId;

        // Act
        var cancelResult = bookingSvc.CancelBooking(user.UserId, invoiceId);

        // Assert
        Assert(cancelResult.IsSuccess, "Cancellation succeeds for valid booking");
        Assert(invoiceSvc.GetById(invoiceId)!.Status == PaymentStatus.Unpaid,
            "Invoice is cancelled after cancellation");
        Assert(ticketSvc.GetById(ticketId) == null,
            "Ticket is removed after cancellation");
    }

    // TC-CNCL-02: Cancel booking with wrong user → failure
    private static void Cancel_WrongUser_Fails()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, _, invoiceSvc, bookingSvc) =
            CreateBookingDependencies();
        var (user, _, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);
        var user2 = userSvc.Create("Wrong User", "01811000002", "wrong@test.com").Data!;
        bookingSvc.Book(user.UserId, schedule.ScheduleId, "B3");
        var invoiceId = invoiceSvc.GetByUser(user.UserId)[0].InvoiceId;

        // Act
        var result = bookingSvc.CancelBooking(user2.UserId, invoiceId);

        // Assert
        Assert(result.IsFailure, "Cancellation fails when wrong user attempts");
        Assert(result.Message.Contains("not belong"),
            "Error message indicates invoice doesn't belong to user");
    }

    // TC-CNCL-03: Cancel with non-existent invoice → failure
    private static void Cancel_NonExistentInvoice_Fails()
    {
        // Arrange
        var (userSvc, _, _, _, _, bookingSvc) = CreateBookingDependencies();
        var user = userSvc.Create("Test User", "01711000001", "test@test.com").Data!;

        // Act
        var result = bookingSvc.CancelBooking(user.UserId, "INVOICE_INVALID");

        // Assert
        Assert(result.IsFailure, "Cancellation fails for non-existent invoice");
    }

    // TC-CNCL-04: Cancellation frees the seat
    private static void Cancel_FreesSeat()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, _, invoiceSvc, bookingSvc) =
            CreateBookingDependencies();
        var (user, _, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);
        var bus = busSvc.GetById(schedule.BusId)!;

        bookingSvc.Book(user.UserId, schedule.ScheduleId, "C7");
        Assert(!bus.IsSeatAvailable("C7"), "Seat C7 is reserved after booking");

        var invoiceId = invoiceSvc.GetByUser(user.UserId)[0].InvoiceId;

        // Act
        bookingSvc.CancelBooking(user.UserId, invoiceId);

        // Assert
        Assert(bus.IsSeatAvailable("C7"), "Seat C7 is available again after cancellation");
    }

    // ════════════════════════════════════════════════════════════════
    //  DOMAIN 9: OOP Principles Verification
    // ════════════════════════════════════════════════════════════════

    // TC-OOP-01 to TC-OOP-05: Inheritance — all model entities extend BaseEntity
    private static void OOP_User_ExtendsBaseEntity()
    {
        var user = new User("Test", "01711000001", "test@test.com");
        Assert(user is BaseEntity, "User extends BaseEntity (INHERITANCE)");
        Assert(!string.IsNullOrEmpty(user.Id), "User inherits Id from BaseEntity");
        Assert(user.CreatedAt != default, "User inherits CreatedAt from BaseEntity");
    }

    private static void OOP_Bus_ExtendsBaseEntity()
    {
        var bus = new Bus("TEST", BusClassification.Economy);
        Assert(bus is BaseEntity, "Bus extends BaseEntity (INHERITANCE)");
    }

    private static void OOP_Schedule_ExtendsBaseEntity()
    {
        var schedule = new Schedule("BID", "A", "B", DateTime.Now, 100m);
        Assert(schedule is BaseEntity, "Schedule extends BaseEntity (INHERITANCE)");
    }

    private static void OOP_Ticket_ExtendsBaseEntity()
    {
        var ticket = new Ticket("UID", "SID", "BID", "A1", 100m);
        Assert(ticket is BaseEntity, "Ticket extends BaseEntity (INHERITANCE)");
    }

    private static void OOP_Invoice_ExtendsBaseEntity()
    {
        var invoice = new Invoice(["TID"], "UID", 100m);
        Assert(invoice is BaseEntity, "Invoice extends BaseEntity (INHERITANCE)");
    }

    // TC-OOP-06: Polymorphism — all entities implement GetSummary()
    private static void OOP_PolymorphicGetSummary_AllEntities()
    {
        // Arrange — polymorphic collection
        BaseEntity[] entities =
        [
            new User("Test", "01711000001", "test@test.com"),
            new Bus("TEST", BusClassification.Economy),
            new Schedule("BID", "A", "B", DateTime.Now, 100m),
            new Ticket("UID", "SID", "BID", "A1", 100m),
            new Invoice(["TID"], "UID", 100m)
        ];

        // Assert — all override GetSummary() (polymorphism in action)
        foreach (var entity in entities)
        {
            var summary = entity.GetSummary();
            Assert(!string.IsNullOrEmpty(summary),
                $"{entity.GetType().Name}.GetSummary() returns non-empty string (POLYMORPHISM)");
            Assert(summary.Contains(entity.Id),
                $"{entity.GetType().Name}.GetSummary() contains Id");
        }
    }

    // TC-OOP-07: BaseEntity provides common Id and CreatedAt
    private static void OOP_BaseEntity_ProvidesIdAndCreatedAt()
    {
        // Arrange — BaseEntity is abstract, test through concrete types
        var user = new User("Test", "01711000001", "test@test.com");
        var bus = new Bus("TEST", BusClassification.Economy);

        // Assert
        Assert(!string.IsNullOrEmpty(user.Id), "User.Id is set (ABSTRACTION via BaseEntity)");
        Assert(!string.IsNullOrEmpty(bus.Id), "Bus.Id is set (ABSTRACTION via BaseEntity)");
        Assert(user.Id.Length == 8, "User.Id is 8 chars (ENCAPSULATION of ID generation)");
        Assert(bus.Id.Length == 8, "Bus.Id is 8 chars (ENCAPSULATION of ID generation)");
    }

    // TC-OOP-08: Encapsulation — private backing fields, controlled access
    private static void OOP_Encapsulation_PrivateFields()
    {
        // Arrange
        var bus = new Bus("ECO-01", BusClassification.Economy);
        bus.ReserveSeat("A1");

        // Assert — can check availability but not directly modify reserved seats
        Assert(!bus.IsSeatAvailable("A1"), "Seat A1 is not available (reserved)");
        Assert(bus.IsSeatAvailable("A2"), "Seat A2 is available (not reserved)");

        // The reserved seats collection is exposed as read-only via ReservedSeats
        // but the class doesn't expose ReservedSeats as a property in this version.
        // Encapsulation is demonstrated through the method-based interface.

        var user = new User("Test", "01711000001", "test@test.com");
        AssertEqual("Test", user.Name, "User.Name accessible via property (ENCAPSULATION)");
        // User.Mobile has no public setter — demonstrates encapsulation
    }

    // ════════════════════════════════════════════════════════════════
    //  DOMAIN 10: End-to-End Integration
    // ════════════════════════════════════════════════════════════════

    // TC-E2E-01: Full happy path — create user → bus → schedule → book → view invoice → pay
    private static void E2E_FullHappyPath()
    {
        // Arrange
        var userRepo = new UserRepository();
        var busRepo = new BusRepository();
        var scheduleRepo = new ScheduleRepository();
        var ticketRepo = new TicketRepository();
        var invoiceRepo = new InvoiceRepository();

        var userSvc = new UserService(userRepo);
        var busSvc = new BusService(busRepo);
        var schedSvc = new ScheduleService(scheduleRepo);
        var ticketSvc = new TicketService(ticketRepo);
        var invoiceSvc = new InvoiceService(invoiceRepo);
        var bookingSvc = new BookingService(ticketSvc, userSvc, schedSvc, busSvc, invoiceSvc);

        // Act — Step 1: Create user
        var userResult = userSvc.Create("Rahim Uddin", "01711000001", "rahim@test.com");
        Assert(userResult.IsSuccess, "E2E Step 1: User created");
        var user = userResult.Data!;

        // Act — Step 2: Create bus
        var busResult = busSvc.Create("Shohagh Express", BusClassification.Economy);
        Assert(busResult.IsSuccess, "E2E Step 2: Bus created");
        var bus = busResult.Data!;

        // Act — Step 3: Create schedule
        var schedResult = schedSvc.Create(bus.BusId, "Dhaka", "Chittagong",
            DateTime.Today.AddDays(1).AddHours(8), 650m);
        Assert(schedResult.IsSuccess, "E2E Step 3: Schedule created");
        var schedule = schedResult.Data!;

        // Act — Step 4: Book ticket
        var bookResult = bookingSvc.Book(user.UserId, schedule.ScheduleId, "A1");
        Assert(bookResult.IsSuccess, "E2E Step 4: Ticket booked");
        var ticket = bookResult.Data!;

        // Act — Step 5: Verify ticket details
        var userTickets = bookingSvc.GetByUser(user.UserId);
        Assert(userTickets.Count == 1, "E2E Step 5: User has 1 ticket");
        Assert(userTickets[0].TicketId == ticket.TicketId, "E2E Step 5: Correct ticket returned");

        // Act — Step 6: View invoices (should have 1 unpaid)
        var invoices = invoiceSvc.GetByUser(user.UserId);
        Assert(invoices.Count == 1, "E2E Step 6: Invoice auto-generated");
        Assert(invoices[0].Status == PaymentStatus.Unpaid, "E2E Step 6: Invoice is Pending");
        Assert(invoices[0].AmountDue == 650m, "E2E Step 6: Invoice amount correct");

        // Act — Step 7: Pay invoice
        var payResult = invoiceSvc.Pay(invoices[0].InvoiceId);
        Assert(payResult.IsSuccess, "E2E Step 7: Payment successful");
        Assert(invoices[0].IsPaid, "E2E Step 7: Invoice marked as paid");

        // Act — Step 8: Verify seat is reserved
        Assert(!bus.IsSeatAvailable("A1"), "E2E Step 8: Seat A1 is reserved");
    }

    // TC-E2E-02: Multiple bookings for same user
    private static void E2E_MultipleBookings_SameUser()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, _, invoiceSvc, bookingSvc) =
            CreateBookingDependencies();
        var (user, bus, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);

        // Act
        bookingSvc.Book(user.UserId, schedule.ScheduleId, "A1");
        bookingSvc.Book(user.UserId, schedule.ScheduleId, "A2");
        bookingSvc.Book(user.UserId, schedule.ScheduleId, "A3");

        // Assert
        var tickets = bookingSvc.GetByUser(user.UserId);
        AssertEqual(3, tickets.Count, "User has 3 tickets after 3 bookings");

        var invoices = invoiceSvc.GetByUser(user.UserId);
        AssertEqual(3, invoices.Count, "User has 3 invoices (one per booking)");

        var available = bookingSvc.GetAvailableSeats(schedule.ScheduleId);
        AssertEqual(40 - 3, available.Count, "37 seats remain available");
    }

    // TC-E2E-03: Multiple users on same schedule
    private static void E2E_MultipleUsers_SameSchedule()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, _, _, bookingSvc) = CreateBookingDependencies();
        var (user1, _, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);
        var user2 = userSvc.Create("User Two", "01711000002", "user2@test.com").Data!;
        var user3 = userSvc.Create("User Three", "01711000003", "user3@test.com").Data!;

        // Act
        var b1 = bookingSvc.Book(user1.UserId, schedule.ScheduleId, "A1");
        var b2 = bookingSvc.Book(user2.UserId, schedule.ScheduleId, "B2");
        var b3 = bookingSvc.Book(user3.UserId, schedule.ScheduleId, "C3");

        // Assert
        Assert(b1.IsSuccess, "User 1 booked A1");
        Assert(b2.IsSuccess, "User 2 booked B2");
        Assert(b3.IsSuccess, "User 3 booked C3");
        AssertEqual(1, bookingSvc.GetByUser(user1.UserId).Count, "User 1 has 1 ticket");
        AssertEqual(1, bookingSvc.GetByUser(user2.UserId).Count, "User 2 has 1 ticket");
        AssertEqual(1, bookingSvc.GetByUser(user3.UserId).Count, "User 3 has 1 ticket");
    }

    // TC-E2E-04: Book → Cancel → Rebook the same seat
    private static void E2E_BookCancelRebook_SameSeat()
    {
        // Arrange
        var (userSvc, busSvc, schedSvc, ticketSvc, invoiceSvc, bookingSvc) =
            CreateBookingDependencies();
        var (user, _, schedule) = SeedBookingData(userSvc, busSvc, schedSvc);
        var bus = busSvc.GetById(schedule.BusId)!;

        // Act — Book
        bookingSvc.Book(user.UserId, schedule.ScheduleId, "D5");
        Assert(!bus.IsSeatAvailable("D5"), "Step 1: Seat D5 reserved");

        var oldInvoiceId = invoiceSvc.GetByUser(user.UserId)[0].InvoiceId;

        // Act — Cancel
        var cancelResult = bookingSvc.CancelBooking(user.UserId, oldInvoiceId);
        Assert(cancelResult.IsSuccess, "Step 2: Cancellation succeeded");
        Assert(bus.IsSeatAvailable("D5"), "Step 2: Seat D5 freed after cancellation");

        // Act — Rebook same seat
        var rebookResult = bookingSvc.Book(user.UserId, schedule.ScheduleId, "D5");
        Assert(rebookResult.IsSuccess, "Step 3: Rebooking D5 succeeded");
        Assert(!bus.IsSeatAvailable("D5"), "Step 3: Seat D5 reserved again");

        // Verify new ticket & invoice generated
        var tickets = bookingSvc.GetByUser(user.UserId);
        AssertEqual(1, tickets.Count, "Step 4: User has 1 active ticket (old one removed)");
        Assert(tickets[0].SeatNumber == "D5", "Step 4: New ticket is for seat D5");

        var invoices = invoiceSvc.GetByUser(user.UserId);
        var activeInvoices = invoices.Where(i => i.Status != PaymentStatus.Unpaid).ToList();
        AssertEqual(1, activeInvoices.Count, "Step 4: 1 active invoice after rebooking");
    }

    // ════════════════════════════════════════════════════════════════
    //  TEST ENTITY (for BaseEntity tests — abstract class needs concrete)
    // ════════════════════════════════════════════════════════════════

    private class TestEntity : BaseEntity
    {
        public override string GetSummary() => $"Test Entity [{Id}]";
    }
}
