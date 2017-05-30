using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathSpace
{
    /// <summary>
    /// MathEidtor.xaml 的交互逻辑
    /// </summary>
    public partial class MathEidtor : UserControl
    {
        public MathEidtor()
        {
            InitializeComponent();
        }

        private void editorCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            caretTextBox.Focus();

            var absolutePoint = e.GetPosition(editorCanvas);
            //todo: find the row who contains this absolutePoint as currentInputBlockRow
           // var rowPoint = new Point(absolutePoint.X, absolutePoint.Y - currentInputBlockRow.RowRect.Top);
        }

        private void editorCanvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void caretTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsChinese(e.Text))
            {
                if (!string.IsNullOrEmpty(e.Text))
                {
                    AcceptChineseInputText(0, 0, e.Text);
                }
            }
            else
            {
               
                 AcceptEnglishInputText(0, 0, e.Text);
            }
            e.Handled = true;
        }

        private void AcceptEnglishInputText(double lineOffsetX, double lineOffsetY, string text)
        {

        }

        private void AcceptChineseInputText(double lineOffsetX, double lineOffsetY, string text)
        {

        }

        private bool IsChinese(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            text = text.Trim();

            foreach (char c in text)
            {
                if (c < 0x301E) return false;
            }

            return true;
        }

        private bool IsLowerAlphabet(char charItem)
        {
            if (charItem>0x0061&&charItem<0x007B)
            {
                return true;
            }   
            return false;
        }
    }
}
