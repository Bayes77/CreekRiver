using CreekRiver.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// llows our api endpoints to access the database through entity framework core
builder.Services.AddNpgsql<CreekRiverDbContext>(builder.Configuration["CreekRiverDbConnectionString"]);

// set the Json serializer options
builder.Services.Configure<JsonOptions>(Options => 
{
    Options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Swagger UI at the root
});

app.MapGet("/api/campsites", (CreekRiverDbContext db) => 
{
    return db.Campsites.ToList();
});

app.MapGet("/api/campsites/{id}", (CreekRiverDbContext db, int id) => 
{
    return db.Campsites.Include(c => c.CampsiteType).Single(c => c.Id == id);
});

app.MapPost("/api/campsites", (CreekRiverDbContext db, Campsite campsite) => 
{
    db.Campsites.Add(campsite);
    db.SaveChanges();
    return Results.Created($"/api/campsites/{campsite.Id}", campsite);
});

app.MapDelete("/api/campsites/{id}", (CreekRiverDbContext db, int id) => 
{
    Campsite campsite = db.Campsites.SingleOrDefault(campsite => campsite.Id == id);
    if (campsite == null)
    {
        return Results.NotFound();
    }
    db.Campsites.Remove(campsite);
    db.SaveChanges();
    return Results.NoContent();
});

app.MapPut("/api/campsites/{id}", (CreekRiverDbContext db, int id, Campsite campsite) => 
{
    Campsite campsiteToUpdate = db.Campsites.SingleOrDefault(campsite => campsite.Id == id);
    if (campsiteToUpdate == null)
    {
        return Results.NotFound();
    }
    campsiteToUpdate.NickName = campsite.NickName;
    campsiteToUpdate.CampsiteTypeId = campsite.CampsiteTypeId;
    campsiteToUpdate.ImageUrl = campsite.ImageUrl;

    db.SaveChanges();
    return Results.NoContent();
});

app.MapGet("/api/reservations", (CreekRiverDbContext db) => 
{
    return db.Reservations
    .Include(r => r.UserProfile)
    .Include(r => r.Campsite)
    .ThenInclude(c => c.CampsiteType)
    .OrderBy(res => res.CheckinDate)
    .ToList();
});

app.MapPost("/api/reservation", (CreekRiverDbContext db, Reservation newRes) => 
{
    try
    {
    db.Reservations.Add(newRes);
    db.SaveChanges();
    return Results.Created($"/api/resevations/{newRes.Id}", newRes);
    }
    catch (DbUpdateException)
    {
        return Results.BadRequest("Invailid data submitted");
    }
});

app.MapDelete("/api/reservations", (CreekRiverDbContext db, int id) => 
{
    Reservation reservation = db.Reservations.SingleOrDefault(reservation => reservation.Id == id);
    if (reservation == null)
    {
        return Results.NotFound();
    }
    db.Reservations.Remove(reservation);
    db.SaveChanges();
    return Results.NoContent();
});
app.Run();
