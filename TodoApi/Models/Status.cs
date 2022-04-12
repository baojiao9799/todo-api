namespace TodoApi.Models {
     public enum Status
    {
        [NpgsqlTypes.PgName("open")]
        Open,
        [NpgsqlTypes.PgName("in_progress")]
        InProgress,
        [NpgsqlTypes.PgName("complete")]
        Complete
    }
}