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
		public string ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
	}
}

