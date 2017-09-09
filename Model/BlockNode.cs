using MathSpace.Tool;
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
            if (null==Children||Children.Count==0)
            {
                return new Size(FontManager.Instance.FontSize, FontManager.Instance.FontSize*1.2);
            }
            
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
            if (Children.Count>0)
            {
                foreach (var item in Children)
                {
                    item.DrawBlock(canvas);
                }
            }
        }


        public void AddChildren(IEnumerable<IBlock> inputCharactors,Point caretPoint, string parentId)
        {
            //查找到索引，在索引处添加入集合即可     
            int index = 0;
            if (Children.Count>0)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    var itemSize = Children[i].GetSize();
                    var expandSize = new Size(itemSize.Width + 2, itemSize.Height + 2);
                    var itemRect = new Rect(Location, expandSize);

                    if (itemRect.Contains(caretPoint))
                    {
                        index = i;
                        break;
                    }
                }
            }

            foreach (var item in inputCharactors)
            {
                Children.Insert(index,item);
                index++;
            }
        }

        public void AddChildren(IBlock inputCharactors)
        {
            Children.Add(inputCharactors);
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
