using HotelAgent.Domain.Entities;
using HotelAgent.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelAgent.Data.Seeding;

public static class HotelSeeder
{
    public static async Task SeedAsync(HotelDbContext context)
    {
        // Idempotency: if already seeded, skip
        if (await context.Rooms.AnyAsync())
        {
            return;
        }

        var rooms = CreateRooms();
        await context.Rooms.AddRangeAsync(rooms);
        await context.SaveChangesAsync();

        var guests = CreateGuests();
        await context.Guests.AddRangeAsync(guests);
        await context.SaveChangesAsync();

        var reservations = CreateReservations(guests, rooms);
        await context.Reservations.AddRangeAsync(reservations);
        await context.SaveChangesAsync();

        var notes = CreateNotes(reservations);
        await context.ReservationNotes.AddRangeAsync(notes);
        await context.SaveChangesAsync();
    }

    private static List<Room> CreateRooms()
    {
        var rooms = new List<Room>();

        // Floor 1: Standard rooms 101-110
        for (int i = 1; i <= 10; i++)
        {
            rooms.Add(new Room
            {
                RoomNumber = $"10{i}",
                Type = RoomType.Standard,
                Status = i % 3 == 0 ? RoomStatus.Dirty : RoomStatus.Inspected,
                BaseRate = 150m,
                MaxOccupancy = 2
            });
        }

        // Floor 2: Deluxe rooms 201-208
        for (int i = 1; i <= 8; i++)
        {
            rooms.Add(new Room
            {
                RoomNumber = $"20{i}",
                Type = RoomType.Deluxe,
                Status = RoomStatus.Inspected,
                BaseRate = 220m,
                MaxOccupancy = 3
            });
        }

        // Floor 3: Suites 301-304
        for (int i = 1; i <= 4; i++)
        {
            rooms.Add(new Room
            {
                RoomNumber = $"30{i}",
                Type = RoomType.Suite,
                Status = i == 2 ? RoomStatus.OutOfOrder : RoomStatus.Inspected,
                BaseRate = 380m,
                MaxOccupancy = 4
            });
        }

        // Penthouse
        rooms.Add(new Room
        {
            RoomNumber = "PH1",
            Type = RoomType.Penthouse,
            Status = RoomStatus.Inspected,
            BaseRate = 850m,
            MaxOccupancy = 6
        });

        return rooms;
    }

    private static List<Guest> CreateGuests()
    {
        return new List<Guest>
        {
            new() { FirstName = "Priya", LastName = "Patel", Email = "priya.patel@example.com",
                Phone = "+1-415-555-0142", IsVip = true,
                Notes = "Prefers quiet rooms, away from elevators. Allergic to feather pillows." },
            new() { FirstName = "Marcus", LastName = "Chen", Email = "m.chen@example.com",
                Phone = "+1-617-555-0193" },
            new() { FirstName = "Sofia", LastName = "Reyes", Email = "sofia.r@example.com",
                Phone = "+1-305-555-0117", IsVip = true,
                Notes = "Anniversary stay regular. Likes champagne on arrival." },
            new() { FirstName = "James", LastName = "O'Brien", Email = "jobrien@example.com",
                Phone = "+1-212-555-0188" },
            new() { FirstName = "Aiko", LastName = "Tanaka", Email = "aiko.t@example.com",
                Phone = "+81-3-5555-0119" },
            new() { FirstName = "Robert", LastName = "Müller", Email = "rmuller@example.com",
                Phone = "+49-30-5555-0144" },
            new() { FirstName = "Fatima", LastName = "Hassan", Email = "f.hassan@example.com",
                Phone = "+1-713-555-0156" },
            new() { FirstName = "David", LastName = "Cohen", Email = "dcohen@example.com",
                Phone = "+1-310-555-0177" },
            new() { FirstName = "Elena", LastName = "Volkov", Email = "elena.v@example.com",
                Phone = "+1-415-555-0124" },
            new() { FirstName = "Thomas", LastName = "Becker", Email = "t.becker@example.com",
                Phone = "+1-202-555-0166" }
        };
    }

