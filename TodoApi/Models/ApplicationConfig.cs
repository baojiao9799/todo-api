namespace TodoApi.Models
{
    public class ApplicationConfig 
    {
        public string ConnectionString { get; set; } = "";
        public JwtSettings JwtSettings { get; set; }
    }

    public class JwtSettings
        {
            public string Secret { get; set; } = "";
            public string Audience { get; set; } = "";
            public string Issuer { get; set; } = "";
            public string Authority { get; set; } = "";
        }
}