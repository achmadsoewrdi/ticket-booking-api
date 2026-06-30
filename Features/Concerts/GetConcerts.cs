using MediatR;
using Microsoft.EntityFrameworkCore;
using TikectingBooking.Api.Features.Concerts;
using TikectingBooking.Api.Database;

namespace TikectingBooking.Api.Features.Concerts;

public static class GetConcerts
{
    public record Query() : IRequest<List<Response>>;
    public record Response(Guid Id, string Name, DateTime Date, int TotalTickets);

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/concerts", async (ISender sender) => {
            var result = await sender.Send(new Query());
            return Results.Ok(result);
        });
    }

    public class Handler : IRequestHandler<Query, List<Response>>
    {
        private readonly AppDbContext _context;
        public Handler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var concerts = await _context.Concerts.Select(c => new Response(c.Id, c.Name, c.Date, c.TotalTickets)).ToListAsync(cancellationToken);
            return concerts;
        }
    }
}