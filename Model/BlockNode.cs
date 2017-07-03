using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Model
{
    public class BlockNode :IBlock
    {       
        private List<IBlock> _children=new List<IBlock>();

        public List<IBlock> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        

        public void AlignElementsToVerticalCenter()
        {
            throw new NotImplementedException();
        }

        public double DrawBlock()
        {
            throw new NotImplementedException();
        }

        public double GetRect()
        {
            throw new NotImplementedException();
        }

        public double GetVerticalAlignmentCenter()
        {
            throw new NotImplementedException();
        }
    }
}
