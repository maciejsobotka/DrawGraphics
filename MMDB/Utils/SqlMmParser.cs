using System.Collections.Generic;
using System.Xml;

namespace MMDB.Utils
{
    public static class SqlMmParser
    {
        #region Constants

        private static readonly Dictionary<string, string> m_ColorDict;

        #endregion
        #region Ctors

        static SqlMmParser()
        {
            m_ColorDict = new Dictionary<string, string>
            {
                {"White", "#FFFFFFFF"},
                {"Black", "#FF000000"},
                {"Gray", "#FF808080"},
                {"Red", "#FFFF0000"},
                {"Green", "#FF008000"},
                {"Blue", "#FF0000FF"}
            };
        }

        #endregion
        #region Public static methods

        public static bool SearchResult(string fileName, string shapeName, int shapeCount, string comparison, string attrName, string attrVal)
        {
            var xml = new XmlDocument();
            xml.Load(fileName);
            var nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            var nodeList = xml.SelectNodes("//x:Canvas/x:" + shapeName + "[@" + attrName + "='" + m_ColorDict[attrVal] + "']", nsMgr);
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

        #endregion
    }
}