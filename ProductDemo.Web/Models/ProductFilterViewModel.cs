namespace ProductDemo.Web.Models
{
    public class ProductFilterViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
