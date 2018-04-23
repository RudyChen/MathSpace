using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MathSpace.Model
{
    public class Row
    {
        private BlockNode _block = new BlockNode();

        public BlockNode Blocks
        {
            get { return _block; }
            set { _block = value; }
        }

        public string ID { get; set; }


        public double Location { get; set; }

        public Row()
        {
            Location = 10.023;
            ID = new Guid().ToString();
            _block.ParentId = ID;
        }

        /// <summary>
        /// 查找当前行中输入的节点
        /// </summary>
        /// <param name="inputParentId"></param>
        /// <returns></returns>
        internal IBlock FindParentNode(string inputParentId)
        {
            foreach (var item in Blocks.Children)
            {
                var node = item.FindNodeById(inputParentId);
                if (null != node)
                {
                    return node;
                }

            }

            return null;
        }

        /// <summary>
        /// 行元素序列化
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public XElement Serialize()
        {
            var type = (new Row()).GetType();
            XElement root = new XElement("Row");
            var node = Blocks.Serialize();
            root.Add(node);
            return root;
        }

        public static Row Deserialize(XmlNode xmlNode)
        {
            Row row = new Row();
            foreach (XmlNode item in xmlNode.ChildNodes)
            {
                //item.Name
               // var itemObject = item.Deserialize(item);
            }

            return row;
        }
    }
}
