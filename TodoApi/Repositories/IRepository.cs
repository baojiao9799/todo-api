namespace TodoApi.Repositories {
    public interface IEntity<TIndex> {
        TIndex Id { get; set; }
    }

    public interface IRepository<TEntity, TIndex> {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<List<TEntity>> FetchAsync();
        Task<TEntity?> UpdateAsync(TIndex index, TEntity entity);
        Task<TEntity?> DeleteAsync(TIndex index);
    }
}