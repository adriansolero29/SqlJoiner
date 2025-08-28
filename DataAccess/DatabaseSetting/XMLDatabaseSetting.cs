using DatabaseSetting.Interface;
using DatabaseSetting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DatabaseSetting
{
    public class XMLDatabaseSetting : IXMLService, IXMLDatabaseSetting
    {
        public string? XMLPath { get; set; }
        private XmlDocument? xmlDoc;

        public XMLDatabaseSetting()
        {
            LoadXML();
            GetValues();
        }

        public string GetValues(string? condition = null)
        {

            if (File.Exists(XMLPath))
            {
                DataCommunication.ServerName = xmlDoc?.DocumentElement?.ChildNodes[0]?.InnerText;
                DataCommunication.PortNumber = xmlDoc?.DocumentElement?.ChildNodes[1]?.InnerText;
                DataCommunication.DatabaseName = xmlDoc?.DocumentElement?.ChildNodes[2]?.InnerText;
                DataCommunication.UserId = xmlDoc?.DocumentElement?.ChildNodes[3]?.InnerText;
                DataCommunication.Password = xmlDoc?.DocumentElement?.ChildNodes[4]?.InnerText;
            }
            else
                throw new FileNotFoundException("Database setting xml not found");

            return string.Empty;
        }

        public void Write(string elementName, string elementValue)
        {
            XDocument xdoc = XDocument.Load(XMLPath!);
            XElement element = xdoc.Element("DatabaseSetting");
            element!.SetElementValue(elementName, elementValue);

            xdoc.Save(XMLPath!);
        }

        public void ChangeDatabaseSetting()
        {
            Write("ServerName", DataCommunication.ServerName ?? "");
            Write("PortNumber", DataCommunication.PortNumber ?? "");
            Write("DatabaseName", DataCommunication.DatabaseName ?? "");
            Write("UserId", DataCommunication.UserId ?? "");
            Write("Password", DataCommunication.Password ?? "" );
        }

        public void Add(string elementName, string elementValue)
        {
            throw new NotImplementedException();
        }

        public void LoadXML()
         {
            this.xmlDoc = new XmlDocument();
            var xmlpath = Path.Combine(Environment.CurrentDirectory, "DatabaseSetting.xml");
            if (File.Exists(xmlpath))
            {
                xmlDoc.Load(this.XMLPath!);
            }
            else
            {
                this.XMLPath = Path.Combine(Environment.CurrentDirectory, "DatabaseSetting.xml");
            }
        }
    }
}
