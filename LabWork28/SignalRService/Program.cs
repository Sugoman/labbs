using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// 5.1.3 Регистрация SignalR
builder.Services.AddSignalR();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// 5.1.5 Endpoint для ChatHub
app.MapHub<ChatHub>("/chat");

app.MapGet("/", () => "Chat Server is running!");

app.Run();

// 5.1.4 Класс ChatHub
public class ChatHub : Hub
{
    // 5.3.1 Словари для хранения данных о пользователях и комнатах
    private static readonly ConcurrentDictionary<string, string> ConnectionToRoom = new();
    private static readonly ConcurrentDictionary<string, string> ConnectionToUser = new();

    // 5.3.2 Метод для присоединения к комнате
    public async Task JoinRoom(string roomName, string userName)
    {
        var connectionId = Context.ConnectionId;

        ConnectionToRoom[connectionId] = roomName;
        ConnectionToUser[connectionId] = userName;

        await Groups.AddToGroupAsync(connectionId, roomName);

        await Clients.Group(roomName).SendAsync("ReceiveMessage", "System", $"{userName} присоединился к комнате {roomName}");
    }

    // 5.1.4 Метод для отправки сообщений
    public async Task SendMessage(string message)
    {
        var connectionId = Context.ConnectionId;

        // 5.3.3 Проверка наличия пользователя и комнаты
        if (ConnectionToRoom.TryGetValue(connectionId, out var room) &&
            ConnectionToUser.TryGetValue(connectionId, out var user))
        {
            await Clients.Group(room).SendAsync("ReceiveMessage", user, message);
        }
    }

    // 5.3.4 Обработка отключения пользователя
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        if (ConnectionToRoom.TryRemove(connectionId, out var room) &&
            ConnectionToUser.TryRemove(connectionId, out var user))
        {
            await Groups.RemoveFromGroupAsync(connectionId, room);
            await Clients.Group(room).SendAsync("ReceiveMessage", "System", $"{user} покинул комнату {room}");
        }

        await base.OnDisconnectedAsync(exception);
    }
}