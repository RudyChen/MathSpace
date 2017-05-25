using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Model
{
   public interface IBlock
    {
        double GetHeight();

        double GetVerticalAlignmentCenter();

        /// <summary>
        /// 给每个元素计算一个位置
        /// </summary>
        void AlignElementsToVerticalCenter();
    }
}
