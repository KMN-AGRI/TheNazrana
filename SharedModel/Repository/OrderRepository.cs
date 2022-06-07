using System;
using Microsoft.EntityFrameworkCore;
using SharedModel.Clients.MainSite;
using SharedModel.Clients.Shared;
using SharedModel.Contexts;
using SharedModel.Helpers;
using SharedModel.Servers;
using SharedModel.Services;

namespace SharedModel.Repository
{
	public interface IOrderRepository
	{
		Task<ApiResponse> createOrder();
		Task<ApiResponse> completeOrder(string id,string paymentId,Address address);
		Task<object> getOrderById(string id);
	}


	public class OrderRepository:IOrderRepository
	{
		private readonly IUserRepository userRepository;
		private readonly MainContext context;
		private readonly IMailService mail;
		private readonly IPaymentRepository paymentRepository;
		private readonly IUserRepository user;
		private readonly IAlertRepository alertRepository;
		public OrderRepository(MainContext context, IMailService mail, IUserRepository user, IUserRepository userRepository, IPaymentRepository paymentRepository, IAlertRepository alertRepository)
		{
			this.context = context;
			this.mail = mail;
			this.user = user;
			this.userRepository = userRepository;
			this.paymentRepository = paymentRepository;
			this.alertRepository = alertRepository;
		}

		public async Task<ApiResponse> completeOrder(string id,string paymentId, Address address)
		{
			var order = await context
				.Orders
				.Include(s=>s.Address)
				.Include(s => s.Payment)
				.Include(s => s.Items)
				.ThenInclude(s => s.Product)
				.SingleOrDefaultAsync(s => s.Id == (id?? "c14fe64b"));
			if (order == null)
				return new ApiResponse("Invalid Order Found");
			if (!paymentRepository.verifyPayment(order.Payment,paymentId))
				return new ApiResponse("Payment Failed");


			foreach(var item in order.Items)
			{
				var cartItem = await context.CartItems
					.FirstOrDefaultAsync(s => s.Product.Id == item.Product.Id & s.Status == Status.Active);

				if(cartItem!=null)
				{
					cartItem.Status = Status.Completed;
					context.CartItems.Update(cartItem);
				}	

				item.Product.Stock--;
			}
			order.Events = new List<OrderEvent>()
			{
				new OrderEvent(Events.Ordered,true),
				new OrderEvent(Events.Shipped),
				new OrderEvent(Events.In_Transit),
				new OrderEvent(Events.Delivered),
			};
			address.User = userRepository.Id();
			order.Status = Status.Active;
			order.Address = address;
			order.Date = DateTime.UtcNow;
			context.Orders.Update(order);
			await context.SaveChangesAsync();
			await alertRepository.notifyOrder(order.Id);
			//mail.orderConfirmation(order.Address.Email ?? user.Email(), order);
			return new ApiResponse("Order Confirmed Successfully", true, order);

		}

		public async Task<ApiResponse> createOrder()
		{
			var cartItems = await context.CartItems
				.Where(s => s.Status == Status.Active & s.User == userRepository.Id())
				.Include(s => s.Product)
				.ToListAsync();

			if (cartItems.Count == 0)
				return new ApiResponse("Cart Is Empty Add some product");

			foreach(var cart in cartItems)
			{
				var product = cart.Product;
				if (product.Stock < cart.Quantity)
					return new ApiResponse($"'{product.Title}' Is Out Of Stock");
			}


			var order = new Order();
			order.Id = Extensions.UtilityExtension.generateId(8);
			order.Date = DateTime.UtcNow;
			order.User = userRepository.Id();
			order.Items = cartItems
				.Select(s => new Orderitem()
				{
					Amount = s.Product.Price,
					Product = s.Product,
					Quantity = s.Quantity,
					OrderId = order.Id,
					
				}).ToList();
			order.SubTotal = order.Items.Sum(s => s.Amount * s.Quantity);
			order.Total = order.SubTotal - order.Discount;
			order.Status = Status.Pending;
			order.Payment = paymentRepository.createPayment(order.Total);

			context.Orders.Add(order);
			await context.SaveChangesAsync();
			return new ApiResponse("Continue With Payment", true, new
			{
				order.Id,
			});


			
		}

		public async Task<object> getOrderById(string id)
			=> await context.Orders
			.Select(s => new
			{
				s.Id,
				Items = s.Items.Select(i => new
				{
					Product = new
					{
						i.Product.Title,
						Image = i.Product.Medias.Select(k => Settings.imageKitUrl + k.ServerName).FirstOrDefault(),

					},
					i.Amount,
					i.Quantity
				}),
				Payment = new
				{
					s.Payment.Razorpay_Id,
					Key = Settings.paymentKeyId,

				},
				s.Total,
				s.SubTotal,
				s.Discount

			}).SingleOrDefaultAsync(s => s.Id == id);
	}
}

