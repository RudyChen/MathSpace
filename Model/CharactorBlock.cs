using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MathSpace.Model
{
    public class CharactorBlock : IBlock
    {
        #region Properties
        public string FontFamily { get; set; }

        public string FontWeight { get; set; }

        public double FontSize { get; set; }

        public string Text { get; set; }

        public string FontStyle { get; set; }

        public string ID { get; set; }

        public string ParentId { get; set; }

        public string ForgroundColor { get; set; }

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
                BlockSize = new Size(FormatedCharactor.WidthIncludingTrailingWhitespace+1, FormatedCharactor.MaxTextHeight);
            }
            else
            {
                BlockSize = new Size(FormatedCharactor.WidthIncludingTrailingWhitespace, FormatedCharactor.MaxTextHeight);
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
            double verticalCenter = 0;
            VerticalCenter = BlockSize.Height / 2;
            verticalCenter= BlockSize.Height / 2;
            return verticalCenter;
        }

        public void SetBlockLocation(double locationX, double alignmentCenterY)
        {
            Location = new Point(locationX, alignmentCenterY - BlockSize.Height / 2);
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

        public void AddChildren(List<IBlock> inputCharactors)
        {
            throw new NotImplementedException();
        }
    }
}
