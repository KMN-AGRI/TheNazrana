using System;
using System.ComponentModel.DataAnnotations;

namespace SharedModel.Servers
{
	public class Banner
	{
		public Banner()
		{

		}
		public Banner(string title,Mediafile media,string href="")
		{
			this.Title = title;
			this.Media = media;
			this.Href = href;
		}
		[Key]
		public int Id { get; set; }
		public string Title { get; set; }
		public Mediafile Media { get; set; }
		public string Href { get; set; }
	}
}

