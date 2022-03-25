namespace TodoApi.Models {
    public class User {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public IEnumerable<Todo> Todos { get; set; }
    }
}