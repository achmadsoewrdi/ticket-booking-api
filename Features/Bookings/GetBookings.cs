using MediatR;
using Microsoft.EntityFrameworkCore;
using TikectingBooking.Api.Database;

namespace TikectingBooking.Api.Features.Bookings;

public static class GetBookings
{
    public record Query(Guid ConcertId) : IRequest<List<Response>>;
    public record Response(Guid Id, string CustomerName, DateTime BookingDate);

    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/api/bookings", async (Guid concertId, ISender sender) =>
            Results.Ok(await sender.Send(new Query(concertId))));

    public class Handler(AppDbContext db) : IRequestHandler<Query, List<Response>>
    {
        public async Task<List<Response>> Handle(Query req, CancellationToken ct) =>
            await db.Tickets.Where(t => t.ConcertId == req.ConcertId)
                .Select(t => new Response(t.Id, t.CustomerName, t.BookingDate))
                .ToListAsync(ct);
    }
}
