# 🎫 Ticketing Booking API

**🌍 Live Demo (Swagger UI):** [https://ticket-booking-api-lgc6.onrender.com/swagger](https://ticket-booking-api-lgc6.onrender.com/swagger)

A robust, enterprise-ready Event/Concert Ticketing Booking API built with **.NET 10**. This project demonstrates modern ASP.NET Core backend practices including CQRS, Asynchronous Messaging, and Clean Architecture principles.

## 🚀 Tech Stack & Features

- **Framework:** .NET 10 / ASP.NET Core Minimal APIs
- **Database:** PostgreSQL with Entity Framework Core
- **Architecture:** CQRS (Command Query Responsibility Segregation) pattern using [MediatR](https://github.com/jbogard/MediatR)
- **Message Broker:** RabbitMQ via [MassTransit](https://masstransit.io/) for asynchronous ticket booking processing
- **Validation:** FluentValidation
- **Logging:** Serilog
- **API Documentation:** Swagger / OpenAPI (Accessible in Production for portfolio purposes)
- **Containerization:** Docker & Docker Compose

## 📁 Features (Endpoints)

### Concerts
- `GET /api/concerts` - Get all available concerts
- `GET /api/concerts/{id}` - Get concert details by ID
- `POST /api/concerts` - Create a new concert
- `PUT /api/concerts/{id}` - Update a concert
- `DELETE /api/concerts/{id}` - Delete a concert

### Bookings
- `GET /api/bookings/{concertId}` - Get all bookings for a specific concert
- `POST /api/bookings` - Submit a ticket booking request (Processed asynchronously in the background via RabbitMQ Consumer)

## 🛠️ Prerequisites

Before you begin, ensure you have the following installed:
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker & Docker Compose](https://www.docker.com/) (for running PostgreSQL and RabbitMQ locally)

## 🚦 Getting Started (Local Development)

1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd TikectingBooking.Api
   ```

2. **Spin up Infrastructure (Database & Message Broker)**
   Use Docker Compose to start PostgreSQL and RabbitMQ:
   ```bash
   docker-compose up -d
   ```

3. **Run the Application**
   Database migrations will run automatically on startup via `Program.cs`.
   ```bash
   dotnet run
   ```

4. **Test the API**
   Open your browser and navigate to the Swagger UI:
   👉 **`https://localhost:7119/swagger/index.html`** (or the HTTP equivalent).
   
   Alternatively, you can use the provided `TikectingBooking.Api.http` file if you are using VS Code / Visual Studio REST Client.

## 🌐 Production Deployment (Render)

This application is ready to be deployed via Docker or standard Web Service on platforms like [Render](https://render.com/).
- **Database:** Uses managed PostgreSQL on Render.
- **Message Broker:** Uses RabbitMQ hosted via CloudAMQP (Mapped via the `RabbitMqUrl` environment variable).
- **Environment Variables:** Needs `DefaultConnection` and `RabbitMqUrl` to run perfectly.

---
*Built as part of advanced .NET Learning.*
