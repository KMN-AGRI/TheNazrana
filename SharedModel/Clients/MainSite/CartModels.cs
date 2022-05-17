using System;
using System.ComponentModel.DataAnnotations;

namespace SharedModel.Clients.MainSite
{
	public class ClientCart
	{
		public int? id { get; set; }
		[Required(ErrorMessage ="Product cannot be empty")]
		public int productId { get; set; }
		public uint quantity { get; set; }

	}
}

