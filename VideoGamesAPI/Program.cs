using VideoGamesAPI.Business.Models;
using VideoGamesAPI.Business;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<VideoGamesService>();
builder.Services.AddControllers().AddJsonOptions(options => {  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/videogames/{id}", async (int id, VideoGamesService vg) =>
{
    return await vg.GetVideoGameById(id);
})
.WithName("GetVideoGameById");

app.MapGet("/api/videogames/search", async (string search, VideoGamesService vg) =>
{
    return await vg.GetVideoGames(search);
})
.WithName("SearchVideoGames");

app.MapGet("/api/videogames/upcoming", async (VideoGamesService vg) =>
{
    return await vg.GetUpcomingGames();
})
.WithName("UpcomingGames");

app.Run();
