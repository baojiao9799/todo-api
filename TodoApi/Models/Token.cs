using TodoApi.Repositories;

namespace TodoApi.Models
{
    public class Token : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? JwtToken { get; set; }
    }
}