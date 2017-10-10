using MathSpace.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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

        public Point Location { get; set; }

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


        public void AddChildren(IEnumerable<IBlock> inputCharactors,Point caretPoint,string parentId)
        {
            Rect moleculeRect;
            Rect denominatorRect;
            if (null==Molecule)
            {
                moleculeRect = new Rect(Location.X-2, Location.Y-4, FontManager.Instance.FontSize+4, FontManager.Instance.FontSize * 1.2+4);               

            }
            else
            {                
                moleculeRect = new Rect(Location.X-2,Location.Y-4, Molecule.GetSize().Width+4, Molecule.GetSize().Height+4);
            }

            if (null==Denominator)
            {
                denominatorRect= new Rect(Location.X-2, Location.Y + FontManager.Instance.FontSize * 1.2-2, FontManager.Instance.FontSize+4, FontManager.Instance.FontSize+4);
            }
            else
            {
                denominatorRect = new Rect(Location.X-2,Location.Y+moleculeRect.Height+FontManager.Instance.FontSize*0.2-2,Denominator.GetSize().Width+4,Denominator.GetSize().Height+4);
            }

            if (moleculeRect.Contains(caretPoint))
            {
                if (null==Molecule)
                {
                    Molecule = new BlockNode();                   
                }
                Molecule.AddChildren(inputCharactors, caretPoint, parentId);
            }
            else if(denominatorRect.Contains(caretPoint))
            {
                if (null==Denominator)
                {
                    Denominator = new BlockNode();
                }
                Denominator.AddChildren(inputCharactors,caretPoint,parentId);
            }
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

        public void SetBlockLocation(double locationX, double alignmentCenterY, double rowY)
        {
            var y = rowY + GetVerticalAlignmentCenter();
           
            if (null!=Molecule)
            {
                var locationY = y - Molecule.GetSize().Height;
                Location = new Point(locationX, locationY);
                Molecule.Location = new Point(locationX, y);
                Molecule.SetBlockLocation(locationX, y,rowY);
            }
            else
            {
                var locationY = y - FontManager.Instance.FontSize * 1.2;
                Location = new Point(locationX, locationY);
            }
            if (null!=Denominator)
            {
                Denominator.Location = new Point(locationX, alignmentCenterY + FontManager.Instance.FontSize * 0.2);
                Denominator.SetBlockLocation(locationX, alignmentCenterY + FontManager.Instance.FontSize * 0.2,rowY);
            }            
        }

        public void DrawBlock(Canvas canvas)
        {
            var line = CreateFractionLine();
            canvas.Children.Add(line);
            if (null!=Molecule)
            {
                Molecule.DrawBlock(canvas);
            }
            if (null!= Denominator)
            {
                Denominator.DrawBlock(canvas);
            }
        }

        private Line CreateFractionLine()
        {
            Line separateLine = new Line();
            separateLine.Uid = ID;
            separateLine.Stroke = Brushes.Black;
            separateLine.X1 = Location.X;
            if (null!=Molecule)
            {
                separateLine.Y1 = Location.Y + Molecule.GetSize().Height + FontManager.Instance.FontSize * 0.2;
                separateLine.X2 = Location.X + Molecule.GetSize().Width;
            }
            else
            {
                separateLine.Y1 = Location.Y +  FontManager.Instance.FontSize * 1.2;
                separateLine.X2 = Location.X + FontManager.Instance.FontSize;
            }
            separateLine.Y2 = separateLine.Y1;

            return separateLine;
        }

        public IBlock FindNodeById(string id)
        {
            if (id==ID)
            {
                return this;
            }
            else
            {
                if (null!=Molecule)
                {
                    var node=Molecule.FindNodeById(id);
                    if (null!=node)
                    {
                        return node;
                    }
                }

                if (null!=Denominator)
                {
                    var deNode = Denominator.FindNodeById(id);
                    if (null!=deNode)
                    {
                        return deNode;
                    }
                }

                return null;
            }
        }
    }
}
