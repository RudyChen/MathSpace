using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MathSpace.Model
{
    public class BlockNode :IBlock
    {       
        private List<IBlock> _children=new List<IBlock>();
        /// <summary>
        /// 子节点
        /// </summary>
        public List<IBlock> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public Point Location { get; set; }

        /// <summary>
        /// 块ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 父容器ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 输入部分索引
        /// </summary>
        public int InputPartIndex { get; set; }

        public BlockNode()
        {
            ID = Guid.NewGuid().ToString();
        }


        public void SetBlockLocation(double locationX,double alignmentCenterY)
        {
            
        }

       

        public Size GetSize()
        {
            double width = 0;
            double maxHeight = 0;
            
            foreach (var item in Children)
            {
               var itemSize= item.GetSize();
                width += itemSize.Width;
                if (itemSize.Height>maxHeight)
                {
                    maxHeight = itemSize.Height;
                }
            }

            return new Size(width,maxHeight);
        }

        public double GetVerticalAlignmentCenter()
        {
            throw new NotImplementedException();
        }
        public void DrawBlock(Canvas canvas)
        {
            throw new NotImplementedException();
        }


        public void AddChildren(IEnumerable<IBlock> inputCharactors,Point caretPoint, string parentId)
        {
            throw new NotImplementedException();
        }

        public void AddChildren(IBlock inputCharactors)
        {
            throw new NotImplementedException();
        }

        public IBlock FindNodeById(string id)
        {
            if (ID==id)
            {
                return this;
            }
            else
            {
                foreach (var item in Children)
                {
                    var node=item.FindNodeById(id);
                    if (node!=null)
                    {
                        return node;
                    }
                }

                return null;
            }
        }
    }
}
