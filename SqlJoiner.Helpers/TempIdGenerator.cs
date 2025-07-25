using SqlJoiner.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Helpers
{
    public static class TempIdGenerator
    {
        public static void GenerateTempIdForList<T>(IEnumerable<T> objList) where T : IModel
        {
            objList.ToList().ForEach(x => x.TempId = GuidGenerator.Generate());
        }
    }

    public static class GuidGenerator
    {
        public static string? Generate()
        {
            return Guid.NewGuid().ToString() ?? "";
        }
    }
}
