using System;
using System.ComponentModel.DataAnnotations;
using SharedModel.Extensions;

namespace SharedModel.Servers
{

	public class Order
	{
		
		[Key]
		public string Id { get; set; }
		public Coupon? Coupon { get; set; }
		public float SubTotal { get; set; }
		public float Total { get; set; }
		public float Discount { get; set; }
		public DateTime Date { get; set; }
		public Status Status { get; set; }
		public Address? Address { get; set; }
		public List<OrderEvent> Events { get; set; }
		public List<Orderitem> Items { get; set; }
		public string? User { get; set; }

	}

	public class Orderitem
	{
		public Orderitem()
		{
		}

		[Key]
		public int Id { get; set; }
		public int Quantity { get; set; }
		public float Amount { get; set; }
		public Product Product { get; set; }
		public string OrderId { get; set; }
		public Feedback? Feedback { get; set; }

	}

	public class OrderEvent
	{
		public OrderEvent()
		{

		}

		public OrderEvent(Events type,bool completed=false)
		{
			this.Type = type;
			this.Completed = completed;
			if (completed)
				this.Date = DateTime.UtcNow;

		}

		[Key]
		public int Id { get; set; }
		public Events Type { get; set; }
		public DateTime? Date { get; set; }
		public bool Completed { get; set; }
	}

}

