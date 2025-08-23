using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Interfaces.Service
{
    public interface IQueryService
    {
        Task<string> RawQueryToJson(string query);
    }
}
