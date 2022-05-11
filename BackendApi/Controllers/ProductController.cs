using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedModel.Contexts;
using SharedModel.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendApi.Controllers
{
    public class ProductController : Controller
    {
        private readonly MainContext context;
		private readonly IUserRepository userRepository;
		public ProductController(MainContext context, IUserRepository userRepository)
		{
			this.context = context;
			this.userRepository = userRepository;
		}

		[HttpGet("{id}")]
		public IActionResult Index(string id)
			=> Ok(context.Products.Select(s => new
			{
				s.Title,
				s.Tags,
				s.Description,
				s.Stock,
				s.Sold,
				s.Mrp,
				s.Medias,
				s.Brand,
				s.UId,
				Seller = new
				{
					name = "The Nazrana"
				},
				Exist = new
				{
					cart = context.CartItems.Count(c => c.Product.Id == s.Id & c.User == userRepository.Id()),
					wishList = context.Wishlists.Count(c => c.Product.Id == s.Id & c.User == userRepository.Id()),
				}
			}).SingleOrDefault(s => s.UId == id));




    }
}

