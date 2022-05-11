using System;
using Microsoft.AspNetCore.Identity;

namespace SharedModel.Servers
{
	public class Appuser:IdentityUser
	{
		public Appuser(string name="")
		{
			this.Date = DateTime.UtcNow;
			this.Name = name;
		}

		public string Name { get; set; }
		public DateTime Date { get; set; }
		public DateTime? DOB { get; set; }
		public string? ImageUri { get; set; }
	}
}

