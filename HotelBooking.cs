using System;
using System.Collections.Generic;

class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    
    public User(int userId, string name, string role)
    {
        UserId = userId;
        Name = name;
        Role = role;
    }
    
    public override string ToString()
    {
        return $"{Role}: {Name}";
    }
}

class Admin : User
{
    public Admin(int userId, string name) : base(userId, name, "Admin") {}
    
    public void AddRoom(Hotel hotel, Room room)
    {
        hotel.Rooms.Add(room);
        Console.WriteLine($"Room {room.RoomNumber} added successfully.");
    }
}

class Receptionist : User
{
    public Receptionist(int userId, string name) : base(userId, name, "Receptionist") {}
    
    public Booking BookRoom(Hotel hotel, Customer customer, int roomNumber, string checkIn, string checkOut)
    {
        foreach (var room in hotel.Rooms)
        {
            if (room.RoomNumber == roomNumber && room.IsAvailable)
            {
                var booking = new Booking(customer, room, checkIn, checkOut);
                hotel.Bookings.Add(booking);
                room.IsAvailable = false;
                Console.WriteLine($"Room {roomNumber} booked for {customer.Name}.");
                return booking;
            }
        }
        Console.WriteLine("Room not available!");
        return null;
    }
}

class Customer : User
{
    public Customer(int userId, string name) : base(userId, name, "Customer") {}
}

class Room
{
    public int RoomNumber { get; set; }
    public string RoomType { get; set; }
    public double Price { get; set; }
    public bool IsAvailable { get; set; } = true;
    
    public Room(int roomNumber, string roomType, double price)
    {
        RoomNumber = roomNumber;
        RoomType = roomType;
        Price = price;
    }
    
    public override string ToString()
    {
        return $"Room {RoomNumber}: {RoomType} - ${Price} {(IsAvailable ? "Available" : "Booked")}";
    }
}

class Booking
{
    public Customer Customer { get; set; }
    public Room Room { get; set; }
    public string CheckIn { get; set; }
    public string CheckOut { get; set; }
    public DateTime BookingDate { get; set; }
    public double TotalCost { get; set; }
    
    public Booking(Customer customer, Room room, string checkIn, string checkOut)
    {
        Customer = customer;
        Room = room;
        CheckIn = checkIn;
        CheckOut = checkOut;
        BookingDate = DateTime.Now;
        TotalCost = CalculateCost();
    }
    
    private double CalculateCost()
    {
        DateTime checkInDate = DateTime.Parse(CheckIn);
        DateTime checkOutDate = DateTime.Parse(CheckOut);
        int nights = (checkOutDate - checkInDate).Days;
        return nights * Room.Price;
    }
    
    public override string ToString()
    {
        return $"Booking: {Customer.Name} -> Room {Room.RoomNumber} from {CheckIn} to {CheckOut}, Total Cost: ${TotalCost}";
    }
}

class Hotel
{
    public string Name { get; set; }
    public List<Room> Rooms { get; set; } = new List<Room>();
    public List<Booking> Bookings { get; set; } = new List<Booking>();
    
    public Hotel(string name)
    {
        Name = name;
    }
    
    public void ShowAvailableRooms()
    {
        var availableRooms = Rooms.FindAll(room => room.IsAvailable);
        if (availableRooms.Count > 0)
        {
            foreach (var room in availableRooms)
            {
                Console.WriteLine(room);
            }
        }
        else
        {
            Console.WriteLine("No available rooms.");
        }
    }
    
    public void ShowBookings()
    {
        if (Bookings.Count > 0)
        {
            foreach (var booking in Bookings)
            {
                Console.WriteLine(booking);
            }
        }
        else
        {
            Console.WriteLine("No bookings available.");
        }
    }
}

class Program
{
    static void Main()
    {
        Hotel hotel = new Hotel("Grand Hotel");
        Admin admin = new Admin(1, "Alice");
        Receptionist receptionist = new Receptionist(2, "Bob");
        Customer customer = new Customer(3, "Charlie");
        
        Room room1 = new Room(101, "Single", 100);
        Room room2 = new Room(102, "Double", 150);
        
        admin.AddRoom(hotel, room1);
        admin.AddRoom(hotel, room2);
        
        Console.WriteLine("\nAvailable Rooms:");
        hotel.ShowAvailableRooms();
        
        Booking booking = receptionist.BookRoom(hotel, customer, 101, "2025-02-20", "2025-02-25");
        
        Console.WriteLine("\nAfter Booking:");
        hotel.ShowAvailableRooms();
        
        if (booking != null)
        {
            Console.WriteLine("\nBooking Details:");
            Console.WriteLine(booking);
        }
        
        Console.WriteLine("\nAll Bookings:");
        hotel.ShowBookings();
    }
}