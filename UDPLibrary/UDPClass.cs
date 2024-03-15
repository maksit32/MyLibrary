using System.Net;
using System.Net.Sockets;
using System.Text;



//Use using construction with UDP
namespace UDPLibrary
{
	public static class UDPClass
	{
		//example:			using UdpClient udpReceiver = new UdpClient(new IPEndPoint(IPAddress.Parse(ipAdress), port));
		public static async Task<string> ReceiveMessageAsync(UdpClient udpReceiver)
		{
			if (udpReceiver is null)
			{
				throw new ArgumentNullException(nameof(udpReceiver));
			}

			if (udpReceiver.Available > 0)
			{
				var message = await udpReceiver.ReceiveAsync();
				return Encoding.UTF8.GetString(message.Buffer);
			}
			return string.Empty;
		}
		public static async Task SendMessageAsync(string ipAdress, int port, string message)
		{
			if (string.IsNullOrWhiteSpace(ipAdress))
			{
				throw new ArgumentException($"\"{nameof(ipAdress)}\" не может быть пустым или содержать только пробел.", nameof(ipAdress));
			}
			if (port <= 0) throw new ArgumentOutOfRangeException("Port can`t be less than 0!");
			if (string.IsNullOrWhiteSpace(message))
			{
				throw new ArgumentException($"\"{nameof(message)}\" не может быть пустым или содержать только пробел.", nameof(message));
			}

			//новый отправитель. Шлет напрямую
			using UdpClient udpSender = new UdpClient();
			await udpSender.SendAsync(Encoding.UTF8.GetBytes(message), new IPEndPoint(IPAddress.Parse(ipAdress), port));
		}
	}
}




//Receive 
/*
		получаем данные
	var result = await udpServer.ReceiveAsync();
		предположим, что отправлена строка, преобразуем байты в строку
	var message = Encoding.UTF8.GetString(result.Buffer);
 */

