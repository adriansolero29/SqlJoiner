using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Interfaces.DataAccess
{
    public interface IDataConnectionInitializer
    {
        void InitializeConnectionAsync();
        void OpenConnectionAsync();
        void CloseConnection();
    }
}
