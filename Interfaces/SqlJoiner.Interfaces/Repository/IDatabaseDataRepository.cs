using SqlJoiner.Models.Base;

namespace SqlJoiner.Interfaces.Repository
{
    public interface IDatabaseDataRepository<Model> where Model : IModel
    {
        Task<IEnumerable<Model>> GetAllAsync();
        Task<Model> GetFirstAsync(string condition);
        Task<IEnumerable<Model>> GetByConditionAsync(string condition);
    }
}
