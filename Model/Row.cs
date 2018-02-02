using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public double Location { get; set; }

        public Row()
        {
            Location = 10.023;
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
    }
}
