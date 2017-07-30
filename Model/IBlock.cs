using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MathSpace.Model
{
   public interface IBlock
    {
        Size GetSize();

        double GetVerticalAlignmentCenter();

        /// <summary>
        /// 给每个元素计算一个位置
        /// </summary>
        void SetBlockLocation(double locationX,double alignmentCenterY);


        void DrawBlock(Canvas canvas);

        /// <summary>
        /// 添加节点元素
        /// </summary>
        /// <param name="inputCharactors"></param>
        void AddChildren(IEnumerable<IBlock> inputCharactors,Point caretPoint, string parentId);

        IBlock FindNodeById(string id);
    }
}
