using System;
using SharedModel.Extensions;

namespace SharedModel.Servers
{

	public class Order
	{
		public Order()
		{
			this.Id = UtilityExtension.generateId(7);
			this.Date = DateTime.UtcNow;
		}
		public Coupon Coupon { get; set; }
		public string Id { get; set; }
		public float SubTotal { get; set; }
		public float Total { get; set; }
		public float Discount { get; set; }
		public DateTime Date { get; set; }
		public Status Status { get; set; }
		public Address Address { get; set; }
		public List<Keypair> Events { get; set; }
		public List<Orderitem> Items { get; set; }
		public string? User { get; set; }

	}

	public class Orderitem
	{
		public Orderitem()
		{
		}

		public int Id { get; set; }
		public int Quantity { get; set; }
		public float Amount { get; set; }
		public Product Product { get; set; }
		public string OrderId { get; set; }
		public Feedback Feedback { get; set; }
	}
}

