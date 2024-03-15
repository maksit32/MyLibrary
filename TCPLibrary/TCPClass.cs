﻿using System.Net.Sockets;
using System.Text;




//Use using construction with TCPClient
namespace TCPLibrary
{
	public static class TCPLibrary
	{
		public static async Task<TcpClient> WaitIncomingConnectionAsync(TcpListener listen)
		{
			if (listen is null)
			{
				throw new ArgumentNullException(nameof(listen));
			}

			listen.Start();
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine($"Ожидаем подключения от {listen.LocalEndpoint}.");

			//получить клиента
			TcpClient client = await listen.AcceptTcpClientAsync();

			Console.WriteLine($"Подключение с {client.Client.RemoteEndPoint} установлено.");
			Console.ResetColor();
			return client;
		}

		public static async Task DisconnectFromClientAsync(TcpClient client)
		{
			if (client is null)
			{
				throw new ArgumentNullException(nameof(client));
			}
			if (!client.Connected) return;


			await SendMessageAsync(client, "DisconnectSucess!");
			await client.Client.DisconnectAsync(false); //для повторного использования - true
		}

		public static async Task SendMessageAsync(TcpClient client, string message)
		{
			if (client is null)
			{
				throw new ArgumentNullException(nameof(client));
			}

			if (string.IsNullOrWhiteSpace(message))
			{
				throw new ArgumentException($"\"{nameof(message)}\" не может быть пустым или содержать только пробел.", nameof(message));
			}

			if (client.Connected)
			{
				await client.Client.SendAsync(Encoding.UTF8.GetBytes(message));
			}
		}

		public static async Task<string> ReceiveMessageAsync(TcpClient client)
		{
			if (client is null)
			{
				throw new ArgumentNullException(nameof(client));
			}

			if (client.Connected)
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
	}
}


/* 
 * ToConnect:
	//ip
	IPAddress localAddr = IPAddress.Parse("192.168.0.13");
	//port
	int port = 9999;
	TcpListener listenClients = new TcpListener(localAddr, port); 
 */