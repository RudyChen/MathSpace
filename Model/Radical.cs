using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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



        public void SetBlockLocation(double locationX,double alignmentCenterY)
        {
            throw new NotImplementedException();
        }

        public Size GetSize()
        {
            throw new NotImplementedException();
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

        public IBlock FindNodeById(string id)
        {
            return null;
        }
    }
}
