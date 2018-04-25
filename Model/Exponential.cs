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
    public class Exponential : IBlock
    {
        #region Properties

        private Point _location;

        public Point Location
        {
            get { return _location; }
            set { _location = value; }
        }

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

        private string _parentId;

        public string ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private double _proportion = 0.666;
        /// <summary>
        /// 分数位置比例
        /// </summary>
        public double Proportion
        {
            get { return _proportion; }
            set { _proportion = value; }
        }

        #endregion

        public Exponential()
        {
            ID = Guid.NewGuid().ToString();
        }


        public void SetBlockLocation(double locationX, double alignmentCenterY, double rowY)
        {
            var selfAlignmentCenter = GetVerticalAlignmentCenter();
            this.Location = new Point(locationX, alignmentCenterY - selfAlignmentCenter);

            var baseAlignmentCenter = FontManager.Instance.FontSize / 2;

            if (null!=Base)
            {
                baseAlignmentCenter= Base.GetVerticalAlignmentCenter();
                Base.Location = new Point(locationX, alignmentCenterY - baseAlignmentCenter);
                Base.SetBlockLocation(locationX, alignmentCenterY, rowY);
            }
            


            var baseSize = GetBaseSize();
            var indexSize = GetIndexSize();
            if (null != Index)
            {                
                var indexAlignmentCenter = Index.GetVerticalAlignmentCenter();
                Index.Location = new Point(locationX+baseSize.Width,this.Location.Y);
                Index.SetBlockLocation(locationX+baseSize.Width,Index.Location.Y+indexAlignmentCenter,rowY);
            }            
        }

        public Size GetSize()
        {
            Size indexSize;
            Size baseSize;
            Size exponentialSize;

            indexSize = GetIndexSize();
            baseSize = GetBaseSize();

            exponentialSize = new Size(baseSize.Width + indexSize.Width, baseSize.Height + indexSize.Height * Proportion);
            return exponentialSize;
        }

        private Size GetIndexSize()
        {
            Size indexSize;
            if (null == Index)
            {
                indexSize = new Size(FontManager.Instance.FontSize / 2, FontManager.Instance.FontSize);
            }
            else
            {
                indexSize = Index.GetSize();
            }

            return indexSize;
        }

        private Size GetBaseSize()
        {
            Size baseSize;
            if (null == Base)
            {
                baseSize = new Size(FontManager.Instance.FontSize / 2, FontManager.Instance.FontSize);
            }
            else
            {
                baseSize = Base.GetSize();
                baseSize = new Size(baseSize.Width+2,baseSize.Height);
            }
            return baseSize;
        }

        public double GetVerticalAlignmentCenter()
        {
            var indexSize = GetIndexSize();
            var baseSize = GetBaseSize();

            double baseVerticalAlignmentCenter = FontManager.Instance.FontSize / 2;
            if (null!=Base)
            {
                baseVerticalAlignmentCenter= Base.GetVerticalAlignmentCenter(); 
            }

            return indexSize.Height * Proportion + baseVerticalAlignmentCenter;
        }

        private Rectangle CreateRangeRect(Rect rect)
        {
            Rectangle indexRectangle = new Rectangle();
            indexRectangle.StrokeDashArray = new DoubleCollection() { 2, 1 };
            indexRectangle.Stroke = Brushes.Black;
            indexRectangle.StrokeThickness = 1;
            indexRectangle.Width = rect.Width;
            indexRectangle.Height = rect.Height;
            Canvas.SetLeft(indexRectangle, rect.Left);
            Canvas.SetTop(indexRectangle, rect.Top);
            return indexRectangle;
        }

        public void DrawBlock(Canvas canvas)
        {
            var indexRect = GetIndexRect();
            var baseRect = GetBaseRect();
            var indexRectangle = CreateRangeRect(indexRect);
            var baseRectangle = CreateRangeRect(baseRect);

            canvas.Children.Add(indexRectangle);
            canvas.Children.Add(baseRectangle);
            if (null != Base)
            {
                Base.DrawBlock(canvas);
            }

            if (null != Index)
            {
                Index.DrawBlock(canvas);
            }
        }

        private Rect GetBaseRect()
        {
            var rect = GetIndexSize();

            var baseRect = GetBaseSize();

            return new Rect(Location.X, Location.Y + rect.Height * Proportion, baseRect.Width, baseRect.Height);
        }

        private Rect GetIndexRect()
        {
            var rect = GetIndexSize();

            var baseRect = GetBaseSize();

            return new Rect(Location.X + baseRect.Width, Location.Y, rect.Width, rect.Height);
        }

        private bool IsIndexContainsCaret(Point caretPoint)
        {
            var indexRect = GetIndexRect();
            var indexExpandRect = new Rect(indexRect.X , indexRect.Y - 2, indexRect.Width + 2, indexRect.Height);
            if (indexExpandRect.Contains(caretPoint))
            {
                return true;
            }
            return false;
        }

        private bool IsBaseContainsCaret(Point caretPoint)
        {
            var baseRect = GetBaseRect();
            var baseExpandRect = new Rect(baseRect.X - 2, baseRect.Y - 2, baseRect.Width + 2, baseRect.Height + 2);
            if (baseExpandRect.Contains(caretPoint))
            {
                return true;
            }

            return false;
        }

        public void AddChildren(IEnumerable<IBlock> inputCharactors, Point caretPoint, string parentId)
        {         
            if (IsIndexContainsCaret(caretPoint))
            {
                if (null==Index)
                {
                    Index = new BlockNode();
                }
                Index.AddChildren(inputCharactors,caretPoint,parentId);
            }
            if (IsBaseContainsCaret(caretPoint))
            {
                if (null==Base)
                {
                    Base = new BlockNode();
                }
                Base.AddChildren(inputCharactors,caretPoint,parentId);
            }

        }

        public IBlock FindNodeById(string id)
        {
            if (id == ID)
            {
                return this;
            }
            else
            {
                if (null != Index)
                {
                    var node = Index.FindNodeById(id);
                    if (null != node)
                    {
                        return node;
                    }
                }

                if (null != Base)
                {
                    var deNode = Base.FindNodeById(id);
                    if (null != deNode)
                    {
                        return deNode;
                    }
                }

                return null;
            }
        }

        public Point GotoNextPart(Point caretLocation)
        {
            if (IsBaseContainsCaret(caretLocation))
            {
                var indexRect=GetIndexRect();
                return indexRect.Location;
            }
            else 
            {
                var baseRect = GetBaseRect();
                var exponentialSize = GetSize();
                var baseVerticalAlignmentCenter = Base.GetVerticalAlignmentCenter();
                MessageManager.Instance.OnInputParentChanged(ParentId);
                double locationY = Base.Location.Y + baseVerticalAlignmentCenter-FontManager.Instance.FontSize/2;
                return new Point(Location.X+ exponentialSize.Width, locationY);
            }

        }

        public Point GotoPreviousPart(Point caretLocation)
        {
            throw new NotImplementedException();
        }

        public XElement Serialize()
        {
            var typeFraction = (new Exponential()).GetType();
            XElement fraction = new XElement("Exponential");
            XElement molecule = new XElement("Base");
            XElement denominator = new XElement("Index");

            XElement baseBlockNode = Base.Serialize();
            molecule.Add(baseBlockNode);

            XElement indexBlockNode = Index.Serialize();
            denominator.Add(indexBlockNode);

            fraction.Add(molecule);
            fraction.Add(denominator);
            return fraction;
        }

        public void GetElementBeforeCaret(Point caretLocation)
        {
            if (null!=Base)
            {
                Base.GetElementBeforeCaret(caretLocation);
                
            }

            if (null!=Index)
            {
                Index.GetElementBeforeCaret(caretLocation);
            }
        }

        public string GetParentId()
        {
            return ParentId;
        }

        public void RemoveChild(IBlock block)
        {
            if (Index!=null)
            {
                Index.RemoveChild(block);
            }

            if (Base!=null)
            {
                Base.RemoveChild(block);
            }
        }

        public Point GetBlockLocation()
        {
            return Location;
        }

        public IBlock GetCaretBrotherElement(bool before, Point caretPoint)
        {
            if (null!=Base)
            {
                var block = Base.GetCaretBrotherElement(before,caretPoint);
                if (null!=block)
                {
                    return block;
                }
            }

            if (null!=Index)
            {
                var block = Index.GetCaretBrotherElement(before,caretPoint);
                if (null!=block)
                {
                    return block;
                }
            }

            return null;
        }

        public void AddChildrenAfterBlock(IBlock block, IEnumerable<IBlock> inputCharactors)
        {
            if (Index!=null)
            {
                if (Index.Children.Contains(block))
                {
                    Index.AddChildrenAfterBlock(block,inputCharactors);
                }
            }

            if (Base!=null)
            {
                if (Base.Children.Contains(block))
                {
                    Base.AddChildrenAfterBlock(block,inputCharactors);
                }
            }
        }
    }
}
