using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModel.Clients.MainSite;
using SharedModel.Contexts;
using SharedModel.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendApi.Controllers
{
	[Route("[controller]")]
	public class SearchController : Controller
	{
		private readonly MainContext context;

		public SearchController(MainContext context)
		{
			this.context = context;
		}
		[HttpPost]
		public async Task<IActionResult> Index([FromBody]SearchRequest search)
		{

			var resp = new SearchResponse();			
			var query = context.Products.AsQueryable();

			resp.total = await query.CountAsync();
			resp.items = query
				.Select(s => new SearchProduct
				{
					brand=s.Brand,
					mrp=s.Mrp,
					price=s.Price,
					discount = (s.Mrp - s.Price) / s.Mrp * 100,
					title=s.Title,
					image = s.Medias.Select(k => Settings.imageKitUrl + k.ServerName).FirstOrDefault()
				}).ToList();



			return Ok(resp);



			
		}
	}
}



