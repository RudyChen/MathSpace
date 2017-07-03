using MathSpace.Model;
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

        private Row _currentRow=new Row();

        public Row CurrentRow
        {
            get { return _currentRow; }
            set { _currentRow = value; }
        }

        private Stack<InputCommands> _inputCommandStack;

        public Stack<InputCommands> InputCommandStack
        {
            get { return _inputCommandStack; }
            set { _inputCommandStack = value; }
        }

        public int FontSize { get; set; }


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
            List<CharactorBlock> inputCharactors = new List<CharactorBlock>();
            inputCharactors = CreateInputCharactorBlocks(text);



        }

        /// <summary>
        /// 创建输入文字块集合
        /// 区分小写字符，小写字符必须用特定字体与样式
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private List<CharactorBlock> CreateInputCharactorBlocks(string text)
        {
            List<CharactorBlock> inputCharactors = new List<CharactorBlock>();
            for (int i = 0; i < text.Length; i++)
            {
                CharactorBlock lowerAlphabet;
                lowerAlphabet = inputCharactors.Last();
                bool isLowerCharactor = IsLowerAlphabet(text[i]);

                if (null == lowerAlphabet)
                {
                    lowerAlphabet = CreateNewCharactorBlock(text[i]);

                    inputCharactors.Add(lowerAlphabet);
                }
                else
                {
                    bool isLastCharactorLowerAlphabet = IsLowerAlphabet(lowerAlphabet.Text[lowerAlphabet.Text.Length - 1]);
                    if (isLastCharactorLowerAlphabet)
                    {
                        if (isLowerCharactor)
                        {
                            lowerAlphabet.Text += text[i].ToString();
                        }
                        else
                        {
                            var newCharactor = CreateNewCharactorBlock(text[i]);
                            inputCharactors.Add(newCharactor);
                        }
                    }
                    else
                    {
                        if (isLowerCharactor)
                        {
                            var newCharactor = CreateNewCharactorBlock(text[i]);
                            inputCharactors.Add(newCharactor);
                        }
                        else
                        {
                            lowerAlphabet.Text += text[i].ToString();
                        }
                    }
                }
            }

            return inputCharactors;
        }

        /// <summary>
        /// 文字块数学中小写字母必须用特定字体
        /// 特殊符号必须用“微软雅黑”
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private CharactorBlock CreateNewCharactorBlock(char c)
        {
            var lowerAlphabet = new CharactorBlock();           
            lowerAlphabet.FontSize = FontSize;
            lowerAlphabet.Text = c.ToString();
            if (IsLowerAlphabet(c))
            {
                lowerAlphabet.FontStyle = "Italic";
                lowerAlphabet.FontFamily = "Times New Roman";
            }
            else
            {
                lowerAlphabet.FontStyle = "Normal";
                lowerAlphabet.FontFamily = "微软雅黑";
            }

            return lowerAlphabet;
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
