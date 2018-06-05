using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MathGraphics
{
    public class MyFontFamily : ViewModelBase
    {
        private string fontName;

        private FontFamily fontFamily;

        public FontFamily FontFamilyEntity
        {
            get { return fontFamily; }
            set { Set(ref fontFamily, value); }
        }

        public string FontName
        {
            get { return fontName; }
            set { Set(ref fontName, value); }
        }
    }
}
