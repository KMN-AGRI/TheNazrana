using System;
using SharedModel.Extensions;

namespace SharedModel.Servers
{
	public class Mediafile
	{
		
		public Mediafile()
		{
			this.Date = DateTime.UtcNow;
			this.Id = UtilityExtension.generateId();
		}

		public string Id { get; set; }
		public DateTime Date { get; set; }
		public string ServerName { get; set; }
		public string ClientName { get; set; }
		public string ContentType { get; set; }
		public int Size { get; set; }

	}
}

