using System;
using Microsoft.EntityFrameworkCore;
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
		Task<ApiResponse> completeOrder(string id);
		Task<object> getOrderById(string id);
	}


	public class OrderRepository:IOrderRepository
	{
		private readonly IUserRepository userRepository;
		private readonly MainContext context;
		private readonly IMailService mail;
		private readonly IPaymentRepository paymentRepository;
		private readonly IUserRepository user;

		public OrderRepository(MainContext context, IMailService mail, IUserRepository user, IUserRepository userRepository, IPaymentRepository paymentRepository)
		{
			this.context = context;
			this.mail = mail;
			this.user = user;
			this.userRepository = userRepository;
			this.paymentRepository = paymentRepository;
		}

		public async Task<ApiResponse> completeOrder(string id)
		{
			var order = await getOrderById(id);
			if (order == null)
				return new ApiResponse("Invalid Order Found");

			//order.Status = Status.Active;
			//order.Date = DateTime.UtcNow;
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

