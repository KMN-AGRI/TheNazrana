using System;
namespace SharedModel.Extensions
{
	static public class UtilityExtension
	{
		static public string generateId(int len = 6)
			=> string.Join("", Guid.NewGuid().ToString().Replace("-", "").Take(len));

	}
}

