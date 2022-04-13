namespace TodoApi.Models {
    public class Session {
        public Guid Id { get; set; }
        public string? Token { get; set; }
        //public User User { get; set; }
        public DateTime Expiration { get; set; }

        public Session(
            Guid id,
            string token
        )
        {
            Id = id;
            Token = token;
        }
    }
}