using System;
using Microsoft.EntityFrameworkCore;
using SharedModel.Clients.Shared;
using SharedModel.Contexts;
using SharedModel.Servers;
using SharedModel.Services;

namespace SharedModel.Repository
{
	public interface IAlertRepository
	{

		Task notifyOrder(string id);

	}
	public class AlertRepository: IAlertRepository
	{
		private readonly IHttpFactory httpFactory;
		private readonly MainContext mainContext;


		public AlertRepository(IHttpFactory httpFactory, MainContext mainContext)
		{
			this.httpFactory = httpFactory;
			this.mainContext = mainContext;
		}

		public async Task notifyOrder(string id)
		{
			var order = mainContext.
				Orders
				.Include(s => s.Address)
				.Include(s => s.Items)
				.ThenInclude(s => s.Product)
				.Include(s => s.Events)
				.SingleOrDefault(s => s.Id == id);

			var mostRecentStatus = order
				.Events
				.OrderByDescending(s => s.Date)
				.FirstOrDefault();



			var strMessage = getMessage(mostRecentStatus.Type);

			var template = new TemplateMessage("91"+order.Address.Mobile, new TemplateMessage.Template("order", new List<TemplateMessage.Template.Component>()
			{
				new TemplateMessage.Template.Component(new List<TemplateMessage.Template.Component.Parameter>()
				{
					new TemplateMessage.Template.Component.Parameter(order.Address.Name),
					new TemplateMessage.Template.Component.Parameter("#"+order.Id),
					new TemplateMessage.Template.Component.Parameter(strMessage),
				})
			}));

			var response = await httpFactory.ExecuteRequest<object>("/messages", template);


		}

		private string getMessage(Events type)
		{
			switch(type)
			{
				case Events.Ordered:return "Placed successfully 🎉";
				case Events.Shipped:return "Shipped at "+DateTime.Now.ToLongTimeString();
				case Events.In_Transit:return "in Transit";
				case Events.Delivered:return "Delivered Successfully 🎉";
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

