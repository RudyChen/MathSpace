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
using System.Xml.Linq;

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

        private double _separateLineY;
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
                moleculeRect = new Rect(Location.X-2, Location.Y-4, FontManager.Instance.FontSize+4, FontManager.Instance.FontSize +4);               

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
            else
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
            double moleculeY = 0;
            if (null!=Molecule)
            {
               var maxMoleculeCenter= Molecule.GetVerticalAlignmentCenter();
                var locationY = alignmentCenterY - Molecule.GetSize().Height- FontManager.Instance.FontSize * 0.2;
                Location = new Point(locationX, locationY);
                Molecule.Location = new Point(locationX, locationY);
                Molecule.SetBlockLocation(locationX, locationY+ maxMoleculeCenter, rowY);
                moleculeY= locationY;
            }
            else
            {
                var locationY = alignmentCenterY - FontManager.Instance.FontSize * 1.2;
                Location = new Point(locationX, locationY);
                moleculeY = locationY;
            }
            if (null!=Denominator)
            {
                var maxDenominatorCenter = Denominator.GetVerticalAlignmentCenter();
                Denominator.Location = new Point(locationX, alignmentCenterY  + FontManager.Instance.FontSize * 0.2);
                Denominator.SetBlockLocation(locationX, Denominator.Location.Y+maxDenominatorCenter,rowY);
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
            double maxLineWidth = 0;
            if (null!=Molecule)
            {
                separateLine.Y1 = Location.Y + Molecule.GetSize().Height + FontManager.Instance.FontSize * 0.2;
                maxLineWidth = Molecule.GetSize().Width;
                separateLine.X2 = Location.X + maxLineWidth;
            }
            else
            {
                separateLine.Y1 = Location.Y +  FontManager.Instance.FontSize * 1.2;
                separateLine.X2 = Location.X + FontManager.Instance.FontSize;
            }

            if (null!=Denominator)
            {
                var denominatorMaxLineWidth = Denominator.GetSize().Width;
                if (denominatorMaxLineWidth>maxLineWidth)
                {
                    separateLine.X2 = Location.X + denominatorMaxLineWidth;
                }
            }
           
            separateLine.Y2 = separateLine.Y1;
            _separateLineY = separateLine.Y1;

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

        public Point GotoNextPart(Point caretLocation)
        {
            /*1.当前输入分子；2.当前输入分母*/
            bool isInputMolecule = IsContainCaretLocation(Molecule,caretLocation);
            
            if (isInputMolecule)
            {
                if (null== Denominator)
                {
                    if (null==Molecule)
                    {
                        return new Point(this.Location.X, this.Location.Y + FontManager.Instance.FontSize * 1.4);
                    }
                    else
                    {
                        return new Point(this.Location.X, this.Location.Y +Molecule.GetSize().Height+ FontManager.Instance.FontSize * 0.4);
                    }
                    
                }
                else
                {
                    var denominatorSize = Denominator.GetSize();

                    return new Point(Denominator.Location.X+denominatorSize.Width, Denominator.Location.Y);
                }
            }
            else
            {
                var fractionSize = GetSize();
                MessageManager.Instance.OnInputParentChanged(ParentId);
                return new Point(Location.X + fractionSize.Width, Location.Y + Molecule.GetSize().Height-FontManager.Instance.FontSize*0.3);

            }

        }

        public Point GotoPreviousPart(Point caretLocation)
        {
            throw new NotImplementedException();
        }

        public bool IsContainCaretLocation(BlockNode node,Point caretLocation)
        {
            //光标位置可能不在输入范围内，所以要找一个中心点判断
            var middlePoint = new Point(caretLocation.X - 6, caretLocation.Y + FontManager.Instance.FontSize / 2);
            Size nodeSize;
            if (null==node)
            {
                nodeSize= new Size(FontManager.Instance.FontSize, FontManager.Instance.FontSize * 1.2);
            }
            else
            {
                nodeSize = node.GetSize();
            }

            
            var nodeRect = new Rect(Location,nodeSize);
            
            if (nodeRect.Contains(middlePoint))
            {
                return true;
            }

            return false;
        }

        public XElement Serialize()
        {
            var typeFraction = (new Fraction()).GetType();
            XElement fraction = new XElement("Fraction");
            XElement molecule = new XElement("Molecule");
            XElement denominator = new XElement("Denominator");

            XElement moleculeBlockNode = Molecule.Serialize();
            molecule.Add(moleculeBlockNode);

            XElement denominatorBlockNode = Denominator.Serialize();
            denominator.Add(denominatorBlockNode);

            fraction.Add(molecule);
            fraction.Add(denominator);
            return fraction;
        }

        public IBlock GetElementBeforeCaret(Point caretLocation)
        {
            if (null!=Molecule)
            {
                var block=Molecule.GetElementBeforeCaret(caretLocation);
                if (null!=block)
                {
                    return block;
                }
            }

            if (null!=Denominator)
            {
                var blockNode = Denominator.GetElementBeforeCaret(caretLocation);
                if (null!=blockNode)
                {
                    return blockNode;
                }
            }

            return null;
        }
    }
}
