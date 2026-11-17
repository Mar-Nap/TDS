using TDS_Parking.Interfaces;
using TDS_Parking.Repository;
using TDS_Parking.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IParkingService, ParkingService>();
builder.Services.AddCors();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IParkingRepository, ParkingRepository>();
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();


app.UseExceptionHandler();
app.MapControllers();
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.Run();

