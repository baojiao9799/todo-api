using TodoApi.Repositories;

namespace TodoApi.Models {
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public static UserDTO UserToDTO(User user) =>
        new UserDTO
        {
            Id = user.Id,
            Username = user.Username
        };
    }
    public class User : IEntity<Guid> 
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class ResetPasswordModel
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}