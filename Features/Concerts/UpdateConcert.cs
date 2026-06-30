using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TikectingBooking.Api.Database;

namespace TikectingBooking.Api.Features.Concerts;

public static class UpdateConcert
{
    public record Command(Guid Id, string Name, DateTime Date, int TotalTickets) : IRequest<IResult>;
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
            RuleFor(x => x.TotalTickets).GreaterThan(0);
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder app) => app.MapPut("/api/concerts/{id:guid}", async (Guid id, Command cmd, ISender sender) => id != cmd.Id ? Results.BadRequest("ID tidak ditemukan") : await sender.Send(cmd));

    public class Handler(AppDbContext db) : IRequestHandler<Command, IResult>
    {
        public async Task<IResult> Handle(Command req, CancellationToken ct)
        {
            var affected = await db.Concerts.Where(c => c.Id == req.Id).ExecuteUpdateAsync(s => s.SetProperty(c => c.Name, req.Name).SetProperty(c => c.Date, req.Date).SetProperty(c => c.TotalTickets, req.TotalTickets), ct);

            return affected > 0 ? Results.NoContent() : Results.NotFound();
        }
    }
}