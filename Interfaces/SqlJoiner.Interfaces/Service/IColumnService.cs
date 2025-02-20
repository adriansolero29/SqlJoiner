using SqlJoiner.Models;

namespace SqlJoiner.Interfaces.Service
{
    public interface IColumnService
    {
        Task<IEnumerable<ColumnOL>> GetByTable(TableOL table);
    }
}
