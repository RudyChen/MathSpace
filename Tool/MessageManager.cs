using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Tool
{
    public class MessageManager
    {
        private static volatile MessageManager instance;
        private static readonly object syncRoot = new object();
        private MessageManager() { }
        public static MessageManager Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new MessageManager();
                    }
                }
                return instance;
            }
        }

        public delegate void InputParentChangedEventHandler(string parentId);
        public event InputParentChangedEventHandler InputParentChangedEvent;
        
        public void OnInputParentChanged(string parentId)
        {
            InputParentChangedEvent(parentId);
        }
    }
}
