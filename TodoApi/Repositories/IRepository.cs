namespace TodoApi.Repositories {
    public interface IEntity<TIndex> {
        TIndex Id { get; set; }
    }
    public interface IRepository<TEntity, TIndex>
        where TEntity : IEntity<TIndex>
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAsync();
        Task<TEntity?> FindAsync(params object[] keyValues);
        Task<TEntity?> UpdateAsync(TIndex index, TEntity entity);
        Task<TEntity?> DeleteAsync(TIndex index);
    }
}