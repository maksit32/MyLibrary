using System.Collections.Generic;
using System.Reflection.Emit;



//EF Library
//1) NuGet --> Microsoft.EntityFrameworkCore
//2) NuGet --> Microsoft.EntityFrameworkCore.SqlServer	(PostgreeSql and others)
//
//This is DbContextFile!!!
//DbContext object has Scoped lifetime!


namespace EFCoreLibrary
{
	//this EFCoreDb takes name from JSON.
	public class EFCoreDbExample : DbContext
	{
		//your connectionString
		//ex (fromJsonFile):

		/*	var config = new ConfigurationBuilder()
		.AddJsonFile(@"C:\Users\korni\source\repos\ScienceMSTUCABot\MSTUCABOT.ConsoleServer\Settings\appsettings.json",
		optional: false,
		reloadOnChange: true)
		.Build();
		using TelegramUsersDb telegramDb = 
		new TelegramUsersDb(config.GetSection("SqlConnectionString").Value ?? throw new EmptyValueConnectionStringException("Не обнаружена секция SqlConnectionString в файле appsettings.json"));
		*/

		private readonly string _connectionString;
		//DataTable named as TelegramUsers (Entity: TelegramUser)
		//creation table of TelegramUser Entity
		public DbSet<TelegramUser> TelegramUsers => Set<TelegramUser>();

		public TelegramUsersDb(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				throw new ArgumentException($"\"{nameof(connectionString)}\" не может быть пустым или содержать только пробел.", nameof(connectionString));
			}

			_connectionString = connectionString;

			Database.EnsureCreated();
		}
		//configuring tables (keys)
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserCreatedEvent>()
			.HasOne(e => e.TgUser)
			.WithMany(u => u.UserCreatedEvents)
			.OnDelete(DeleteBehavior.Cascade);
			base.OnModelCreating(modelBuilder);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//using SqlServer (only for MSSQL Server!)
			optionsBuilder.UseSqlServer(_connectionString);
		}

		//CRUD	methods ex:
		public async Task<bool> AddTgUserAsync(TelegramUser user)
		{
			if (user is null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			//проверка на существоание? (по chatId)
			if (TelegramUsers.ToList().Exists(e => e.TgChatId == user.TgChatId))
			{
				return false;
			}

			//добавление пользователя
			await TelegramUsers.AddAsync(user);
			await this.SaveChangesAsync();
			return true;
		}
		public async Task<IReadOnlyCollection<TelegramUser>> ReadAllTgUsersAsync()
		{
			return await TelegramUsers.ToListAsync();
		}
		public async Task<bool> UpdateAdminStatusTgUserAsync(string lowerCaseMessage, long senderChatId)
		{
			if (string.IsNullOrWhiteSpace(lowerCaseMessage))
			{
				throw new ArgumentException($"\"{nameof(lowerCaseMessage)}\" не может быть пустым или содержать только пробел.", nameof(lowerCaseMessage));
			}

			if (senderChatId < 0) throw new ArgumentOutOfRangeException(nameof(senderChatId));

			lowerCaseMessage = lowerCaseMessage.Replace("/adminchadm", "");
			lowerCaseMessage.Replace(" ", "");

			long chatId = long.Parse(lowerCaseMessage);

			var _sender = await GetTgUserByIdAsync(senderChatId);
			var _user = await GetTgUserByIdAsync(chatId);
			if (_sender == null || _user == null) return false;

			if (_sender.IsAdmin && senderChatId != chatId)
			{
				_user.IsAdmin = !_user.IsAdmin;

				await this.SaveChangesAsync();
				return true;
			}
			return false;
		}
		public async Task<string> UpdateNameTgUserAsync(long chatId, string newName)
		{
			if (chatId < 0) throw new ArgumentOutOfRangeException(nameof(chatId));

			if (string.IsNullOrWhiteSpace(newName))
			{
				throw new ArgumentException($"\"{nameof(newName)}\" не может быть пустым или содержать только пробел.", nameof(newName));
			}

			var tgUser = await GetTgUserByIdAsync(chatId);
			if (tgUser == null) return "Ошибка. Пользователь не найден";

			newName = Char.ToUpper(newName[0]) + newName.Substring(1);
			tgUser.Name = newName;
			await this.SaveChangesAsync();
			return $"{GreenCircleEmj}Имя успешно изменено на: {newName}";
		}
		public async Task<TelegramUser?> ReadTgUserByIdAsync(long chatId)
		{
			if (chatId < 0) throw new ArgumentOutOfRangeException(nameof(chatId));

			var _user = await GetTgUserByIdAsync(chatId);
			return _user;
		}
		public async Task<bool> DeleteTgUserByIdAsync(Guid Id)
		{
			var _user = await GetTgUserByIdAsync(Id);
			if (_user is not null)
			{
				TelegramUsers.Remove(_user);
				await this.SaveChangesAsync();
				return true;
			}
			return false;
		}
		//etc....
	}
}
