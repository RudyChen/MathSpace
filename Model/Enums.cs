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

    }
}
