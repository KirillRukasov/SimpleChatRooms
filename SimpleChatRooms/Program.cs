using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleChatRooms.Data;
using SimpleChatRooms.Hubs;
using SimpleChatRooms.Interfaces;
using SimpleChatRooms.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ISimpleChatRoomsDbContext, SimpleChatRoomsDbContext>(option =>
    option.UseSqlServer(connectionString));

builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapHub<ChatHub>("/chatHub");

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action=Index}/{id?}");

app.Run();
