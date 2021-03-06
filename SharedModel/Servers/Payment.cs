using System;
using System.ComponentModel.DataAnnotations;

namespace SharedModel.Servers
{
	public class Payment
	{
		public Payment()
		{
			this.card_id = "";
			this.Card = "";
			this.ErrorCode = "";
			this.ErrorMessage = "";
			this.currency = "";
			this.upi_transaction_id = "";
			this.wallet = "";
			this.bank = "";
			this.Status = "";
			this.Vpa = "";
			this.Method = "";

		}

		[Key]
		public int Id { get; set; }
		public string Razorpay_Id { get; set; }
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

