using System;
namespace SharedModel.Clients.Shared
{
	public class ApiResponse
	{
		public ApiResponse(string msg,bool status=false,object data=null)
		{
			this.msg = msg;
			this.status = status;
			this.data = data;
		}

		public string msg { get; set; }
		public bool status { get; set; }
		public object data { get; set; }

	}
}

