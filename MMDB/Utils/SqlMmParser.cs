using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MMDB.Utils
{
    public static class SqlMmParser
    {
        public static bool SearchResult(string fileName, string shapeName, int shapeCount, string comparison, string attrName, string attrVal)
        {
            var xml = new XmlDocument();
            xml.Load(fileName);
            var nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            var nodeList = xml.SelectNodes("//x:Canvas/x:" + shapeName + "[@" + attrName + "='" + attrVal + "']", nsMgr);
            if (nodeList != null)
            {
                switch (comparison)
                {
                    case "==":
                        if (nodeList.Count == shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "!=":
                        if (nodeList.Count != shapeCount)
                        {
                            return true;
                        }
                        break;
                    case ">":
                        if (nodeList.Count > shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "<":
                        if (nodeList.Count < shapeCount)
                        {
                            return true;
                        }
                        break;
                    case ">=":
                        if (nodeList.Count >= shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "<=":
                        if (nodeList.Count <= shapeCount)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        public static bool SearchResult(string fileName, string shapeName, int shapeCount, string comparison)
        {
            var xml = new XmlDocument();
            xml.Load(fileName);
            var nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            var nodeList = xml.SelectNodes("//x:Canvas/x:" + shapeName + "", nsMgr);
            if (nodeList != null)
            {
                switch (comparison)
                {
                    case "==":
                        if (nodeList.Count == shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "!=":
                        if (nodeList.Count != shapeCount)
                        {
                            return true;
                        }
                        break;
                    case ">":
                        if (nodeList.Count > shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "<":
                        if (nodeList.Count < shapeCount)
                        {
                            return true;
                        }
                        break;
                    case ">=":
                        if (nodeList.Count >= shapeCount)
                        {
                            return true;
                        }
                        break;
                    case "<=":
                        if (nodeList.Count <= shapeCount)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }
    }
}
