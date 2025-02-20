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

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddNpgsql<CreekRiverDBContext>(builder.Configuration["CreekRiverDbConnectionString"]);

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

app.MapGet("/api/campsites", (CreekRiverDBContext db) => 
{
    return db.Campsites.ToList();
});

app.MapGet("/api/campsites/{id}", (CreekRiverDBContext db, int id) => 
{
    return db.Campsites.Include(c => c.CampsiteType).Single(c => c.Id == id);
});

app.MapPost("/api/campsites", (CreekRiverDBContext db, Campsite campsite) => 
{
    db.Campsites.Add(campsite);
    db.SaveChanges();
    return Results.Created($"/api/campsites/{campsite.Id}", campsite);
});

app.MapDelete("/api/campsites/{id}", (CreekRiverDBContext db, int id) => 
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

app.MapPut("/api/campsites/{id}", (CreekRiverDBContext db, int id, Campsite campsite) => 
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
app.Run();
