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

        /// <summary>
        /// 块ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 父容器ID
        /// </summary>
        public string ParentId { get; set; }


        public Fraction()
        {
            ID = Guid.NewGuid().ToString();
        }


        public void AddChildren(List<IBlock> inputCharactors,Point caretPoint,string parentId)
        {
            var moleculeSize = Molecule.GetSize();

        }

        public Size GetSize()
        {
            if (null==Molecule&&null== Denominator)
            {
                //返回默认分数大小
               return new Size(FontManager.Instance.FontSize, FontManager.Instance.FontSize * 2.4);
            }
            else
            {
                double maxWidth = 0;
                double maxHeight = 0;
                if (null==Molecule&&null!= Denominator)
                {
                    maxWidth = FontManager.Instance.FontSize > Denominator.GetSize().Width ? FontManager.Instance.FontSize : Denominator.GetSize().Width;
                    maxHeight = FontManager.Instance.FontSize * 1.4 + Denominator.GetSize().Height;
                    return new Size(maxWidth,maxHeight);
                }

                if (null!=Molecule&&null==Denominator)
                {
                    maxWidth = FontManager.Instance.FontSize > Molecule.GetSize().Width ? FontManager.Instance.FontSize : Molecule.GetSize().Width;
                    maxHeight = FontManager.Instance.FontSize * 1.4 + Molecule.GetSize().Height;
                    return new Size(maxWidth, maxHeight);
                }

                var topSize = Molecule.GetSize();
                var bottomSize = Denominator.GetSize();

                maxWidth = topSize.Width > bottomSize.Width ? topSize.Width : bottomSize.Width;
                return new Size(maxWidth,topSize.Height+bottomSize.Height+FontManager.Instance.FontSize*0.4);
            }
        }

        public double GetVerticalAlignmentCenter()
        {

            if (null==Molecule)
            {
                //默认情况
                return FontManager.Instance.FontSize * 1.2;
            }
            else
            {
                var moleculeSize = Molecule.GetSize();

                return moleculeSize.Height + FontManager.Instance.FontSize * 0.2;
            }
        }

        public void SetBlockLocation(double locationX, double alignmentCenterY)
        {
            throw new NotImplementedException();
        }

        public void DrawBlock(Canvas canvas)
        {
            if (null==Molecule&&null==Denominator)
            {

            }
        }

       
    }
}
