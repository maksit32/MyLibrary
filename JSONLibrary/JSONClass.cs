using Newtonsoft.Json;



namespace JSONLibrary
{
	public static class JSONLibrary
	{
		//Object - is your type
		public static string SerializeClassObject(Object deserializedObject)
		{
			if(deserializedObject is null) throw new ArgumentNullException(nameof(deserializedObject));

			string serializedObject = JsonConvert.SerializeObject(deserializedObject);
			return serializedObject;
		}
		public static Object DeserializeClassObject(string serializedString)
		{
			if (string.IsNullOrWhiteSpace(serializedString))
			{
				throw new ArgumentException($"\"{nameof(serializedString)}\" не может быть пустым или содержать только пробел.", nameof(serializedString));
			}

			try
			{
				Object deserializedObject = JsonConvert.DeserializeObject<Object>(serializedString) ?? throw new NullReferenceException("Deserialized object is null!");
				return deserializedObject;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
