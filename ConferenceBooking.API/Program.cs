using ConferenceBooking.API.Middlewares;
using ConferenceBooking.BLL.Extensions;
using ConferenceBooking.DAL.Data;
using ConferenceBooking.DAL.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDal(builder.Configuration);
builder.Services.AddBll();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.SeedAsync(dbContext);   
}

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint(
        "/openapi/v1.json", "ConferenceBooking API v1"));   
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();