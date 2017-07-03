using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
