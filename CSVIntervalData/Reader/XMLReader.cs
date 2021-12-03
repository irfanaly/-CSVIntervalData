using System.Xml;

namespace CSVIntervalData.Reader
{
    public class XMLReader
    {
        private readonly string _filename;

        public XMLReader(string filename)
        {
            _filename = filename;
        }

        public string GetData(string node)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_filename);
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes(node);
            return nodes[0].InnerText;
        }
    }
}
