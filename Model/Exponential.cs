using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Model
{
    public class Exponential : IBlock
    {
        private BlockNode _base;

        public BlockNode Base
        {
            get { return _base; }
            set { _base = value; }
        }

        private BlockNode _index;

        public BlockNode Index
        {
            get { return _index; }
            set { _index = value; }
        }


        public void AlignElementsToVerticalCenter()
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

        public double DrawBlock()
        {
            throw new NotImplementedException();
        }
    }
}
