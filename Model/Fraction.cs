using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MathSpace.Model
{
    public class Fraction : IBlock
    {
        private BlockNode _molecule;

        public BlockNode Molecule
        {
            get { return _molecule; }
            set { _molecule = value; }
        }

        private BlockNode _denominator;

        public BlockNode Denominator
        {
            get { return _denominator; }
            set { _denominator = value; }
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

        public void AddChildren(List<IBlock> inputCharactors)
        {
            throw new NotImplementedException();
        }
    }
}
