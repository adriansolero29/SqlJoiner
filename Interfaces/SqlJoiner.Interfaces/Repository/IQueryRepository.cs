using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Interfaces.Repository
{
    public interface IQueryRepository
    {
        Task<string> RawSqlToJson(string query);
    }
}
