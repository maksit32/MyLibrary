using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static UDPLibrary.UDPClass;




using UdpClient sender = CreateUpdClient("192.168.56.1", 8080);
Console.WriteLine(sender.Client.RemoteEndPoint);

ReceiveMessagesAsync();
await SendMessageAsync(sender, $"Spam 12312");
await TestSpamMessages();





Console.WriteLine("Press to exit");
Console.ReadLine();




async Task ReceiveMessagesAsync()
{
	//2й - получатель (порт как у отправителя)
	using UdpClient receiver = new UdpClient(8080);
	while (true)
	{
		// получаем данные
		var result = await receiver.ReceiveAsync();
		var message = Encoding.UTF8.GetString(result.Buffer);
		// выводим сообщение
		Console.WriteLine(message);
	}
}
async Task TestSpamMessages()
{

	for (int i = 1; i <= 10; i++)
	{
		await SendMessageAsync(sender, $"Spam #{i}");
	}
}









//2nd part

//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//IPAddress localAddress = IPAddress.Parse("127.0.0.1");
//Console.Write("Введите свое имя: ");
//string? username = Console.ReadLine();
//Console.Write("Введите порт для приема сообщений: ");
//if (!int.TryParse(Console.ReadLine(), out var localPort)) return;
//Console.Write("Введите порт для отправки сообщений: ");
//if (!int.TryParse(Console.ReadLine(), out var remotePort)) return;
//Console.WriteLine();

//// запускаем получение сообщений
//Task.Run(ReceiveMessageAsync);
//// запускаем ввод и отправку сообщений
//await SendMessageAsync();

//// отправка сообщений в группу
//async Task SendMessageAsync()
//{
//	using UdpClient sender = new UdpClient();
//	Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");
//	// отправляем сообщения
//	while (true)
//	{
//		var message = Console.ReadLine(); // сообщение для отправки
//										  // если введена пустая строка, выходим из цикла и завершаем ввод сообщений
//		if (string.IsNullOrWhiteSpace(message)) break;
//		// иначе добавляем к сообщению имя пользователя
//		message = $"{username}: {message}";
//		byte[] data = Encoding.UTF8.GetBytes(message);
//		// и отправляем на 127.0.0.1:remotePort
//		await sender.SendAsync(data, new IPEndPoint(localAddress, remotePort));
//	}
//}
//// отправка сообщений
//async Task ReceiveMessageAsync()
//{
//	using UdpClient receiver = new UdpClient(localPort);
//	while (true)
//	{
//		// получаем данные
//		var result = await receiver.ReceiveAsync();
//		var message = Encoding.UTF8.GetString(result.Buffer);
//		// выводим сообщение
//		Console.WriteLine(message);
//	}
//}