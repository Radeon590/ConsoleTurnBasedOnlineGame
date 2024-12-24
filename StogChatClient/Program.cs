using System.Text;
using Microsoft.AspNetCore.SignalR.Client;

Console.Write("Введите ваше имя: ");
var userName = Console.ReadLine();
var messageBuilder = new StringBuilder();
var messagesList = new List<string>();

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5120/chatHub")
    .Build();

connection.On<string, string>("ReceiveMessage", (user, message) =>
{
    if (user != userName)
    {
        messagesList.Add($"{user}: {message}");
    }
});

try
{
    await connection.StartAsync();
    Console.WriteLine("Подключено к серверу чата.");

    while (true)
    {
        Console.Clear();
        foreach (var m in messagesList)
        {
            Console.WriteLine(m);
        }
        Console.WriteLine("===========================");
        Console.Write("Введите сообщение: ");

        messageBuilder.Clear();
        
        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            
            messageBuilder.Append(key.KeyChar);
        }
        var message = messageBuilder.ToString();
        if (string.IsNullOrWhiteSpace(message)) continue;

        messagesList.Add($"you: {message}");
        await connection.InvokeAsync("SendMessage", userName, message);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка подключения: {ex.Message}");
}
finally
{
    await connection.StopAsync();
    Console.WriteLine("Соединение закрыто.");
}