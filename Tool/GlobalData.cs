using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Tool
{
   public class GlobalData
    {
        private GlobalData()
        {

        }

        private static GlobalData _instance;
        private static object _lock = new object();

        public static GlobalData Instance
        {
            get
            {
                if (null==_instance)
                {
                    lock (_lock)
                    {
                        if (null==_instance)
                        {
                            _instance = new GlobalData();
                        }                        
                    }
                   
                }
                return _instance;
            }
            set { _instance = value; }
        }

        /// <summary>
        /// 页面块包含查找使用堆栈
        /// </summary>
        public Stack<Model.IBlock> ContainStack { get; set; }

    }
}
