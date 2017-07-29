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
            if (null==Molecule&&null== Denominator)
            {
                //返回默认分数大小
               return new Size(MathEidtor.FontSize, MathEidtor.FontSize * 2.4);
            }
            else
            {
                double maxWidth = 0;
                double maxHeight = 0;
                if (null==Molecule&&null!= Denominator)
                {
                    maxWidth = MathEidtor.FontSize > Denominator.GetSize().Width ? MathEidtor.FontSize : Denominator.GetSize().Width;
                    maxHeight = MathEidtor.FontSize * 1.4 + Denominator.GetSize().Height;
                    return new Size(maxWidth,maxHeight);
                }

                if (null!=Molecule&&null==Denominator)
                {
                    maxWidth = MathEidtor.FontSize > Molecule.GetSize().Width ? MathEidtor.FontSize : Molecule.GetSize().Width;
                    maxHeight = MathEidtor.FontSize * 1.4 + Molecule.GetSize().Height;
                    return new Size(maxWidth, maxHeight);
                }

                var topSize = Molecule.GetSize();
                var bottomSize = Denominator.GetSize();

                maxWidth = topSize.Width > bottomSize.Width ? topSize.Width : bottomSize.Width;
                return new Size(maxWidth,topSize.Height+bottomSize.Height+MathEidtor.FontSize*0.4);
            }
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
