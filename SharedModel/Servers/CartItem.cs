using System;
using System.ComponentModel.DataAnnotations;

namespace SharedModel.Servers
{
	public class CartItem
	{
		public CartItem()
		{

		}
		public CartItem(Product product,int quantity,string user="")
		{
			this.Date = DateTime.UtcNow;
			this.Product = product;
			this.Quantity = quantity;
			this.User = user;

		}
		[Key]
		public int Id { get; set; }
		public Product Product { get; set; }
		public DateTime Date { get; set; }
		public int Quantity { get; set; }
		public Status Status { get; set; }
		public string User { get; set; }

	}
}

