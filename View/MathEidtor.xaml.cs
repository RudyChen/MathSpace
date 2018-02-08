﻿using MathSpace.Model;
using MathSpace.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Xml;
using System.Xml.Linq;

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
            MessageManager.Instance.InputParentChangedEvent += Instance_InputParentChangedEvent;
        }

        /// <summary>
        /// 输入父元素改变事件
        /// </summary>
        /// <param name="parentId"></param>
        private void Instance_InputParentChangedEvent(string parentId)
        {
            this.InputParentId = parentId;
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
                var charactorBlock =FontManager.CreateNewCharactorBlock(item);
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
                CurrentRow.Blocks.Children.AddRange(inputCharactors);
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
            maxVerticalAlignment = (double)GetMaxVerticalAlignment();

            double locationX = 0;
            double alignmentCenterY = 0;
            foreach (var item in CurrentRow.Blocks.Children)
            {
                var itemSize = item.GetSize();

                alignmentCenterY = (maxVerticalAlignment + CurrentRow.Location);
                item.SetBlockLocation(locationX, alignmentCenterY, CurrentRow.Location);
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
                parentNode.AddChildren(blocks, GetCaretLocation(), InputParentId);
            }
            else
            {
                CurrentRow.Blocks.Children.Add(block);
            }

            RefreshRow();
        }

        private void ClearInputElements()
        {
            var caretLeft = Canvas.GetLeft(caretTextBox);
            var caretTop = Canvas.GetTop(caretTextBox);
            var tempCaretTextBox = caretTextBox;

            editorCanvas.Children.Clear();
            editorCanvas.Children.Add(caretTextBox);
            Canvas.SetLeft(caretTextBox, caretLeft);
            Canvas.SetTop(caretTextBox, caretTop);
        }

        private void ResetCaretLocation(double offsetX)
        {
            caretTextBox.Text = string.Empty;
            var oldLeft = Canvas.GetLeft(caretTextBox);
            Canvas.SetLeft(caretTextBox, oldLeft + offsetX);
        }

        private void SetCaretLocation(Point location)
        {
            Canvas.SetLeft(caretTextBox, location.X);
            Canvas.SetTop(caretTextBox, location.Y);
        }

        private double GetMaxVerticalAlignment()
        {
            double maxValue = 0;
            List<double> verticalAlignmentList = new List<double>();
            foreach (var item in CurrentRow.Blocks.Children)
            {
                var itemSize = item.GetSize();
                var temMax = item.GetVerticalAlignmentCenter();
                if (temMax > maxValue)
                {
                    maxValue = temMax;
                }

            }
            return maxValue;
        }
        

        private Point GetCaretLocation()
        {
            var left = Canvas.GetLeft(caretTextBox);
            var top = Canvas.GetTop(caretTextBox);

            return new Point(left, top);
        }

        private void InputType_Changed(InputTypes inputType)
        {
            switch (inputType)
            {
                case InputTypes.Fraction:
                    AddDefaultFraction();
                    break;
                case InputTypes.Radical:
                    AddDefaultRadical();
                    break;
                case InputTypes.Exponential:
                    AddDefaultExponential();
                    break;
                default:
                    break;
            }
        }

        private void AddDefaultRadical()
        {
            Radical radical = new Radical();
            AddComponentType(radical);
            if (!string.IsNullOrEmpty(InputParentId))
            {
                radical.ParentId = InputParentId;
            }
            InputParentId = radical.ID;


            SetCaretLocation(radical.Location);
        }

        private void AddDefaultFraction()
        {
            Fraction fraction = new Fraction();
            AddComponentType(fraction);
            if (!string.IsNullOrEmpty(InputParentId))
            {
                fraction.ParentId = InputParentId;
            }
            InputParentId = fraction.ID;


            SetCaretLocation(fraction.Location);
        }


        private void AddDefaultExponential()
        {
            Exponential exponential = new Exponential();
            AddComponentType(exponential);
            if (!string.IsNullOrEmpty(InputParentId))
            {
                exponential.ParentId = InputParentId;
            }

            InputParentId = exponential.ID;

            SetCaretLocation(new Point(exponential.Location.X, exponential.Location.Y + FontManager.Instance.FontSize * exponential.Proportion));
        }

        private void InputCommand_Changed(InputCommands command)
        {
            switch (command)
            {
                case InputCommands.NextCommand:
                    GotoNextPart();
                    break;
                case InputCommands.PreviousCommand:
                    break;
                case InputCommands.DeleteCommand:
                    break;
                case InputCommands.SerializeCommand:
                    SerializeEquations();
                    break;
                case InputCommands.DeserializeCommand:
                    DeserializeEquation();
                    break;
                default:
                    break;
            }
        }

        private void DeserializeEquation()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("d:\\rowDocument.xml");
            var rootElement = doc.DocumentElement;
            var type = DeserailizeHelper.GetXmlNodeType(rootElement.Name);
            if (null != type)
            {
                var xmlObj = Activator.CreateInstance(type);
                if (xmlObj is Row)
                {
                    Row row = xmlObj as Row;
                    foreach (XmlNode item in rootElement.ChildNodes)
                    {
                        var nodeType = DeserailizeHelper.GetXmlNodeType(item.Name);
                        if (null != nodeType)
                        {
                            var nodeObj = Activator.CreateInstance(nodeType);
                            if (nodeObj is BlockNode)
                            {
                                row.Blocks = nodeObj as BlockNode;

                                DeserailizeHelper.DeserializeBlockNode(item, row.Blocks);
                            }
                        }
                    }
                    CurrentRow = row;
                    RefreshRow();
                }
            }
        }

        private void SerializeEquations()
        {
            //XDocument document = new XDocument("d:\\rowDocument.xml");
            var rowElement = CurrentRow.Serialize();
            rowElement.Save("d:\\rowDocument.xml");
            var rowstring = rowElement.ToString();

           
           
        }

       

        private void GotoNextPart()
        {
            if (!string.IsNullOrEmpty(InputParentId))
            {
                var parentNode = CurrentRow.FindParentNode(InputParentId);
                //非输入行元素子元素
                if (null != parentNode)
                {
                    var location = GetCaretLocation();
                    var point = parentNode.GotoNextPart(location);
                    if (null == point)
                    {
                        return;
                    }
                    SetCaretLocation(point);
                }
            }
        }

    }
}
