using MediatR;
using Microsoft.EntityFrameworkCore;
using TikectingBooking.Api.Database;

namespace TikectingBooking.Api.Features.Concerts;

public static class GetConcertById
{
    public record Query(Guid Id) : IRequest<Response?>;
    public record Response(Guid Id, string Name, DateTime Date, int TotalTickets);

    public static void MapEndpoint(IEndpointRouteBuilder app) => app.MapGet("/api/concerts/{id:guid}", async (Guid id, ISender sender) => await sender.Send(new Query(id)) is { } result ? Results.Ok(result) : Results.NotFound());

    public class Handler(AppDbContext db) : IRequestHandler<Query, Response?>
    {
        public async Task<Response?> Handle(Query req, CancellationToken ct) => await db.Concerts.Where(c => c.Id == req.Id).Select(c => new Response(c.Id, c.Name, c.Date, c.TotalTickets)).FirstOrDefaultAsync(ct);
    }
}