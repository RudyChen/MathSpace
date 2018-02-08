using MathSpace.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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

        /// <summary>
        /// 文字块数学中小写字母必须用特定字体
        /// 特殊符号必须用“微软雅黑”
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static CharactorBlock CreateNewCharactorBlock(char c)
        {
            var lowerAlphabet = new CharactorBlock();
            lowerAlphabet.FontSize = FontManager.Instance.FontSize;
            lowerAlphabet.Text = c.ToString();
            lowerAlphabet.FontWeight = FontWeights.Normal.ToString();
            lowerAlphabet.ForgroundColor = Brushes.Black.ToString();
            lowerAlphabet.ID = new Guid().ToString();
            if (IsLowerAlphabet(c) || IsSymbolOrNumber(c))
            {

                lowerAlphabet.FontStyle = IsSymbolOrNumber(c) ? "Normal" : "Italic";
                lowerAlphabet.FontFamily = "Times New Roman";
            }
            else
            {
                lowerAlphabet.FontStyle = "Normal";
                lowerAlphabet.FontFamily = "微软雅黑";
            }

            return lowerAlphabet;
        }


        private static bool IsChinese(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            text = text.Trim();

            foreach (char c in text)
            {
                if (c < 0x301E) return false;
            }

            return true;
        }

        private static bool IsLowerAlphabet(char charItem)
        {
            if (charItem >= 'a' && charItem <= 'z')
            {
                return true;
            }
            return false;
        }

        private static bool IsSymbolOrNumber(char charItem)
        {
            if (charItem > '!' && charItem <= '?')
            {
                return true;
            }

            return false;
        }
    }
}
