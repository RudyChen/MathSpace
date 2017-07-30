using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MathSpace.Model
{
   public class Row
    {
        private List<IBlock> _members=new List<IBlock>();

        [XmlElement(ElementName = "IBlock")]
        public List<IBlock> Members
        {
            get { return _members; }
            set { _members = value; }
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
            foreach (var item in Members)
            {
              var node=  item.FindNodeById(inputParentId);
                if (null!=node)
                {
                    return node;
                }
            }

            return null;
        }
    }
}
