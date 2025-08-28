using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSetting.Interface
{
    public interface IXMLService
    {
        string XMLPath { get; set; }
        void Write(string elementName, string elementValue);
        void Add(string elementName, string elementValue);
        string GetValues(string? condition = null);
        void LoadXML();
    }
}
