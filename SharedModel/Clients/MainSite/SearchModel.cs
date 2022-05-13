using System;
namespace SharedModel.Clients.MainSite
{

	public class SearchRequest
	{
		public string query { get; set; }
		public string category { get; set; }
		public List<ItemRange> discountRanges { get; set; } = new List<ItemRange>();
		public List<ItemRange> priceRanges { get; set; } = new List<ItemRange>();
		public int page { get; set; } = 1;
	}

	public class SearchResponse
	{
		public string title { get; set; }
		public List<SearchProduct> items { get; set; }
		public bool hasMore { get; set; }
		public int total { get; set; }
		public int nextPage { get; set; }
		public string curRange { get; set; }
	}

	public class SearchProduct
	{
		public string brand { get; set; }
		public string title { get; set; }
		public string image { get; set; }
		public float discount { get; set; }
		public float price { get; set; }
		public float mrp { get; set; }
	}

	public class ItemRange
	{
		public int min { get; set; }
		public int max { get; set; }
	}


}

