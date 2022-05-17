using System;
namespace SharedModel.Servers
{
	public class Address
	{
		public Address()
		{
			
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Building { get; set; }
		public string Street { get; set; }
		public string? Landmark { get; set; }
		public string Mobile { get; set; }
		public string? Email { get; set; }
		public string User { get; set; }

	}
}

