namespace TikectingBooking.Api.Database.Entities;

public class Concert{
    public Guid Id {get; set;}
    public string Name {get; set;} =string.Empty;
    public DateTime Date {get; set;}

    public int TotalTickets {get; set;}

    public uint Version  {get; set;}
}
