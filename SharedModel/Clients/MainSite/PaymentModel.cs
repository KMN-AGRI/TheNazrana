using System;
namespace SharedModel.Clients.MainSite
{
	public class PaymentResponse
	{
		public string id { get; set; }
		public string key { get; set; }
		public float amount { get; set; }
		public string currency { get; set; }
		public string name { get; set; }
		public string email { get; set; }
	}

	public class PaymentModel
	{
		public PaymentModel()
		{
		}
	}
}

