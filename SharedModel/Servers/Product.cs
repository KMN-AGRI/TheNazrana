using System;
using System.ComponentModel.DataAnnotations;

namespace SharedModel.Servers
{
	public class Product
	{
		public Product()
		{
			this.Date = DateTime.UtcNow;
		}
		[Key]
		public int Id { get; set; }
		public string UId { get; set; }
		public string Brand { get; set; }
		public uint Sold { get; set; }
		public uint Stock { get; set; }
		public float Mrp { get; set; }
		public float Price { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public List<Mediafile> Medias { get; set; }
		public DateTime Date { get; set; }
		public string Category { get; set; }
		public List<Keypair> Details { get; set; }
		public string Tags { get; set; }
	}
}

