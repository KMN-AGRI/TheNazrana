using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModel.Clients.Shared;
using SharedModel.Contexts;
using SharedModel.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendApi.Controllers
{
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly MainContext context;

		public AdminController(MainContext context)
		{
			this.context = context;
		}

		[HttpGet("products")]
		public IActionResult Products()
			=> Ok(context.Products
				.Select(s => new
				{
					Id = s.Id,
					brand = s.Brand,
					mrp = s.Mrp,
					price = s.Price,
					discount = (s.Mrp - s.Price) / s.Mrp * 100,
					title = s.Title,
					image = s.Medias.Select(k => Settings.imageKitUrl + k.ServerName).FirstOrDefault(),
					s.Sold,
					s.Stock,

				}));

		[HttpPost("products/{id}")]
		public async Task<IActionResult> UpdateStock(int id,[FromForm]int stock)
		{
			var product = await context.Products.FindAsync(id);
			product.Stock = (uint)stock;
			context.Products.Update(product);
			await context.SaveChangesAsync();
			return Ok(new ApiResponse("Product Stock Updated", true));
		}


		[HttpGet("orders")]
		public IActionResult Orders()
			=> Ok(context.Orders
				.Select(s => new
				{
					s.Id,
					s.User,
					s.Address,
					s.Total,
					s.SubTotal,
					s.Discount,
					s.Status,
					Items = s.Items
					.Select(i => new
					{
						Product = new
						{
							i.Product.Title,
							image = i.Product.Medias.Select(k => Settings.imageKitUrl + k.ServerName).FirstOrDefault()
						},
						i.Amount,
						i.Quantity,
						i.Feedback
					})
				})
				.Where(s => s.Status != Status.Pending));

		[HttpPost("updateOrder/{id}")]
		public async Task<IActionResult> updateOrder(string id)
		{
			var order = await context
				.Orders
				.Include(s => s.Events)
				.SingleOrDefaultAsync(s => s.Id == id);

			var events = order.Events;

			var nextEvent = events
				.OrderByDescending(s => s.Type)
				.FirstOrDefault(s => s.Completed == false);

			nextEvent.Completed = true;
			nextEvent.Date = DateTime.UtcNow;

			if (nextEvent.Type == Events.Completed)
				order.Status = Status.Completed;

			context.Orders.Update(order);
			await context.SaveChangesAsync();
			return Ok(new ApiResponse("Order Updated Successfully", true));


		}

		[HttpPost("cancelOrder/{id}")]
		public async Task<IActionResult> cancelOrder(string id)
		{
			var order = await context
				.Orders
				.Include(s => s.Events)
				.SingleOrDefaultAsync(s => s.Id == id);

			order.Events.RemoveAll(s => !s.Completed);
			order.Events.Add(new SharedModel.Servers.OrderEvent(Events.Cancelled, true));
			order.Status = Status.Cancelled;

			context.Orders.Update(order);
			await context.SaveChangesAsync();
			return Ok(new ApiResponse("Order Cancelled Successfully", true));


		}


	}
}

