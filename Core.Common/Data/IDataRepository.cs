namespace Core.Common.Data {
    public interface IDataRepository<T> where T : class, new() {
        T Add(T entity);
        IEnumerable<T> Get();
        T Get(string id);
        IEnumerable<T> Get(int pageIndex, int pageSize, ref long totalItems);
        void Remove(string id);
        void Remove(T entity);
        T Update(T entity);
        void BulkInsert<E>(IEnumerable<E> data, string connectionName = null, List<string> excludedProperties = null);
    }
}
