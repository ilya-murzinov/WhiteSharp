using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Castle.Core.Internal;

namespace WhiteSharp.Helpers
{
    public class XmlHelper
    {
        private readonly XDocument _testData;
        private XElement _node;

        private XmlHelper(string fileName)
        {
            _testData = XDocument.Load(fileName);
        }

        public static XmlHelper Load(string fileName)
        {
            return new XmlHelper(fileName);
        }

        public string GetValue(string nodeName)
        {
            try
            {
                if (_node == null)
                {
                    return _testData.Descendants(nodeName).First().FirstAttribute.Value;
                }

                string returnValue = _node.Descendants(nodeName).First().FirstAttribute.Value;
                _node = null;
                return returnValue;
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception)
            {
                Logging.Info(String.Format("Node \"{0}\" not found", nodeName));
            }
            _node = null;
            return null;
        }

        public string[] GetValues(string nodeName)
        {
            try
            {
                IEnumerable<XAttribute> attributes;
                if (_node == null)
                {
                    attributes = _testData.Descendants(nodeName).First().Attributes();
                }
                else
                {
                    attributes = _node.Descendants(nodeName).First().Attributes();
                    _node = null;
                }

                var returnValue = new List<String>();
                attributes.ForEach(x => returnValue.Add(x.Value));
                return returnValue.ToArray();
            }
            catch (Exception)
            {
                Logging.Info(String.Format("Node \"{0}\" not found", nodeName));
            }

            return null;
        }

        public XmlHelper GetCategory(string nodeName)
        {
            try
            {
                _node = _node == null
                    ? _testData.Descendants(nodeName).First()
                    : _node.Descendants(nodeName).First();

                return this;
            }
            catch (Exception)
            {
                Logging.Info(String.Format("Node \"{0}\" not found", nodeName));
            }

            return this;
        }
    }
}