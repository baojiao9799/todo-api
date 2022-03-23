using TodoApi.Repositories;

namespace TodoApi.Models{
    public class Todo : IEntity<Guid> {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public bool Status { get; set; } = false;
        public DateTime CreationDate { get; private set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
    }

    enum Status {
        Open,
        InProgress,
        Complete
    }
}