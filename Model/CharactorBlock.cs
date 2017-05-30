using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Model
{
    public class CharactorBlock : IBlock
    {
        public string FontFamily { get; set; }

        public string FontWeight { get; set; }

        public int FontSize { get; set; }

        public string Text { get; set; }

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
    }
}
