using MediatR;
using FluentValidation;
using TikectingBooking.Api.Database;
using TikectingBooking.Api.Database.Entities;

namespace TikectingBooking.Api.Features.Concerts;



public static class CreateConcert
{

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nama konser tidak boleh kosong!")
                .MinimumLength(3).WithMessage("Nama konser minimal 3 huruf.");
            RuleFor(x => x.Date)
                .GreaterThan(DateTime.UtcNow).WithMessage("Tanggal konser tidak boleh di masa lalu!");
            RuleFor(x => x.TotalTickets)
                .GreaterThan(0).WithMessage("Total tiket harus lebih dari 0.");
        }
    }
    
    public record Command(string Name, DateTime Date, int TotalTickets) : IRequest<Guid>;
    
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/concerts", async (Command command, ISender sender) => {
            var result = await sender.Send(command);
            return Results.Ok(new {Id = result});
        });
    }

    public class Handler : IRequestHandler<Command, Guid>
    {
        private readonly AppDbContext _context;

        public Handler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var concert = new Concert{
                Id = Guid.NewGuid(),
                Name = request.Name,
                Date = request.Date,
                TotalTickets = request.TotalTickets
            };

            _context.Concerts.Add(concert);
            await _context.SaveChangesAsync();

            return concert.Id;
        }
    }
}