using SqlJoiner.Models;

namespace SqlJoiner.Interfaces.Service
{
    public interface IColumnService
    {
        Task<IEnumerable<ColumnOL>> GetByTable(TableOL table);
        Task<IEnumerable<ColumnOL>> GetAll();
        Task<IEnumerable<ColumnOL>> GetForeignKeyColumns();
    }
}
