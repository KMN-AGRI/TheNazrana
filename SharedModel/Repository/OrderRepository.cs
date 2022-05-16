using System;
using Microsoft.EntityFrameworkCore;
using SharedModel.Clients.Shared;
using SharedModel.Contexts;
using SharedModel.Servers;
using SharedModel.Services;

namespace SharedModel.Repository
{
	public interface IOrderRepository
	{
		Task<ApiResponse> completeOrder(string id);
		Task<Order> getOrderById(string id);
	}


	public class OrderRepository:IOrderRepository
	{
		private readonly MainContext context;
		private readonly IMailService mail;
		private readonly IUserRepository user;

		public OrderRepository(MainContext context, IMailService mail, IUserRepository user)
		{
			this.context = context;
			this.mail = mail;
			this.user = user;
		}

		public async Task<ApiResponse> completeOrder(string id)
		{
			var order = await getOrderById(id);
			if (order == null)
				return new ApiResponse("Invalid Order Found");

			order.Status = Status.Active;
			order.Date = DateTime.UtcNow;
			mail.orderConfirmation(order.Address.Email ?? user.Email(), order);
			return new ApiResponse("Order Confirmed Successfully", true, order);

		}

		public Task<Order> getOrderById(string id)
			=> context.Orders
				.Include(s => s.Items)
				.ThenInclude(s => s.Product)
				.Include(s => s.Address)
				.SingleOrDefaultAsync(s => s.Id == id);
	}
}

