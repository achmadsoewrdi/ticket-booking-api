using MassTransit;
using Microsoft.EntityFrameworkCore;
using TikectingBooking.Api.Database;
using TikectingBooking.Api.Database.Entities;

namespace TikectingBooking.Api.Features.Bookings;

public class BookingConsumer(AppDbContext db) : IConsumer<BookingSubmitted>
{
    public async Task Consume(ConsumeContext<BookingSubmitted> context)
    {
        var msg = context.Message;
        var rows = await db.Concerts
            .Where(c => c.Id == msg.ConcertId && c.TotalTickets > 0)
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.TotalTickets, c => c.TotalTickets - 1));

        if (rows > 0)
        {
            // Jika berhasil kurangi tiket, catat riwayatnya
            db.Tickets.Add(new Ticket
            {
                Id = Guid.NewGuid(),
                ConcertId = msg.ConcertId,
                CustomerName = msg.CustomerName,
                BookingDate = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }
        else
        {
            // Tiket habis atau konser tidak ada
            Console.WriteLine($"Gagal booking untuk {msg.CustomerName} - Tiket Habis");
        }
    }
}
