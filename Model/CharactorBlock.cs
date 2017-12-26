using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MathSpace.Model
{
    [DataContract]
    public class CharactorBlock : IBlock
    {
        #region Properties
        public string FontFamily { get; set; }

        public string FontWeight { get; set; }

        public double FontSize { get; set; }

        public string ForgroundColor { get; set; }

        public string FontStyle { get; set; }

        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// 块ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 父容器ID
        /// </summary>
        public string ParentId { get; set; }

        public CharactorBlock()
        {
            ID = Guid.NewGuid().ToString();
        }


        public Point Location { get; set; }

        public Size BlockSize { get; set; }

        public FormattedText FormatedCharactor { get; set; }

        public double VerticalCenter { get; set; }
        #endregion


        public Size GetSize()
        {
            if (null == FormatedCharactor)
            {
                FontStyle fontStyle = GetFontStyle();
                FontWeight fontWeight = GetFontWeight();

                SolidColorBrush foreGroundBrush = GetBlockForeground();
                FontFamily fontFamily = GetFontFamily();

                Typeface typeFact = new Typeface(fontFamily, fontStyle, fontWeight, FontStretches.Normal);
                FormatedCharactor = new FormattedText(Text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeFact, FontSize, foreGroundBrush);
            }

            if (FontStyles.Italic==GetFontStyle())
            {
                BlockSize = new Size(FormatedCharactor.WidthIncludingTrailingWhitespace+1, FormatedCharactor.Height);
            }
            else
            {
                BlockSize = new Size(FormatedCharactor.WidthIncludingTrailingWhitespace, FormatedCharactor.Height);
            }
            


            return BlockSize;
        }

        private FontStyle GetFontStyle()
        {
            var fontStyleConverter = new FontStyleConverter();
            var fontStyle = (FontStyle)fontStyleConverter.ConvertFromString(FontStyle);
            return fontStyle;
        }

        private FontWeight GetFontWeight()
        {
            var fontWeightConverter = new FontWeightConverter();
            var fontWeight = (FontWeight)fontWeightConverter.ConvertFromString(FontWeight);
            return fontWeight;
        }

        private FontFamily GetFontFamily()
        {
            var fontFamilyConverter = new FontFamilyConverter();
            var fontFamily = (FontFamily)fontFamilyConverter.ConvertFromString(FontFamily);

            return fontFamily;
        }

        private SolidColorBrush GetBlockForeground()
        {
            var foreGround = (Color)ColorConverter.ConvertFromString(ForgroundColor);
            var foreGroundBrush = new SolidColorBrush(foreGround);
            return foreGroundBrush;
        }

        public double GetVerticalAlignmentCenter()
        {            
            VerticalCenter = BlockSize.Height / 2;            
            return VerticalCenter;
        }

        public void SetBlockLocation(double locationX, double alignmentCenterY, double rowY)
        {
            var y = alignmentCenterY - BlockSize.Height / 2;
            Location = new Point(locationX, y);
        }

        public void DrawBlock(Canvas canvas)
        {
            TextBlock tb = CreateTextBlock();
            canvas.Children.Add(tb);
            Canvas.SetLeft(tb, Location.X);
            Canvas.SetTop(tb, Location.Y);
        }

        private TextBlock CreateTextBlock()
        {
            TextBlock tb = new TextBlock();
            tb.Text = Text;
            tb.FontFamily = GetFontFamily();
            tb.Foreground = GetBlockForeground();
            tb.FontWeight = GetFontWeight();
            tb.FontStyle = GetFontStyle();
            tb.FontSize = FontSize;
            tb.Uid = this.ID;

            return tb;
        }

        public void AddChildren(IEnumerable<IBlock> inputCharactors,Point caretPoint, string parentId)
        {
            throw new NotImplementedException();
        }

        public IBlock FindNodeById(string id)
        {
            if (ID==id)
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public Point GotoNextPart(Point caretLocation)
        {
            throw new NotImplementedException();
        }

        public Point GotoPreviousPart(Point caretLocation)
        {
            throw new NotImplementedException();
        }
    }
}
