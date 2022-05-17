using System;
using Microsoft.EntityFrameworkCore;
using SharedModel.Clients.MainSite;
using SharedModel.Clients.Shared;
using SharedModel.Contexts;
using SharedModel.Servers;

namespace SharedModel.Repository
{

	public interface ICartRepository
	{
		IEnumerable<CartItem> getItems();
		bool validateStock(int productId, int stock,int? id=null);
		CartItem getItem(int id);
		ApiResponse addToCart(ClientCart client);
		ApiResponse removeCart(int id);
		void solidateCart(string id);
	}

	public class CartRepository:ICartRepository
	{
		private readonly MainContext context;
		private readonly IUserRepository userRepository;
		public CartRepository(IUserRepository userRepository, MainContext context)
		{
			this.userRepository = userRepository;
			this.context = context;
		}

		public CartItem getItem(int id)
			=> context.CartItems
			.Include(s => s.Product)
			.SingleOrDefault(s => s.Id == id);



		public bool validateStock(int productId, int stock,int? id)
		{
			int newStock = stock;

			if (id.HasValue)
				newStock += getItem(id.Value)?.Quantity??0;

			var product = context.Products.SingleOrDefault(s=>s.Id==productId);

			if (product.Stock <= newStock)
				return false;

			return true;


		}

		public ApiResponse addToCart(ClientCart client)
		{
			var cart = client.id.HasValue ? getItem(client.id.Value) :
				new CartItem(context.Products.SingleOrDefault(s=>s.Id==client.productId), (int)client.quantity,userRepository.Id());


			if (client.id.HasValue)
			{
				cart.Quantity = (int)client.quantity;
				context.CartItems.Update(cart);
			}
			else
				context.CartItems.Add(cart);

			context.SaveChanges();

			return new ApiResponse(client.id.HasValue ? "Cart updated successfully" : "Item Added to cart", true);


		}

		public ApiResponse removeCart(int id)
		{
			var cart = getItem(id);
			if (cart!= null)
			{
				cart.Status = Status.Disabled;
				context.CartItems.Update(cart);
				context.SaveChanges();
			}
			

			return new ApiResponse("Item Removed Successfully", true);
		}

		public IEnumerable<CartItem> getItems()
			=> context
			.CartItems.Where(s => s.User == userRepository.Id() & s.Status != Status.Disabled)
			.Include(s=>s.Product)
			.ToList();

		public void solidateCart(string id)
		{
			throw new NotImplementedException();
		}
	}
}

