﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MathSpace.Model
{
    public class ShapeBlock : IBlock
    {
        public void AddChildren(List<IBlock> inputCharactors)
        {
            throw new NotImplementedException();
        }

        public void SetBlockLocation(double locationX,double alignmentCenterY)
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
    }
}
