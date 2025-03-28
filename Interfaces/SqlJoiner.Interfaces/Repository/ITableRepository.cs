using SqlJoiner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Interfaces.Repository
{
    public interface ITableRepository : IDatabaseDataRepository<TableOL>
    {
        Task<IEnumerable<TableOL>> CheckIfForeignKey(ColumnOL col);
    }
}
