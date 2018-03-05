using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Model
{
    public enum InputCommands
    {
        /// <summary>
        /// 输入下一个部分
        /// </summary>
        NextCommand,

        /// <summary>
        /// 输入位置切换到上一部分
        /// </summary>
        PreviousCommand,
       
        /// <summary>
        /// 删除命令
        /// </summary>
        DeleteCommand,

        /// <summary>
        /// 序列化
        /// </summary>
        SerializeCommand,

        /// <summary>
        /// 反序列化
        /// </summary>
        DeserializeCommand,

        /// <summary>
        /// 回退命令
        /// </summary>
        Backspace,
    }

    public enum ContentPages
    {
        /// <summary>
        /// 成就页面
        /// </summary>
        Achievement,
        /// <summary>
        /// 章节页面
        /// </summary>
        Chapters,
        /// <summary>
        /// 题目列表页面
        /// </summary>
        Questions,
        /// <summary>
        /// 答题页面
        /// </summary>
        AnswerQuestion,
    }
}
