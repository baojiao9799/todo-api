using System.Text.Json.Serialization;
using TodoApi.Repositories;

namespace TodoApi.Models
{
    public class Todo : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public string? Title { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; } = Status.Open;
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
    }
}