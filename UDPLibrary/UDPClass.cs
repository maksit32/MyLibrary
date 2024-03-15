using System.Net;
using System.Net.Sockets;
using System.Text;



//Use using construction with UDP
namespace UDPLibrary
{
	public static class UDPClass
	{
		public static UdpClient CreateUpdClient(int port)
		{
			if (port < 0) throw new ArgumentOutOfRangeException("Port can`t be less than 0!");

			UdpClient udpClient = new UdpClient(port);
			return udpClient;
		}
		public static UdpClient CreateUpdClient(string hostname, int port)
		{
			if (string.IsNullOrWhiteSpace(hostname))
			{
				throw new ArgumentException($"\"{nameof(hostname)}\" не может быть пустым или содержать только пробел.", nameof(hostname));
			}
			if (port < 0) throw new ArgumentOutOfRangeException("Port can`t be less than 0!");

			UdpClient udpClient = new UdpClient(hostname, port);
			return udpClient;
		}
		public static UdpClient CreateUpdClient(int port, AddressFamily family)
		{
			if (port < 0) throw new ArgumentOutOfRangeException("Port can`t be less than 0!");

			UdpClient udpClient = new UdpClient(port, family);
			return udpClient;
		}
		public static UdpClient CreateUpdClient(IPEndPoint localEP)
		{
			if (localEP is null)
			{
				throw new ArgumentNullException(nameof(localEP));
			}

			UdpClient udpClient = new UdpClient(localEP);
			return udpClient;
		}
		public static async Task<string> ReceiveMessageAsync(UdpClient client)
		{
			if (client is null)
			{
				throw new ArgumentNullException(nameof(client));
			}

			if (client.Available > 0)
			{
				// буфер для получения данных
				byte[] responseBytes = new byte[4096];
				// получаем данные
				int bytesCount = await client.Client.ReceiveAsync(responseBytes);
				// преобразуем полученные данные в строку
				string response = Encoding.UTF8.GetString(responseBytes, 0, bytesCount);

				return response;
			}
			return string.Empty;
		}
		public static async Task SendMessageAsync(UdpClient client, string message)
		{
			if (client is null)
			{
				throw new ArgumentNullException(nameof(client));
			}
			if (string.IsNullOrWhiteSpace(message))
			{
				throw new ArgumentException($"\"{nameof(message)}\" не может быть пустым или содержать только пробел.", nameof(message));
			}

			if(client.Client.Connected)
			{
				await client.Client.SendAsync(Encoding.UTF8.GetBytes(message));
			}
		}
		public static async Task DisconnectUdpClient(UdpClient client)
		{
			if (client is null)
			{
				throw new ArgumentNullException(nameof(client));
			}

			if(client.Client.Connected)
			{
				await SendMessageAsync(client, "DisconnectSucess!");
				await client.Client.DisconnectAsync(false); //для повторного использования - true
			}
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

