﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Model
{
    public enum InputCommands
    {
        /// <summary>
        /// 分数
        /// </summary>
        FractionCommand,

        /// <summary>
        /// 输入下一个部分
        /// </summary>
        NextCommand,

        /// <summary>
        /// 指数
        /// </summary>
        Exponential,

        /// <summary>
        /// 根式
        /// </summary>
        Radical,

        /// <summary>
        /// 下标
        /// </summary>
        SubscriptCommand,

        /// <summary>
        /// 删除命令
        /// </summary>
        DeleteCommand,

    }
}
