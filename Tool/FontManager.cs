using System;
using System.Collections.Generic;
using System.Configuration;
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
            var fontsizeStr=ConfigurationManager.AppSettings["FontSize"].ToString();
            double fontSize = 0;
            double.TryParse(fontsizeStr,out fontSize);
            FontSize = fontSize;
        }

        private static FontManager _instance;

        public static FontManager Instance
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
