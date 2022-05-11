using System;
namespace SharedModel.Servers
{
	public class Notification
	{
		public Notification()
		{

		}
		public Notification(string title,string desc="",AlertType type=AlertType.Info)
		{
			this.Date = DateTime.UtcNow;
			this.Title = title;
			this.Description = desc;
			this.Type = type;
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public AlertType Type { get; set; }
		public DateTime Date { get; set; }
		public bool SeenDate { get; set; }


	}
}

