using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        

    }
}

