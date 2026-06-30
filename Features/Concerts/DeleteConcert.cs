using MediatR;
using Microsoft.EntityFrameworkCore;
using TikectingBooking.Api.Database;

namespace TikectingBooking.Api.Features.Concerts;

public static class DeleteConcert
{
    public record Command(Guid Id) : IRequest<IResult>;

    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapDelete("/api/concerts/{id:guid}", async (Guid id, ISender sender) => await sender.Send(new Command(id)));

    public class Handler(AppDbContext db) : IRequestHandler<Command, IResult>
    {
        public async Task<IResult> Handle(Command req, CancellationToken ct)
        {
            if (await db.Tickets.AnyAsync(t => t.ConcertId == req.Id, ct))
                return Results.BadRequest("Tidak bisa menghapus konser karena sudah ada tiket yang dibooking.");

            var affected = await db.Concerts.Where(c => c.Id == req.Id).ExecuteDeleteAsync(ct);
            return affected > 0 ? Results.NoContent() : Results.NotFound();
        }
    }
}
