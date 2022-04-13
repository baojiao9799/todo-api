namespace TodoApi.Models
{
    public class ApplicationConfig 
    {
        public string ConnectionString { get; set; } = "";
        public string JwtSecret { get; set; } = "";
    }
}