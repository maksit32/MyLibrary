using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



//use only props! For json serealization!
namespace EFCoreLibrary.Entities
{
	public class TelegramUser
	{
		public Guid Id { get; init; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Patronymic { get; set; }
		public string PhoneNumber { get; set; }
		public long TgChatId { get; set; }
		public bool IsSubscribed { get; set; } = false;
		public bool IsAdmin { get; set; } = false;
		public DateTime LastMessageTime { get; set; }
		//много эвентов (Fluent API)
		public List<UserCreatedEvent> UserCreatedEvents { get; set; } = new();

		//PROTECTED!
		protected TelegramUser()
		{

		}
		//спец. добавление для клиентского приложения
		public TelegramUser(Guid UserId, string name, string surname, string patronymic, string phoneNumber, long tgChatId, DateTime lastMessageTime, bool isSubscribed = false, bool isAdmin = false)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"\"{nameof(name)}\" не может быть пустым или содержать только пробел.", nameof(name));
			}

			if (string.IsNullOrWhiteSpace(surname))
			{
				throw new ArgumentException($"\"{nameof(surname)}\" не может быть пустым или содержать только пробел.", nameof(surname));
			}

			if (string.IsNullOrWhiteSpace(patronymic))
			{
				throw new ArgumentException($"\"{nameof(patronymic)}\" не может быть пустым или содержать только пробел.", nameof(patronymic));
			}

			if (string.IsNullOrWhiteSpace(phoneNumber))
			{
				throw new ArgumentException($"\"{nameof(phoneNumber)}\" не может быть пустым или содержать только пробел.", nameof(phoneNumber));
			}

			if (tgChatId <= 0) throw new ArgumentOutOfRangeException("Поле tgChatId не должно быть меньше 0!");

			Id = UserId;
			TgChatId = tgChatId;
			IsSubscribed = isSubscribed;
			IsAdmin = isAdmin;
			LastMessageTime = lastMessageTime;
			Name = name;
			Surname = surname;
			Patronymic = patronymic;
			PhoneNumber = phoneNumber;
		}
		//основное добавление
		public TelegramUser(long tgChatId, string name, string surname, string patronymic, string phoneNumber, DateTime lastMessageTime, bool isSubscribed = false, bool isAdmin = false)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"\"{nameof(name)}\" не может быть пустым или содержать только пробел.", nameof(name));
			}

			if (string.IsNullOrWhiteSpace(surname))
			{
				throw new ArgumentException($"\"{nameof(surname)}\" не может быть пустым или содержать только пробел.", nameof(surname));
			}

			if (string.IsNullOrWhiteSpace(patronymic))
			{
				throw new ArgumentException($"\"{nameof(patronymic)}\" не может быть пустым или содержать только пробел.", nameof(patronymic));
			}

			if (string.IsNullOrWhiteSpace(phoneNumber))
			{
				throw new ArgumentException($"\"{nameof(phoneNumber)}\" не может быть пустым или содержать только пробел.", nameof(phoneNumber));
			}
			if (tgChatId <= 0) throw new ArgumentOutOfRangeException("tgChatId");

			Id = Guid.NewGuid();    //рандомим GUID
			TgChatId = tgChatId;
			IsSubscribed = isSubscribed;
			IsAdmin = isAdmin;
			Name = name;
			Surname = surname;
			Patronymic = patronymic;
			PhoneNumber = phoneNumber;
		}
		public override string ToString()
		{
			return $"Id: {Id}	Имя: {Name}	  Фамилия: {Surname}   Отчество: {Patronymic}   Номер: {PhoneNumber}   TgChatId: {TgChatId}	IsSubscribed: {IsSubscribed}	IsAdmin: {IsAdmin}";
		}
	}
}
