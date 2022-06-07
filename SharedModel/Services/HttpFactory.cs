using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SharedModel.Helpers;

namespace SharedModel.Services
{
	public interface IHttpFactory
	{
		Task<T> ExecuteRequest<T>(string _, object __);
	}

	public class HttpFactory:IHttpFactory
	{
		private IHttpClientFactory httpClientFactory;
		private readonly HttpClient wpClient;
		public HttpFactory(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
			wpClient = httpClientFactory.CreateClient("wpClient");
			wpClient.BaseAddress = new Uri(Settings.apiEndpoint);
			wpClient.DefaultRequestHeaders.TryAddWithoutValidation("D360-API-KEY", Settings.apiKey360);
		}


		public async Task<T> ExecuteRequest<T>(string path, object content)
		{
			var raw = JsonConvert.SerializeObject(content);
			try
			{
				var stringEnumConverter = new JsonStringEnumConverter();
				JsonSerializerOptions opts = new JsonSerializerOptions()
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				};
				opts.Converters.Add(stringEnumConverter);

				var resp = await wpClient.PostAsJsonAsync("/v1" + path, content, opts);
				var body = await resp.Content.ReadAsStringAsync();
				if (resp.StatusCode == System.Net.HttpStatusCode.BadRequest)
					Console.Write(body);
				//resp.EnsureSuccessStatusCode();
				return await resp.Content.ReadFromJsonAsync<T>();

			}
			catch (HttpRequestException e)
			{
				//loggingRepository.LogError($"Error while executing whatsapp request - {e.Message} ", e, true);
			}

			return default(T);
		}


	}
}

