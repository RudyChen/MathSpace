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
   public interface IBlock
    {
        Size GetSize();

        double GetVerticalAlignmentCenter();

        /// <summary>
        /// 给每个元素计算一个位置
        /// </summary>
        void SetBlockLocation(double locationX,double alignmentCenterY,double rowY);


        void DrawBlock(Canvas canvas);

        /// <summary>
        /// 添加节点元素
        /// </summary>
        /// <param name="inputCharactors"></param>
        void AddChildren(IEnumerable<IBlock> inputCharactors,Point caretPoint, string parentId);

        IBlock FindNodeById(string id);

        /// <summary>
        /// 在组合模式对象中跳转输入部分
        /// </summary>
        Point GotoNextPart(Point location);

        /// <summary>
        /// 在组合模式对象中跳转到前一个部分
        /// </summary>
        /// <returns></returns>
        Point GotoPreviousPart(Point location);

        /// <summary>
        /// 元素序列化
        /// </summary>
        /// <returns></returns>
        XElement Serialize();
    }
}
