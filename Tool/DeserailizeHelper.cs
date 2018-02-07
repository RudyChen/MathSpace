using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Tool
{
   public class DeserailizeHelper
    {
        public static Type GetXmlNodeType(string nodeName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var allTypes = currentAssembly.GetTypes();
            foreach (var typeItem in allTypes)
            {
                if (typeItem.Name == nodeName)
                {
                    return typeItem;
                }
            }

            return null;
        }
    }
}
