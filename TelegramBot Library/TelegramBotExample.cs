

//installation:
//1) NuGet --> Telegram.Bot
//2) NuGet --> Telegram.Bots.Extensions.Polling




namespace TelegramBotLibrary
{
	public class TelegramBotLibrary
	{
		public class TelegramBotExample
		{
			#region [EXAMPLE2]
			#region [Objects]
			private static string token = "6485249044:AAFfU30VmVPwQmcMXPBGjXVA0dHLw5sdSKE";
			private static ITelegramBotClient botClient = new TelegramBotClient(token);
			private static ReceiverOptions receiverOptions = null!;
			private static CancellationTokenSource cts = new CancellationTokenSource();
			private static INotifyService notifyService = null!;
			private static object locker = new(); //для записи логов
			private static SemaphoreSlim semaphore = new SemaphoreSlim(1); //для изменения документов

			//filepaths
			private static readonly string messageLogPath = "C:\\Users\\korni\\source\\repos\\ScienceMSTUCABot\\MSTUCABOT.ConsoleServer\\Logs\\messagesLog.txt";
			private static readonly string startBotLogPath = "C:\\Users\\korni\\source\\repos\\ScienceMSTUCABot\\MSTUCABOT.ConsoleServer\\Logs\\startBotLog.txt";
			private static readonly string errorLogPath = "C:\\Users\\korni\\source\\repos\\ScienceMSTUCABot\\MSTUCABOT.ConsoleServer\\Logs\\errorLog.txt";

			private static readonly string fileSNOFullPath = "C:\\Users\\korni\\source\\repos\\ScienceMSTUCABot\\MSTUCABOT.ConsoleServer\\Documents\\SNO.docx";
			private static readonly string fileSMUFullPath = "C:\\Users\\korni\\source\\repos\\ScienceMSTUCABot\\MSTUCABOT.ConsoleServer\\Documents\\SMU.docx";
			#endregion
			#endregion



			#region [Configuration&objects]
			private static readonly IConfiguration config = new ConfigurationBuilder()
				.AddJsonFile(@"C:\Users\korni\source\repos\TelegramChatProject\TelegramChatProject\Settings\appsettings.json",
					optional: false,
					reloadOnChange: true)
					.Build();
			private static readonly ReceiverOptions receiverOptions = new ReceiverOptions
			{
				AllowedUpdates = new[]
						{
						UpdateType.Message,
						UpdateType.CallbackQuery
					},
				ThrowPendingUpdates = true,
			};

			private static readonly string token = config.GetSection("Token").Value ?? throw new EmptyValueStringException("Не обнаружена секция Token в файле appsettings.json");
			private static readonly ITelegramBotClient botClient = new TelegramBotClient(token);
			private static readonly CancellationTokenSource cts = new CancellationTokenSource();
			private static readonly IResenderService resService = new ResenderService();
			#endregion


			#region [Buttons]
			private static ReplyKeyboardMarkup replyKeyboardUserSub = new ReplyKeyboardMarkup(new List<KeyboardButton[]>(){
											new KeyboardButton[]
											{
												new KeyboardButton($"Проверка уведомлений"),
											},
											new KeyboardButton[]
											{
												new KeyboardButton($"Изменить статус уведомлений"),
											}})
			{ ResizeKeyboard = true };
			#endregion

			//starts the bot
			private static void Main()
			{
				Console.WriteLine("Запущен бот " + botClient.GetMeAsync().Result.FirstName);



				//Начало получения сообщений от Telegram
				botClient.StartReceiving(UpdateHandlerAsync, ErrorHandlerAsync, receiverOptions, cts.Token);
				Console.WriteLine("Нажмите Enter, чтобы выключить сервер:");
				Console.ReadLine();
				cts.Cancel();
			}

			//Each new message from client	(EFCore - Scoped)
			private static async Task UpdateHandlerAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
			{
				try
				{
					using TelegramUsersDb telegramDb = new TelegramUsersDb(config.GetSection("SqlConnectionString").Value ?? throw new EmptyValueStringException("Не обнаружена секция SqlConnectionString в файле appsettings.json"));


					switch (update.Type)
					{
						case UpdateType.Message:
							{
								var message = update.Message;
								if (message is null) throw new NullReferenceException(nameof(update.Message));
								long chatId = message.Chat.Id;

								switch (message.Type)
								{
									case MessageType.Text:
										{
											string lowerCaseMessage = message.Text.ToLower();
											if (lowerCaseMessage == "/start")
											{
												string sendMessage = await telegramDb.AddTgUserAsync(new TelegramUser(chatId));
												await botClient.SendTextMessageAsync(chatId, $"Добро пожаловать в чат! Пишите любые сообщения!\n{sendMessage}", replyMarkup: replyKeyboardUserSub);
											}
										}
										break;
									case MessageType.Photo:
										break;
									case MessageType.Audio:
										break;
									case MessageType.Video:
										break;
									case MessageType.Voice:
										break;
									case MessageType.Document:
										break;
									case MessageType.Sticker:
										break;
									case MessageType.Location:
										break;
									case MessageType.Contact:
										break;
								}
							}
							break;
						case UpdateType.CallbackQuery:
							{

							}
							break;
					}
				}
				catch (Exception)
				{
					await Console.Out.WriteLineAsync($"Возникла ошибка! Проверьте лог!	Время: {DateTime.Now}");
				}
			}

			//обработчик ошибок
			#region [ErrorHandler]
			private static async Task ErrorHandlerAsync(ITelegramBotClient botClient, Exception ex, CancellationToken cancellationToken)
			{
				var ErrorMessage = ex switch
				{
					ApiRequestException apiRequestException
						=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
					_ => ex.ToString()
				};

				await Console.Out.WriteLineAsync($"Возникла ошибка! Проверьте лог!	Время: {DateTime.Now}");
			}
			#endregion
		}
	}
}


//главное все в private + выносить все в appsettings.json или в private static readonly поля класса
//не хранить ничего важного в коде!