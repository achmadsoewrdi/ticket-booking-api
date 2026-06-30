FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy file .csproj dulu agar Docker bisa nge-cache library (mempercepat build ulang)
COPY ["TikectingBooking.Api.csproj", "./"]
RUN dotnet restore "TikectingBooking.Api.csproj"

# Copy sisa kode dan jalankan proses Publish (Release)
COPY . .
RUN dotnet publish "TikectingBooking.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 👱‍♀️ ponytail: Tahap Runtime (Menggunakan image sangat ringan, SDK dibuang)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Ambil hasil kompilasi dari tahap build di atas
COPY --from=build /app/publish .

# Beritahu server bahwa API kita jalan di port 8080
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

# Jalankan aplikasinya
ENTRYPOINT ["dotnet", "TikectingBooking.Api.dll"]
