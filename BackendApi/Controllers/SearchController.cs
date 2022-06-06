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

			var (price_min, price_max) = GetRange(search.priceRanges);
			var (discount_min, discount_max) = GetRange(search.discountRanges);
			var keyword = search.query;
			var sort = search.sort;

			var resp = new SearchResponse();
			var query = context.Products.AsQueryable()
				.Select(s => new SearchProduct
				{
					Id = s.Id,
					brand = s.Brand,
					mrp = s.Mrp,
					price = s.Price,
					sold=s.Sold,
					date=s.Date,
					description=s.Description,
					discount = (s.Mrp - s.Price) / s.Mrp * 100,
					title = s.Title,
					image = s.Medias.Select(k => Settings.imageKitUrl + k.ServerName).FirstOrDefault()
				});


			search.page = search.page < 1 ? 1 : search.page;
			int startingPosition = (search.page - 1) * 12;


			if (!string.IsNullOrEmpty(search.query))
				query = query.Where(s => s.title.Contains(search.query) || s.description.Contains(search.query));

			if (price_min.HasValue)
				query = query.Where(s => s.price>=price_min.Value);

			if (price_max.HasValue)
				query = query.Where(s => s.price <= price_max.Value);

			if (discount_min.HasValue)
				query = query.Where(s => s.discount >= discount_min);

			if (discount_max.HasValue)
				query = query.Where(s => s.discount <= discount_max);


			switch (sort)
			{
				case ResultOrder.PriceHighToLow:
					query = query.OrderByDescending(x => x.price);
					break;
				case ResultOrder.PriceLowToHigh:
					query = query.OrderBy(x => x.price);
					break;
				case ResultOrder.Latest:
					query = query.OrderByDescending(x => x.date);
					break;
				case ResultOrder.DiscountHighToLow:
					query = query.OrderByDescending(s => s.discount);
					break;
				case ResultOrder.DiscountLowToHigh:
					query = query.OrderBy(s => s.discount);
					break;
				default:
					query = query.OrderByDescending(x => x.sold);
					break;

			}


			resp.total = await query.CountAsync();
			resp.items = await query
				
				.Skip(startingPosition).Take(12).ToListAsync();

			resp.hasMore = resp.items.Count() == 12;
			resp.nextPage = resp.hasMore ? search.page + 1 : search.page;
			resp.curRange = $"Showing {startingPosition} - {startingPosition + 12}";
			


			return Ok(resp);



			
		}


		private (int?, int?) GetRange(List<ItemRange> ranges)
		{
			if (ranges == null | ranges?.Count == 0)
				return (null, null);

			var min = ranges.Select(s => s.min).OrderBy(s => s).FirstOrDefault();
			var max = ranges.Select(s => s.max).OrderByDescending(s => s).FirstOrDefault();
			return (min, max);

		}
	}
}



