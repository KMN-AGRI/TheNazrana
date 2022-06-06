using System;
using SharedModel.Clients.Shared;
using SharedModel.Servers;
using SharedModel.Services;

namespace SharedModel.Repository
{
	public interface IAlertRepository
	{

		Task notifyOrder(Order order);

	}
	public class AlertRepository: IAlertRepository
	{
		private readonly IHttpFactory httpFactory;



		public AlertRepository(IHttpFactory httpFactory)
		{
			this.httpFactory = httpFactory;
		}

		public async Task notifyOrder(Order order)
		{
			var mostRecentStatus = order
				.Events
				.OrderByDescending(s => s.Date)
				.FirstOrDefault();



			var strMessage = getMessage(mostRecentStatus.Type);

			var template = new TemplateMessage(order.Address.Mobile, new TemplateMessage.Template("", new List<TemplateMessage.Template.Component>()
			{
				new TemplateMessage.Template.Component(new List<TemplateMessage.Template.Component.Parameter>()
				{
					new TemplateMessage.Template.Component.Parameter(order.Address.Name),
					new TemplateMessage.Template.Component.Parameter(order.Id),
					new TemplateMessage.Template.Component.Parameter(getItems(order)),
					new TemplateMessage.Template.Component.Parameter(strMessage),
					new TemplateMessage.Template.Component.Parameter("https://thenazrana.in")
				})
			}));

			var response = await httpFactory.ExecuteRequest<object>("/messages", template);


		}

		private string getMessage(Events type)
		{
			switch(type)
			{
				case Events.Ordered:return "placed successfully";
				case Events.Shipped:return "shipped";
				case Events.In_Transit:return "travelling";
				case Events.Delivered:return "delivered successfully";
			}
			return "";
		}

		private string getItems(Order order)
			=> order.Items.Count() == 1 ?
			order.Items.FirstOrDefault()
			.Product.Title :
			order.Items.FirstOrDefault()
			.Product.Title + " and " + (order.Items.Count - 1) + " more";

	}
}

