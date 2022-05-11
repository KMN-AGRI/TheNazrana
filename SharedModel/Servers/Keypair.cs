using System;
using Newtonsoft.Json;

namespace SharedModel.Servers
{
	public class Keypair
	{
		public Keypair()
		{

		}
		public Keypair(string label,string value,object id,TargetType type, object data=null)
		{
			this.Date = DateTime.UtcNow;
			this.Label = label;
			this.Value = value;
			this.TargetId = id.ToString();
			this.Data = data;
			this.Type = type;
		}

		public int Id { get; set; }
		public string Label { get; set; }
		public string Value { get; set; }
		public object? Data { get; set; }
		public DateTime Date { get; set; }
		public TargetType Type { get; set; }
		public string TargetId { get; set; }
	}
}

