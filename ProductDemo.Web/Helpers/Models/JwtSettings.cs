namespace ProductDemo.Web.Helpers.Models
{
    public partial class JwtSettings
    {
        public string? AccessKey { get; set; }
        public string? RefreshKey { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public double AccessTokenExpiration { get; set; }
        public double RefreshTokenExpiration { get; set; }
    }
}
