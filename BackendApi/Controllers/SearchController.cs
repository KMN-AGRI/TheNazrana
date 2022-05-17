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


			search.page = search.page < 1 ? 1 : search.page;
			int startingPosition = (search.page - 1) * 12;


			if (!string.IsNullOrEmpty(search.query))
				query = query.Where(s => s.Title.Contains(search.query) || s.Description.Contains(search.query));



			resp.total = await query.CountAsync();
			resp.items = await query
				.Select(s => new SearchProduct
				{
					Id=s.Id,
					brand=s.Brand,
					mrp=s.Mrp,
					price=s.Price,
					discount = (s.Mrp - s.Price) / s.Mrp * 100,
					title=s.Title,
					image = s.Medias.Select(k => Settings.imageKitUrl + k.ServerName).FirstOrDefault()
				})
				.Skip(startingPosition).Take(12).ToListAsync();

			resp.hasMore = resp.items.Count() == 12;
			resp.nextPage = resp.hasMore ? search.page + 1 : search.page;
			resp.curRange = $"Showing {startingPosition} - {startingPosition + 12}";
			


			return Ok(resp);



			
		}
	}
}



