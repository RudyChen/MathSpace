using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Tool
{
   public class FontManager
    {
        #region Properties
        public string FontFamily { get; set; }

        public string FontWeight { get; set; }

        public double FontSize { get; set; }

        public string ForgroundColor { get; set; }

        public string FontStyle { get; set; }
        #endregion


        private  FontManager()
        {
           
        }

        private FontManager _instance;

        public FontManager Instance
        {
            get
            {
                if (null==_instance)
                {
                    _instance = new FontManager();
                }
                return _instance;
            }
            set { _instance = value; }
        }


    }
}
