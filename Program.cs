using Microsoft.EntityFrameworkCore;
using MassTransit;
using Serilog;
using TikectingBooking.Api.Database;
using TikectingBooking.Api.Features.Concerts;
using TikectingBooking.Api.Features.Bookings;
using FluentValidation;
using TikectingBooking.Api.Behaviors;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.WriteTo.Console().ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// --- INI KODE YANG KITA TAMBAHKAN TADI ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// ----------------------------------------

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddMassTransit(x =>
{
    // Daftarkan consumer kita
    x.AddConsumer<BookingConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqUrl = builder.Configuration["RabbitMqUrl"];

        if (!string.IsNullOrEmpty(rabbitMqUrl))
        {
            // 👱‍♀️ ponytail: Jika ada URL (dari CloudAMQP), gunakan ini
            cfg.Host(new Uri(rabbitMqUrl));
        }
        else
        {
            // Jika tidak ada (sedang run di laptop lokal), gunakan localhost
            cfg.Host("localhost", "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
        }

        cfg.ConfigureEndpoints(context);
    });

});


var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

CreateConcert.MapEndpoint(app);
GetConcerts.MapEndpoint(app);
BookTicket.MapEndpoint(app);
GetConcertById.MapEndpoint(app);
UpdateConcert.MapEndpoint(app);
DeleteConcert.MapEndpoint(app);
GetBookings.MapEndpoint(app);

app.UseHttpsRedirection();

app.Run();
