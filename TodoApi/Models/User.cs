using TodoApi.Repositories;
using System.Text.Json.Serialization;

namespace TodoApi.Models {
    public class User : IEntity<Guid> 
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        [JsonIgnore]
        public string? Password { get; set; }
        [JsonIgnore]
        public string? Salt { get; set; }

        public User
        (
            string username,
            string password
        )
        {
            Username = username;
            Password = password;
        }
    }

    public class FetchUsersMeta
    {
        public int UserCount { get; set; }
    }

    public class ResetPasswordModel
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}