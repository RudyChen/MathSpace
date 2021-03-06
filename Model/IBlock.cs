﻿using System;
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

        /// <summary>
        /// 获得插字符前面的元素
        /// </summary>
        /// <returns></returns>
        void GetElementBeforeCaret(Point caretLocation);

        /// <summary>
        /// 获取组合元素的父亲ID
        /// </summary>
        /// <returns></returns>
        string GetParentId();

        /// <summary>
        /// 移除子元素
        /// </summary>
        /// <param name="block"></param>
        void RemoveChild(IBlock block);

        /// <summary>
        /// 获取块位置
        /// </summary>
        Point GetBlockLocation();
        
        /// <summary>
        /// 获取插字符兄弟节点,获取遇到了问题，不能确定插字符位于那个输入层级
        /// </summary>
        /// <param name="before">ture表示前面的兄弟，false表示后面的兄弟</param>
        /// <returns></returns>
        IBlock GetCaretBrotherElement(bool before,Point caretPoint);

        /// <summary>
        /// 在元素之后加入元素
        /// </summary>
        /// <param name="block"></param>
        /// <param name="inputCharactors"></param>
        void AddChildrenAfterBlock(IBlock block,IEnumerable<IBlock> inputCharactors);

        /// <summary>
        /// 设置父节点ID
        /// </summary>
        /// <param name="parentID"></param>
        void SetParentId(string parentID);
        

    }
}
