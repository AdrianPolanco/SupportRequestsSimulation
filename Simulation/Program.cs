using Microsoft.EntityFrameworkCore;
using Simulation.Dtos.Create;
using Simulation.Interfaces;
using Simulation.Persistence.Context;
using Simulation.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString is null)
{
    throw new NullReferenceException("Connection string has no value from user secrets nor environment variables.");
}

builder.Services.AddDbContext<SupportDbContext>(options =>
{
    if(builder.Environment.IsProduction())
    {
        options.UseSqlServer(connectionString);
    }else{
        options.UseNpgsql(connectionString);
    }
});
builder.Services.AddScoped<SupportService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/support", async (SupportService supportService, 
    CreateSupportRequest request, CancellationToken cancellationToken) =>
{
    var result = await supportService.CreateAsync(request, 
        cancellationToken);

    if (!result.IsSuccess)
    {
        return Results.BadRequest(result.Error.Message);
    }

    return Results.Ok(result.Value);
});

app.MapGet("/support", async (SupportService supportService, CancellationToken cancellationToken) =>
{
    var result = await supportService.GetAsync(cancellationToken);

    return Results.Ok(result);
});

app.Run();