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
            FontSize = 14;
        }


        /// <summary>
        /// 输入父节点ID
        /// 通过父节点ID寻找输入对象，不用再用栈存储
        /// </summary>
        public string InputParentId { get; set; }

        private Row _currentRow = new Row();
        /// <summary>
        /// 当前输入的行
        /// </summary>
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

        public static int FontSize { get; set; }


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
            AcceptInputText(e.Text);
          
            e.Handled = true;
        }

        private List<Row> _rows = new List<Row>();
        /// <summary>
        /// 编辑器的多行公式集合
        /// </summary>
        public List<Row> Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }


        private void AcceptInputText(string text)
        {
            //todo:空格输入没有响应

            List<CharactorBlock> inputCharactors = new List<CharactorBlock>();
            foreach (var item in text)
            {
                var charactorBlock = CreateNewCharactorBlock(item);
                if (!string.IsNullOrEmpty(InputParentId))
                {
                    charactorBlock.ParentId = InputParentId;
                }
                inputCharactors.Add(charactorBlock);
            }


            if (!string.IsNullOrEmpty(InputParentId))
            {
                var parentNode = CurrentRow.FindParentNode(InputParentId);

                parentNode.AddChildren(inputCharactors, GetCaretLocation(), InputParentId);              
            }
            else
            {
                CurrentRow.Members.AddRange(inputCharactors);
            }

            RefreshRow();
            /*输入完毕之后得
              1.绘制整个currentRow
              2.管理输入文本框的位置
            */
            double offsetx = 0;
            foreach (var item in inputCharactors)
            {
                offsetx += item.GetSize().Width;
            }
            var charactorSize = inputCharactors.Last().GetSize();
            ResetCaretLocation(offsetx);
        }

        /// <summary>
        /// 刷新行显示内容
        /// </summary>
        private void RefreshRow()
        {
            ClearInputElements();

            double maxVerticalAlignment = 0.0;
            maxVerticalAlignment= (double)GetMaxVerticalAlignment();

            double locationX = 0;
            double alignmentCenterY = 0;
            foreach (var item in CurrentRow.Members)
            {
                var itemSize = item.GetSize();

                alignmentCenterY = (maxVerticalAlignment + CurrentRow.Location);
                item.SetBlockLocation(locationX, alignmentCenterY,CurrentRow.Location);
                locationX += itemSize.Width;
                item.DrawBlock(editorCanvas);
            }
        }

        private void AddComponentType(IBlock block)
        {
            if (!string.IsNullOrEmpty(InputParentId))
            {
                var parentNode = CurrentRow.FindParentNode(InputParentId);
                List<IBlock> blocks = new List<IBlock>();
                blocks.Add(block);
                parentNode.AddChildren(blocks, GetCaretLocation(),InputParentId);
            }
            else
            {
                CurrentRow.Members.Add(block);
            }

            RefreshRow();                       
        }

        private void ClearInputElements()
        {
           var caretLeft= Canvas.GetLeft(caretTextBox);
            var caretTop= Canvas.GetTop(caretTextBox);
            var tempCaretTextBox = caretTextBox;

            editorCanvas.Children.Clear();
            editorCanvas.Children.Add(caretTextBox);
            Canvas.SetLeft(caretTextBox,caretLeft);
            Canvas.SetTop(caretTextBox, caretTop);           
        }

        private void ResetCaretLocation(double offsetX)
        {
            caretTextBox.Text = string.Empty;
            var oldLeft = Canvas.GetLeft(caretTextBox);
            Canvas.SetLeft(caretTextBox, oldLeft+ offsetX);
        }

        private double GetMaxVerticalAlignment()
        {
            double maxValue = 0;
            List<double> verticalAlignmentList = new List<double>();
            foreach (var item in CurrentRow.Members)
            {
                var itemSize = item.GetSize();
                var temMax = item.GetVerticalAlignmentCenter();
                if (temMax> maxValue)
                {
                    maxValue = temMax;
                }
             
            }
            return maxValue;
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
            lowerAlphabet.FontWeight = FontWeights.Normal.ToString();
            lowerAlphabet.ForgroundColor = Brushes.Black.ToString();
            lowerAlphabet.ID = new Guid().ToString();
            if (IsLowerAlphabet(c)||IsSymbolOrNumber(c))
            {
               
                lowerAlphabet.FontStyle = IsSymbolOrNumber(c)?"Normal":"Italic";
                lowerAlphabet.FontFamily = "Times New Roman";
            }
            else
            {
                lowerAlphabet.FontStyle = "Normal";
                lowerAlphabet.FontFamily = "微软雅黑";
            }

            return lowerAlphabet;
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
            if (charItem >= 'a' && charItem <= 'z')
            {
                return true;
            }
            return false;
        }

        private bool IsSymbolOrNumber(char charItem)
        {
            if (charItem>'!'&&charItem<='?')
            {
                return true;
            }

            return false;
        }

        private void InputType_Changed(InputTypes inputType)
        {
            switch (inputType)
            {
                case InputTypes.Fraction:
                    AddDefaultFraction();
                    break;
                case InputTypes.Radical:
                    break;
                case InputTypes.Exponential:
                    break;
                default:
                    break;
            }
        }

        private void AddDefaultFraction()
        {
            Fraction fraction = new Fraction();           
            AddComponentType(fraction);
            InputParentId = fraction.ID;

        }

        private Point GetCaretLocation()
        {
           var left= Canvas.GetLeft(caretTextBox);
            var top = Canvas.GetTop(caretTextBox);

            return new Point(left,top);
        }
    }
}
