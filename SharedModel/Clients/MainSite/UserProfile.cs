using System;
using Microsoft.AspNetCore.Http;

namespace SharedModel.Clients.MainSite
{
	public class UserProfile
	{
		public IFormFile Image { get; set; }
		public string Name { get; set; }
		public DateTime? DOB { get; set; }
	}
}

