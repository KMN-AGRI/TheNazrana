using System;
using System.ComponentModel.DataAnnotations;

namespace SharedModel.Servers
{
	public class Payment
	{
		public Payment()
		{
		}

		[Key]
		public int Id { get; set; }
		public DateTime? Date { get; set; }
		public float Amount { get; set; }
		public string Method { get; set; }
		public string Vpa { get; set; }
		public string Status { get; set; }
		public bool Captured { get; set; }
		public string card_id { get; set; }
		public string Card { get; set; }
		public string bank { get; set; }
		public string wallet { get; set; }
		public string upi_transaction_id { get; set; }
		public string currency { get; set; }
		public string ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
	}
}

