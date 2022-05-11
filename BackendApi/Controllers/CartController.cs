//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using SharedModel.Clients.MainSite;
//using SharedModel.Repository;

//// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace BackendApi.Controllers
//{
//	[Route("[controller]")]
//    public class CartController : Controller
//    {
//        private readonly ICartRepository repository;

//		public CartController(ICartRepository repository)
//		{
//			this.repository = repository;
//		}
		
//		public IActionResult Index()
//			=> Ok(repository.getItems());


//		[HttpPost]
//		public IActionResult Insert([FromBody] ClientCart client)
//			=> Ok(repository.addToCart(client));


//		[HttpDelete]
//		public IActionResult Remove(int id)
//			=> Ok(repository.removeCart(id));


//	}
//}

