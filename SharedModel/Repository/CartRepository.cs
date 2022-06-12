using System;
using Microsoft.EntityFrameworkCore;
using SharedModel.Clients.MainSite;
using SharedModel.Clients.Shared;
using SharedModel.Contexts;
using SharedModel.Helpers;
using SharedModel.Servers;

namespace SharedModel.Repository
{

	public interface ICartRepository
	{
		IEnumerable<object> getItems();
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



        public bool validateStock(int productId, int stock, int? id)
        {
            int newStock = stock;

            if (id.HasValue)
                newStock += getItem(id.Value)?.Quantity ?? 0;

            var product = context.Products.SingleOrDefault(s => s.Id == productId);

            if (product.Stock <= newStock)
                return false;

            return true;


        }

        public ApiResponse addToCart(ClientCart client)
		{
			var product = context
				.Products
				.Find(client.productId);

			if (product == null)
				return new ApiResponse("Invalid Product Sent");

			if (product.Stock < client.quantity)
				return new ApiResponse("Oops! only " + product.Stock + " stock left");

			if (client.quantity == 0)
				return new ApiResponse("Qauntity should be greater than 0");

			var existingCart = context.CartItems
				.Where(s => s.User == userRepository.Id() & s.Status == Status.Active & s.Product.Id == client.productId)
				.FirstOrDefault();

			if (existingCart == null)
				context.CartItems.Add(new CartItem()
				{
					Date = DateTime.UtcNow,
					Product = product,
					Quantity = (int)client.quantity,
					Status = Status.Active,
					User = userRepository.Id()
				});
			else
            {
				existingCart.Quantity = (int)client.quantity;
				context.CartItems.Update(existingCart);
			}


			context.SaveChanges();

			return new ApiResponse(existingCart!=null ? "Cart updated successfully" : "Item Added to cart", true);


		}

		public ApiResponse removeCart(int id)
		{
			var cart = getItem(id);
			if (cart == null)
				return new ApiResponse("Invalid Cart Id Sent", false, id);
			cart.Status = Status.Disabled;
			context.CartItems.Update(cart);
			context.SaveChanges();


			return new ApiResponse("Item Removed Successfully", true);
		}

		public IEnumerable<object> getItems()
			=> context
			.CartItems.Where(s => s.User == userRepository.Id() & s.Status == Status.Active)
			.Include(s=>s.Product)
			.Select(s=>new
			{
				s.Id,
				s.Quantity,
				Product=new
				{
					s.Product.Price,
					s.Product.Mrp,
					s.Product.Id,
					s.Product.Title,
					s.Product.Brand,
					Image = s.Product.Medias.Select(k => Settings.imageKitUrl + k.ServerName).FirstOrDefault(),
				},
			})
			.ToList();


		public void solidateCart(string id)
		{
			throw new NotImplementedException();
		}
	}
}

