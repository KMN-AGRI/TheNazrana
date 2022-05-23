using System;
using SharedModel.Servers;

namespace SharedModel.Clients.MainSite
{
	public class OrderModel
	{
		public OrderModel()
		{
		}
	}

	public class ConfirmOrder
	{
		public string PaymentId { get; set; }
		public Address Address { get; set; }
	}
}

