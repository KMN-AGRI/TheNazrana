using System;
using Microsoft.AspNetCore.Mvc;
using SharedModel.Clients.MainSite;
using SharedModel.Clients.Shared;
using SharedModel.Contexts;
using SharedModel.Helpers;
using SharedModel.Repository;

namespace BackendApi.Controllers
{
	[Route("[controller]")]
	public class AccountController:ControllerBase
	{
		private readonly MainContext context;
		private readonly IUserRepository userRepository;
		public AccountController(MainContext context, IUserRepository userRepository)
		{
			this.context = context;
			this.userRepository = userRepository;
		}

		[HttpGet]
		public IActionResult Index()
			=> Ok(context.AppUsers
				.Select(s => new
				{
					s.Id,
					s.Name,
					s.UserName,
					s.Email,
					s.DOB,
					s.ImageUri,
					s.Date,

				}).SingleOrDefault(s => s.Id == userRepository.Id()));
		[HttpPost]
		public async Task<IActionResult> updateUser([FromForm]UserProfile update)
		{
			var user = await context
				.AppUsers
				.FindAsync(userRepository.Id());

			user.Name = update.Name;
			user.DOB = update.DOB;
			if (update.Image != null)
				user.ImageUri = uploadImage(update.Image);
			context.AppUsers.Update(user);
			await context.SaveChangesAsync();
			return Ok(new ApiResponse("Profile Update Successfully", true, new
			{
				user.Id,
				user.Name,
				user.UserName,
				user.Email,
				user.DOB,
				user.ImageUri,
				user.Date,
			}));
		}

		[HttpGet("orders")]
		public IActionResult Orders()
			=> Ok(context.Orders
				.Select(s=>new
				{
					s.Id,
					s.User,
					s.Address,
					s.Total,
					s.Date,
					s.SubTotal,
					s.Discount,
					s.Status,
					Items=s.Items
					.Select(i=>new
					{
						Product=new
						{
							i.Product.Title,
							image = i.Product.Medias.Select(k => Settings.imageKitUrl + k.ServerName).FirstOrDefault()
						},
						i.Amount,
						i.Quantity,
						i.Feedback
					})
				})
				.OrderByDescending(s=>s.Date)
				.Where(s => s.User == userRepository.Id()&s.Status!=Status.Pending));

		[HttpGet("/Track/{id}")]
		public IActionResult trackOrder(string id)
			=> Ok(context.Orders.Select(s => new
			{
				s.Id,
				s.User,
				s.Address,
				s.Total,
				s.SubTotal,
				s.Discount,
				s.Status,
				s.Events,
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
					 }),
			}).SingleOrDefault(s=>s.Id==id));

		private string uploadImage(IFormFile file)
		{
			string path = "/profiles/" + String.Join("", Guid.NewGuid().ToString().Take(8))+"-" +file.FileName;
			Azure.Storage.Blobs.BlobClient blobClient = new Azure.Storage.Blobs.BlobClient(
				connectionString: Settings.blobConnection,
				blobContainerName: "thenazrana",
				blobName: path);
			blobClient.Upload(GetStream(file), true);
			return Settings.imageKitUrl+ path;

		}

		private Stream GetStream(IFormFile file)
		{
			var ms = new MemoryStream();
			file.CopyTo(ms);
			ms.Seek(0, SeekOrigin.Begin);
			return ms;
		}


	}
}

