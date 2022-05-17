using System;
using System.ComponentModel.DataAnnotations;

namespace SharedModel.Servers
{
	public class Feedback
	{
		public Feedback()
		{
			this.Date = DateTime.UtcNow;
			
		}
		[Key]
		public int Id { get; set; }
		public string Message { get; set; }
		public float Stars { get; set; }
		public DateTime Date { get; set; }
		public string TargetId { get; set; }
		public TargetType TargetType { get; set; }
		public string User { get; set; }
	}
}

