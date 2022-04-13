using TodoApi.Repositories;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class Token : IEntity<Guid>
    {
        public Guid Id { get; set; }
        [Column("token")]
        public string? JwtToken { get; set; }

        public Token(string jwtToken)
        {
            JwtToken = jwtToken;
        }
    }
}