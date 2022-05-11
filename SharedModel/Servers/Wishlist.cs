using System;
using System.ComponentModel.DataAnnotations;

namespace SharedModel.Servers
{
	public class Wishlist
	{
		public Wishlist()
		{

		}
		public Wishlist(Product product,string user)
		{
			this.Product = product;
			this.User = user;
			this.Date = DateTime.UtcNow;
		}
		[Key]
		public int Id { get; set; }
		public Product Product { get; set; }
		public DateTime Date { get; set; }
		public Status Status { get; set; }
		public string User { get; set; }
	}
}

