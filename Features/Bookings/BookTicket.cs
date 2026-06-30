using MassTransit;
using FluentValidation;

namespace TikectingBooking.Api.Features.Bookings;

public static class BookTicket
{
    public record Command(Guid ConcertId, string CustomerName);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.CustomerName).NotEmpty();
            RuleFor(x => x.ConcertId).NotEmpty();
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/api/bookings", async (Command cmd, IPublishEndpoint publishEndpoint) =>
        {
            await publishEndpoint.Publish(new BookingSubmitted(cmd.ConcertId, cmd.CustomerName));

            return Results.Accepted(value: "Booking sedang diproses antrean!");
        });
}
