using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace MathSpace.Model
{
    public class ShapeBlock : IBlock
    {
        public void AddChildren(IEnumerable<IBlock> inputCharactors,Point caretPoint, string parentId)
        {
            throw new NotImplementedException();
        }

        public void SetBlockLocation(double locationX,double alignmentCenterY, double rowY)
        {
            throw new NotImplementedException();
        }

        public void DrawBlock(Canvas canvas)
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

        public IBlock FindNodeById(string id)
        {
            throw new NotImplementedException();
        }

        public Point GotoNextPart(Point caretLocation)
        {
            throw new NotImplementedException();
        }

        public Point GotoPreviousPart(Point caretLocation)
        {
            throw new NotImplementedException();
        }

        public XElement Serialize()
        {
            throw new NotImplementedException();
        }

        public void GetElementBeforeCaret(Point caretLocation)
        {
            throw new NotImplementedException();
        }

        public string GetParentId()
        {
            return string.Empty;
        }

        public void RemoveChild(IBlock block)
        {
            throw new NotImplementedException();
        }

        public Point GetBlockLocation()
        {
            return new Point(0,0);
        }

        public IBlock GetCaretBrotherElement(bool before, Point caretPoint)
        {
            throw new NotImplementedException();
        }
    }
}
