using System.Net;
using System.Net.Sockets;
using System.Text;
using static UDPLibrary.UDPClass;


//***
//using needed!!!!
//***



//192.168.0.13
//using UdpClient udpClient = new UdpClient();
//using UdpClient udpReceiver = new UdpClient(new IPEndPoint(IPAddress.Parse("192.168.0.13"), 11111));

//var helloStr = "Hello";
//var bytes = Encoding.UTF8.GetBytes(helloStr);
//var task = Task.Run(async () =>
//{
//	var message = await udpReceiver.ReceiveAsync();
//	Console.WriteLine(Encoding.UTF8.GetString(message.Buffer));
//});
//await udpClient.SendAsync(bytes, new IPEndPoint(IPAddress.Parse("192.168.0.13"), 11111));
//Task.WaitAny(task);




//получатель(receiver) держит ip + port,
//отправитель(sender) просто шлет напрямую (он пустой)
string ipAdress = "192.168.0.13";
int port = 5555;
string message = "Hello, world!";
using UdpClient udpReceiver = new UdpClient(new IPEndPoint(IPAddress.Parse(ipAdress), port));


await SendMessageAsync(ipAdress, port, message);
string receivedMessage = await ReceiveMessageAsync(udpReceiver);





Console.WriteLine(receivedMessage);
Console.WriteLine("Press to exit");
Console.ReadLine();





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