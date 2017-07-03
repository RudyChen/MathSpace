using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Model
{
    public class Radical : IBlock
    {
        private BlockNode _rootIndex;

        public BlockNode RootIndex
        {
            get { return _rootIndex; }
            set { _rootIndex = value; }
        }

        private BlockNode _radicand;

        public BlockNode Radicand
        {
            get { return _radicand; }
            set { _radicand = value; }
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
