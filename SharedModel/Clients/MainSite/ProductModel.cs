using System;
using SharedModel.Servers;

namespace SharedModel.Clients.MainSite
{
	record class SmallProduct
	{
		public string title { get; set; }
		public string brand { get; set; }
		public Mediafile media { get; set; }
	}
}

