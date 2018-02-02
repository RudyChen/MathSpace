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

        private string _parentId;

        public string ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        public Point Location { get; set; }
        public string ID { get; private set; }

        public Radical()
        {
            ID = Guid.NewGuid().ToString();
        }

        public void SetBlockLocation(double locationX, double alignmentCenterY, double rowY)
        {
            var rootindexSize = GetRootIndexSize();
            double rootIndexY = alignmentCenterY - GetVerticalAlignmentCenter();
            this.Location = new Point(locationX, rootIndexY);
            if (null != RootIndex)
            {
                RootIndex.Location = new Point(locationX, rootIndexY);
                var rootIndexVerticalCenter = RootIndex.GetVerticalAlignmentCenter();
                RootIndex.SetBlockLocation(locationX, RootIndex.Location.Y + rootIndexVerticalCenter, rowY);
            }

            if (null != Radicand)
            {
                Radicand.Location = new Point(locationX + rootindexSize.Width + 10, this.Location.Y + rootindexSize.Height / 3);
                double radicandVerticalCenter=Radicand.GetVerticalAlignmentCenter();
                Radicand.SetBlockLocation(Radicand.Location.X, Radicand.Location.Y+ radicandVerticalCenter,rowY);
            }
        }

        private Size GetRootIndexSize()
        {
            if (null == RootIndex)
            {
                return new Size(FontManager.Instance.FontSize / 2, FontManager.Instance.FontSize);
            }
            else
            {
                return RootIndex.GetSize();
            }
        }

        private Size GetRadicandSize()
        {
            if (null == Radicand)
            {
                return new Size(FontManager.Instance.FontSize, FontManager.Instance.FontSize);
            }
            else
            {
                return Radicand.GetSize();
            }
        }

        private Rect GetRootIndexRect()
        {
            var rootIndexSize = GetRootIndexSize();
            return new Rect(Location, rootIndexSize);
        }

        private Rect GetRadicandRect()
        {
            var rootIndexSize = GetRootIndexSize();
            var radicandSize = GetRadicandSize();

            return new Rect(Location.X + rootIndexSize.Width + 10, Location.Y + rootIndexSize.Height / 3, radicandSize.Width, radicandSize.Height);
        }

        public Size GetSize()
        {
            var rootIndexSize = GetRootIndexSize();
            var radicandSize = GetRadicandSize();

            return new Size(rootIndexSize.Width+10 + radicandSize.Width, rootIndexSize.Height / 3 + radicandSize.Height);
        }



        public double GetVerticalAlignmentCenter()
        {
            Size rootIndexSize;
            if (null == RootIndex)
            {
                rootIndexSize = new Size(12, FontManager.Instance.FontSize);
            }
            else
            {
                rootIndexSize = RootIndex.GetSize();
            }


            if (null == Radicand)
            {
                return FontManager.Instance.FontSize / 2 + rootIndexSize.Height / 3;
            }
            else
            {
                double verticalCenter = Radicand.GetVerticalAlignmentCenter();
                return verticalCenter + rootIndexSize.Height / 3;
            }
        }

        public void DrawBlock(Canvas canvas)
        {

            Polyline radicalPolyline = CreateRadicalPolyline();

            var rootIndexRect = GetRootIndexRect();
            var rootIndexRectangle = CreateRangeRect(rootIndexRect);

            var radicandRect = GetRadicandRect();
            var radicandRectangle = CreateRangeRect(radicandRect);

            canvas.Children.Add(rootIndexRectangle);
            canvas.Children.Add(radicandRectangle);

            canvas.Children.Add(radicalPolyline);
            if (null != RootIndex)
            {
                RootIndex.DrawBlock(canvas);
            }

            if (null != Radicand)
            {
                Radicand.DrawBlock(canvas);
            }
        }

        private Rectangle CreateRangeRect(Rect rect)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.StrokeDashArray = new DoubleCollection() { 2, 1 };
            rectangle.Stroke = Brushes.Black;
            rectangle.StrokeThickness = 1;
            rectangle.Width = rect.Width;
            rectangle.Height = rect.Height;
            Canvas.SetLeft(rectangle, rect.Left);
            Canvas.SetTop(rectangle, rect.Top);
            return rectangle;
        }

        private Polyline CreateRadicalPolyline()
        {
            var radicalSize = GetSize();


            var rootIndexSize = GetRootIndexSize();
            Point point4 = new Point(this.Location.X + rootIndexSize.Width+10, this.Location.Y + rootIndexSize.Height / 3);
            Point point5 = new Point(this.Location.X + radicalSize.Width, point4.Y);

            var radicandSize = GetRadicandSize();

            Point point3 = new Point(point4.X - 10, point4.Y + radicandSize.Height);
            Point point2 = new Point(point3.X - 2, point3.Y - 6);
            Point point1 = new Point(point2.X - 4, point2.Y + 4);

            Polyline polyline = new Polyline();
            polyline.Stroke = Brushes.Black;
            polyline.StrokeThickness = 1;
            polyline.Points = new PointCollection() { point1, point2, point3, point4, point5 };

            return polyline;
        }

        private bool IsRootIndexContains(Point caretPoint)
        {
            Rect rootIndexRect = GetRootIndexRect();
            Rect rootIndexExpandRect = new Rect(rootIndexRect.X - 2, rootIndexRect.Y - 2, rootIndexRect.Width + 4, rootIndexRect.Height);
            if (rootIndexExpandRect.Contains(caretPoint))
            {
                return true;
            }
            return false;
        }

        private bool IsRadicandContains(Point caretPoint)
        {
            Rect radicandRect = GetRadicandRect();
            Rect radicandExpandRect = new Rect(radicandRect.X - 2, radicandRect.Y - 2, radicandRect.Width + 4, radicandRect.Height);
            if (radicandExpandRect.Contains(caretPoint))
            {
                return true;
            }
            return false;
        }


        public void AddChildren(IEnumerable<IBlock> inputCharactors, Point caretPoint, string parentId)
        {
            if (IsRootIndexContains(caretPoint))
            {
                if (null == RootIndex)
                {
                    RootIndex = new BlockNode();
                }
                RootIndex.AddChildren(inputCharactors, caretPoint, parentId);
            }

            if (IsRadicandContains(caretPoint))
            {
                if (null == Radicand)
                {
                    Radicand = new BlockNode();
                }
                Radicand.AddChildren(inputCharactors, caretPoint, parentId);
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
                if (null != RootIndex)
                {
                    var node = RootIndex.FindNodeById(id);
                    if (null != node)
                    {
                        return node;
                    }
                }

                if (null != Radicand)
                {
                    var deNode = Radicand.FindNodeById(id);
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
            var rect = GetRadicandRect();
            if (IsRootIndexContains(caretLocation))
            {

                return rect.Location;
            }
            else
            {
                var size = GetSize();
                double alignmentCenter = GetVerticalAlignmentCenter();
                MessageManager.Instance.OnInputParentChanged(ParentId);
                return new Point(Location.X + size.Width, Location.Y + alignmentCenter-FontManager.Instance.FontSize/2);
            }

        }

        public Point GotoPreviousPart(Point caretLocation)
        {
            throw new NotImplementedException();
        }

        public XElement Serialize()
        {
            var typeFraction = (new Radical()).GetType();
            XElement fraction = new XElement("Radical");
            XElement molecule = new XElement("RootIndex");
            XElement denominator = new XElement("Radicand");

            XElement rootIndexBlockNode = RootIndex.Serialize();
            molecule.Add(rootIndexBlockNode);

            XElement radicandBlockNode = Radicand.Serialize();
            denominator.Add(radicandBlockNode);

            fraction.Add(molecule);
            fraction.Add(denominator);
            return fraction;
        }
    }
}
