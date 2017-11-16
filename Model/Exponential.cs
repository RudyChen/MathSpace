using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MathSpace.Model
{
    public class Exponential : IBlock
    {
        #region Properties
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
        #endregion

        public Exponential()
        {
            ID = Guid.NewGuid().ToString();
        }


        public void SetBlockLocation(double locationX,double alignmentCenterY, double rowY)
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

        public Point GotoNextPart(Point caretLocation)
        {
            throw new NotImplementedException();
        }

        public Point GotoPreviousPart(Point caretLocation)
        {
            throw new NotImplementedException();
        }
    }
}