    private static List<Reservation> CreateReservations(List<Guest> guests, List<Room> rooms)
    {
        // Use today as the anchor so the seeded data always feels current
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var deluxeRooms = rooms.Where(r => r.Type == RoomType.Deluxe).ToList();
        var standardRooms = rooms.Where(r => r.Type == RoomType.Standard).ToList();
        var suites = rooms.Where(r => r.Type == RoomType.Suite).ToList();

        var reservations = new List<Reservation>
        {
            // Arriving today — Priya, the VIP
            new()
            {
                ConfirmationNumber = "AUR-10001",
                Guest = guests[0],
                Room = deluxeRooms[0],
                RequestedRoomType = RoomType.Deluxe,
                CheckInDate = today,
                CheckOutDate = today.AddDays(3),
                NumberOfGuests = 2,
                RatePerNight = 220m,
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-14)
            },
            // Arriving today — Marcus, standard
            new()
            {
                ConfirmationNumber = "AUR-10002",
                Guest = guests[1],
                Room = standardRooms[0],
                RequestedRoomType = RoomType.Standard,
                CheckInDate = today,
                CheckOutDate = today.AddDays(2),
                NumberOfGuests = 1,
                RatePerNight = 150m,
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-7)
            },
            // Arriving today — Sofia, VIP, suite
            new()
            {
                ConfirmationNumber = "AUR-10003",
                Guest = guests[2],
                Room = suites[0],
                RequestedRoomType = RoomType.Suite,
                CheckInDate = today,
                CheckOutDate = today.AddDays(4),
                NumberOfGuests = 2,
                RatePerNight = 380m,
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            },
            // Arriving tomorrow — James, no room assigned yet
            new()
            {
                ConfirmationNumber = "AUR-10004",
                Guest = guests[3],
                Room = null,
                RequestedRoomType = RoomType.Standard,
                CheckInDate = today.AddDays(1),
                CheckOutDate = today.AddDays(3),
                NumberOfGuests = 2,
                RatePerNight = 150m,
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            // Arriving in 3 days — Aiko
            new()
            {
                ConfirmationNumber = "AUR-10005",
                Guest = guests[4],
                Room = deluxeRooms[1],
                RequestedRoomType = RoomType.Deluxe,
                CheckInDate = today.AddDays(3),
                CheckOutDate = today.AddDays(7),
                NumberOfGuests = 1,
                RatePerNight = 220m,
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-21)
            },
            // Currently in-house — checked in yesterday
            new()
            {
                ConfirmationNumber = "AUR-09998",
                Guest = guests[5],
                Room = deluxeRooms[2],
                RequestedRoomType = RoomType.Deluxe,
                CheckInDate = today.AddDays(-1),
                CheckOutDate = today.AddDays(2),
                NumberOfGuests = 2,
                RatePerNight = 220m,
                Status = ReservationStatus.CheckedIn,
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            // Currently in-house — long stay
            new()
            {
                ConfirmationNumber = "AUR-09995",
                Guest = guests[6],
                Room = standardRooms[2],
                RequestedRoomType = RoomType.Standard,
                CheckInDate = today.AddDays(-3),
                CheckOutDate = today.AddDays(4),
                NumberOfGuests = 1,
                RatePerNight = 150m,
                DiscountAmount = 50m,
                Status = ReservationStatus.CheckedIn,
                CreatedAt = DateTime.UtcNow.AddDays(-20)
            },
            // Departing today
            new()
            {
                ConfirmationNumber = "AUR-09990",
                Guest = guests[7],
                Room = standardRooms[3],
                RequestedRoomType = RoomType.Standard,
                CheckInDate = today.AddDays(-2),
                CheckOutDate = today,
                NumberOfGuests = 2,
                RatePerNight = 150m,
                Status = ReservationStatus.CheckedIn,
                CreatedAt = DateTime.UtcNow.AddDays(-12)
            },
            // Recently completed (history)
            new()
            {
                ConfirmationNumber = "AUR-09850",
                Guest = guests[0], // Priya again — she's a regular
                Room = deluxeRooms[3],
                RequestedRoomType = RoomType.Deluxe,
                CheckInDate = today.AddDays(-30),
                CheckOutDate = today.AddDays(-27),
                NumberOfGuests = 2,
                RatePerNight = 220m,
                Status = ReservationStatus.CheckedOut,
                CreatedAt = DateTime.UtcNow.AddDays(-45)
            },
            // Recently completed (history)
            new()
            {
                ConfirmationNumber = "AUR-09875",
                Guest = guests[2], // Sofia again
                Room = suites[2],
                RequestedRoomType = RoomType.Suite,
                CheckInDate = today.AddDays(-60),
                CheckOutDate = today.AddDays(-55),
                NumberOfGuests = 2,
                RatePerNight = 380m,
                Status = ReservationStatus.CheckedOut,
                CreatedAt = DateTime.UtcNow.AddDays(-75)
            },
            // Cancelled
            new()
            {
                ConfirmationNumber = "AUR-10006",
                Guest = guests[8],
                Room = null,
                RequestedRoomType = RoomType.Suite,
                CheckInDate = today.AddDays(5),
                CheckOutDate = today.AddDays(8),
                NumberOfGuests = 2,
                RatePerNight = 380m,
                Status = ReservationStatus.Cancelled,
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            },
            // Future — Thomas
            new()
            {
                ConfirmationNumber = "AUR-10007",
                Guest = guests[9],
                Room = null,
                RequestedRoomType = RoomType.Penthouse,
                CheckInDate = today.AddDays(10),
                CheckOutDate = today.AddDays(13),
                NumberOfGuests = 4,
                RatePerNight = 850m,
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            }
        };

        return reservations;
    }

    private static List<ReservationNote> CreateNotes(List<Reservation> reservations)
    {
        return new List<ReservationNote>
        {
            new()
            {
                Reservation = reservations[0], // Priya, today
                Content = "VIP guest — confirm welcome amenity is in room before arrival. Allergic to feathers; double-check pillows.",
                AddedBy = "Sarah (Front Desk Manager)",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new()
            {
                Reservation = reservations[2], // Sofia, today
                Content = "Anniversary stay. Champagne and chocolate-covered strawberries arranged for arrival. 7th anniversary at the Aurora.",
                AddedBy = "Marcus (Concierge)",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new()
            {
                Reservation = reservations[6], // Fatima, in-house, has discount
                Content = "Applied $50/night discount per manager approval — guest reported plumbing issue night 1, room 105. Compensation per policy.",
                AddedBy = "Janet (Manager)",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new()
            {
                Reservation = reservations[8], // Priya's previous stay
                Content = "Promised loyalty discount of 10% on next booking — confirmed at checkout. Document: Loyalty-2026-Q1.",
                AddedBy = "Sarah (Front Desk Manager)",
                CreatedAt = DateTime.UtcNow.AddDays(-27)
            }
        };
    }
}