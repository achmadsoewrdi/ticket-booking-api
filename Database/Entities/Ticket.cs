namespace TikectingBooking.Api.Database.Entities;

public class Ticket{
    public Guid Id {get; set;}
    public Guid ConcertId {get; set;}
    public Concert? Concert {get; set;}

    public string CustomerName {get; set;} = string.Empty;
    public DateTime BookingDate {get; set;}
}

