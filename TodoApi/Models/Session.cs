namespace TodoApi.Models 
{
    public class Session 
    {
        public Guid Id { get; set; }
        public string? Token { get; set; }
        public User User { get; set; }
        public DateTime Expiration { get; set; }

        public Session(
            Guid id,
            string token,
            User user,
            DateTime expiration
        )
        {
            Id = id;
            Token = token;
            User = user;
            Expiration = expiration;
        }
    }

    public class CreateSessionMeta
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}