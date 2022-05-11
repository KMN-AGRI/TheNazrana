using System;
using SharedModel.Extensions;

namespace SharedModel.Servers
{
	public class Coupon
	{
		public Coupon()
		{

		}
		public Coupon(string id =null,string msg="")
		{
			this.Id = id ?? UtilityExtension.generateId(8);
			this.ReedemBy = "";
			this.Message = msg;
		}

		public string Id { get; set; }
		public string Message { get; set; }
		public float Discount { get; set; }
		public int Limit { get; set; }
		public Status Status { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public string ReedemBy { get; set; }
		public int Count { get; set; }

	}
}

