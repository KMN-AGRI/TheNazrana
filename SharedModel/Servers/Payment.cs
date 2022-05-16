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
		public string Method { get; set; }
	}
}

