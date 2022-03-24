using System.Text.Json.Serialization;

namespace TodoApi.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; } = Status.Open;
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
    }

    public enum Status
    {
        Open,
        InProgress,
        Complete
    }
}