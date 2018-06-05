using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathGraphics
{
    /// <summary>
    /// GraphicPanelControl.xaml 的交互逻辑
    /// </summary>
    public partial class GraphicPanelControl : UserControl
    {
        #region Fields
        AdornerLayer drawCanvasAdornerLayer;
        ResizeMoveAdorner resizeMoveAdorner = null;

        Point? mouseLeftButtonDownPoint = null;

        Line tempLine = null;
        Polyline tempLLinePolyline = null;
        Label tempLabel = null;
        ToggleButton tempToggleButton = null;
        FrameworkElement selectedUIElement = null;
        TextBox nameInputTextBox = null;
        TextBox textDescriptInputTextBox = null;

        Point canvasMoveTempPoint = new Point(0, 0);

        Point canvasMoveIncrementPoint = new Point(0, 0);

        Rectangle multiSelectRect = null;
        bool isDrawMultiSelectRect = false;

        bool canMoveSelectedElements = false;
        double canvasSizeRatio = 1;

        bool canMutiSelectByControl = false;
        private List<FrameworkElement> selectedElements = new List<FrameworkElement>();

        public delegate void SwitchToSelectedToolItemEventHandler();
        public event SwitchToSelectedToolItemEventHandler SwitchToSelectedToolItemEvent;


        #endregion

        public GraphicPanelControl()
        {
            InitializeComponent();
        }

        public GraphicPanelControl(ScrollViewer scrollViewer)
        {
            InitializeComponent();
            userControlGrid.Children.Remove(userControlGrid);
            userControlGrid.Children.Clear();
            userControlGrid.Children.Add(scrollViewer);
            drawCanvas = null;
            drawCanvas = scrollViewer.Content as ZoomableCanvas;
            drawCanvas.MouseLeftButtonDown += DrawCanvas_MouseLeftButtonDown;
            drawCanvas.MouseMove += DrawCanvas_MouseMove;
            drawCanvas.MouseLeftButtonUp += DrawCanvas_MouseLeftButtonUp;
            drawCanvas.KeyDown += drawCanvas_KeyDown;
            drawCanvas.MouseMove += drawCanvas_MouseEnter;
            drawCanvas.MouseLeave += drawCanvas_MouseLeave;
            // drawCanvas.Background = (VisualBrush)TryFindResource("BackgroundVisualBrush");
        }

        private void DrawCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //添加设备名称时点击鼠标移出输入框         
            if (null != nameInputTextBox)
            {
                drawCanvas.Children.Remove(nameInputTextBox);
                nameInputTextBox = null;
            }



            mouseLeftButtonDownPoint = e.GetPosition(drawCanvas);
            Point mouseDownPoint = (Point)mouseLeftButtonDownPoint;
            canvasMoveIncrementPoint = new Point(mouseDownPoint.X, mouseDownPoint.Y);
            ToolItem toolItem = ToolManager.GetInstance().Tools.Find(item => item.IsSelected);

            if (toolItem != null && toolItem.ControlType == ToolControlType.ToggleButton)
            {
                SaveToggleButton(toolItem.ToolType.ToString() + "Style");
                this.Cursor = Cursors.Arrow;
                if (null != SwitchToSelectedToolItemEvent)
                {
                    SwitchToSelectedToolItemEvent();
                }
            }

            if (toolItem != null && toolItem.ControlType == ToolControlType.Label && toolItem.ToolType.ToString() != "TrackLine")
            {
                SaveTrackLabel(toolItem.ToolType.ToString() + "Style");
                this.Cursor = Cursors.Arrow;
                if (null != SwitchToSelectedToolItemEvent)
                {
                    SwitchToSelectedToolItemEvent();
                }
            }

            if (null != toolItem)
            {
                Point moseDownPoint = (Point)mouseLeftButtonDownPoint;
                if (toolItem.ToolType == ToolType.ItemSelect)
                {


                    if (canMoveSelectedElements)
                    {
                        if (!IsPointInSelectedElements(moseDownPoint))
                        {
                            ClearResizeMoveAdorner();
                            canMoveSelectedElements = false;
                        }
                    }
                    else
                    {
                        if (canMutiSelectByControl)
                        {
                            SelectElementByControl(moseDownPoint);
                        }
                        else
                        {
                            SelectSingleElement(moseDownPoint);
                        }

                        if (e.ClickCount == 2 && selectedElements.Count == 1)
                        {
                            if (IsClickInUIElement(moseDownPoint, selectedElements[0]))
                            {
                                AddElementName();
                            }
                        }
                    }
                }
                else if (toolItem.ToolType == ToolType.CanvasMove)
                {
                    isDrawMultiSelectRect = false;
                    ClearResizeMoveAdorner();
                }
                else if (toolItem.ToolType == ToolType.Text)
                {
                    AddDescriptionText();
                }
            }
        }

        private void AddDescriptionText()
        {
            var mainWindow = Application.Current.MainWindow;
            DrawBoardViewModel viewModel = null;
            if (null != mainWindow)
            {
                viewModel = mainWindow.DataContext as DrawBoardViewModel;
            }


            textDescriptInputTextBox = new TextBox();
            textDescriptInputTextBox.LostFocus += TextDescriptInputTextBox_LostFocus;
            textDescriptInputTextBox.KeyDown += TextDescriptInputTextBox_KeyDown;
            textDescriptInputTextBox.MinWidth = 100;
            textDescriptInputTextBox.Height = viewModel.SelectedFontSize + 10;
            textDescriptInputTextBox.FontSize = viewModel.SelectedFontSize;
            textDescriptInputTextBox.FontFamily = viewModel.SelectedFontFamily.FontFamilyEntity;
            var foregroundColor = ColorConverter.ConvertFromString(viewModel.SelectedColor);
            textDescriptInputTextBox.Foreground = new SolidColorBrush((Color)foregroundColor);
            drawCanvas.Children.Add(textDescriptInputTextBox);

            var moseDownPoint = (Point)mouseLeftButtonDownPoint;
            Canvas.SetLeft(textDescriptInputTextBox, moseDownPoint.X);
            Canvas.SetTop(textDescriptInputTextBox, moseDownPoint.Y);
        }

        private void TextDescriptInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                drawCanvas.Focus();
            }
        }

        private void TextDescriptInputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textDescriptInputTextBox.Text))
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Foreground = Brushes.LightGreen;//cffdc9
                textBlock.IsHitTestVisible = false;
                textBlock.Text = textDescriptInputTextBox.Text;
                textBlock.FontSize = textDescriptInputTextBox.FontSize;
                textBlock.FontFamily = textDescriptInputTextBox.FontFamily;
                textBlock.Foreground = textDescriptInputTextBox.Foreground;
                drawCanvas.Children.Add(textBlock);

                var left = Canvas.GetLeft(textDescriptInputTextBox);
                var top = Canvas.GetTop(textDescriptInputTextBox);

                Canvas.SetLeft(textBlock, left);
                Canvas.SetTop(textBlock, top);

                drawCanvas.Children.Remove(textDescriptInputTextBox);
                textDescriptInputTextBox = null;
            }


        }

        private void DrawCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            ToolItem toolItem = ToolManager.GetInstance().Tools.Find(item => item.IsSelected);

            double xOffset = 0;
            double yOffset = 0;
            Point nowMovingPoint = e.GetPosition(drawCanvas);

            if (e.LeftButton == MouseButtonState.Pressed && null != toolItem)
            {
                if (null == mouseLeftButtonDownPoint)
                {
                    return;
                }
                var positionPoint = ((Point)mouseLeftButtonDownPoint);
                xOffset = nowMovingPoint.X - canvasMoveIncrementPoint.X;
                yOffset = nowMovingPoint.Y - canvasMoveIncrementPoint.Y;
                if (toolItem.IsDrawShape && mouseLeftButtonDownPoint != null)
                {
                    if (toolItem.ToolType == ToolType.StraightLine)
                    {
                        DrawStraightLine(nowMovingPoint, "Shape.Line.Color", 2, false);
                    }
                    else if (toolItem.ToolType == ToolType.Bus)
                    {
                        DrawStraightLine(nowMovingPoint, "Shape.Line.Color", 6, true);
                    }
                    else if (toolItem.ToolType == ToolType.ObliqueLine)
                    {
                        DrawLine(nowMovingPoint);
                    }
                    else if (toolItem.ToolType == ToolType.LShapeLine)
                    {
                        DrawLLine(nowMovingPoint);
                    }
                    else if (toolItem.ControlType == ToolControlType.ToggleButton)
                    {
                        DrawToggleButton(nowMovingPoint, toolItem.ToolType.ToString() + "Style");
                    }
                    else if (toolItem.ControlType == ToolControlType.Label)
                    {
                        DrawLabel(nowMovingPoint, toolItem.ToolType.ToString() + "Style");
                    }
                }

                if (toolItem.ToolType == ToolType.CanvasMove)
                {
                    if (drawCanvas.Scale > 1)
                    {
                        Point offsetPoint = new Point();
                        offsetPoint.X = -(nowMovingPoint.X - ((Point)mouseLeftButtonDownPoint).X) * drawCanvas.Scale;
                        offsetPoint.Y = -(nowMovingPoint.Y - ((Point)mouseLeftButtonDownPoint).Y) * drawCanvas.Scale;
                        Point limitPoint = new Point(offsetPoint.X + canvasMoveTempPoint.X, offsetPoint.Y + canvasMoveTempPoint.Y);


                        if (limitPoint.X < 0 && limitPoint.Y < 0)
                        {
                            drawCanvas.Offset = new Point(0, 0);
                        }

                        drawCanvas.Offset = limitPoint;
                    }
                }

                ////框选，多选
                if (toolItem.ToolType == ToolType.ItemSelect && isDrawMultiSelectRect)
                {
                    DrawMultiSelectRect(nowMovingPoint);
                }

                if (toolItem.ToolType == ToolType.ItemSelect && canMoveSelectedElements)
                {
                    canvasMoveIncrementPoint = new Point(nowMovingPoint.X, nowMovingPoint.Y);
                    //移动多选元素
                    MoveSelectedElements(xOffset, yOffset);
                }
            }
            else if (e.LeftButton != MouseButtonState.Pressed && null != toolItem)
            {
                if (toolItem.ControlType == ToolControlType.ToggleButton)
                {
                    MoveDefaultToggleButton(nowMovingPoint);
                }
                else if (toolItem.ControlType == ToolControlType.Label && toolItem.ToolType.ToString() != "TrackLine")
                {

                    MoveDefaultLable(nowMovingPoint);
                }
            }
        }

        private void DrawCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var viewModel = this.DataContext as DrawBoardViewModel;
            var lineColor = (Color)ColorConverter.ConvertFromString(viewModel.SelectedLineBrush);
            SolidColorBrush solidBrush = new SolidColorBrush() { Color = lineColor };


            canvasMoveTempPoint = drawCanvas.Offset;

            Point mouseUpPoint = e.GetPosition(drawCanvas);

            ToolItem toolItem = ToolManager.GetInstance().Tools.Find(item => item.IsSelected);
            if (toolItem != null && toolItem.IsDrawShape)
            {

                string styleName = GetToolStyleName(toolItem.ToolType, mouseUpPoint);
                if (toolItem.ControlType == ToolControlType.ToggleButton && tempToggleButton != null)
                {
                    SaveToggleButton(styleName);
                }
                else if (toolItem.ControlType == ToolControlType.Label && tempLabel != null)
                {
                    SaveTrackLabel(styleName);
                }
                else if (toolItem.ControlType == ToolControlType.Line && tempLine != null)
                {
                    int strokeThickness = toolItem.ToolType == ToolType.Bus ? 6 : 2;
                    bool isBus = toolItem.ToolType == ToolType.Bus ? true : false;
                    SaveLine(strokeThickness, isBus);
                }
                else if (toolItem.ControlType == ToolControlType.Polyline && tempLLinePolyline != null)
                {

                    SaveLShapeLine(viewModel.SelectedLineBrush);
                }
            }

            if (null != toolItem && toolItem.ToolType == ToolType.ItemSelect)
            {
                if (!canMoveSelectedElements)
                {
                    if (isDrawMultiSelectRect && null != multiSelectRect)
                    {
                        var children = LogicalTreeHelper.GetChildren(drawCanvas);
                        foreach (FrameworkElement item in children)
                        {
                            if (item != multiSelectRect)
                            {
                                if (IsElementInSelectedRect(multiSelectRect, item))
                                {
                                    canMoveSelectedElements = true;
                                    selectedElements.Add(item);
                                }
                            }
                        }
                    }

                    foreach (var item in selectedElements)
                    {
                        var tempResizeMoveAdorner = new ResizeMoveAdorner(item);

                        drawCanvasAdornerLayer.Add(tempResizeMoveAdorner);
                    }
                    if (selectedElements.Count > 1)
                    {
                        canMoveSelectedElements = true;
                    }

                    if (multiSelectRect != null)
                    {

                        drawCanvas.Children.Remove(multiSelectRect);
                        multiSelectRect = null;
                        isDrawMultiSelectRect = false;
                    }
                }

            }

            mouseLeftButtonDownPoint = null;
        }

        private void DrawMultiSelectRect(Point nowMovingPoint)
        {
            if (multiSelectRect != null && drawCanvas.Children.Contains(multiSelectRect))
            {
                drawCanvas.Children.Remove(multiSelectRect);
                multiSelectRect = null;
            }

            var positionPoint = (Point)mouseLeftButtonDownPoint;

            var xOffset = nowMovingPoint.X - positionPoint.X;
            var yOffset = nowMovingPoint.Y - positionPoint.Y;
            multiSelectRect = new Rectangle();
            multiSelectRect.Width = Math.Abs(xOffset);
            multiSelectRect.Height = Math.Abs(yOffset);
            multiSelectRect.Stroke = new SolidColorBrush(Colors.White);
            multiSelectRect.StrokeDashArray = new DoubleCollection() { 5, 3 };
            multiSelectRect.IsHitTestVisible = false;
            var topLeftPoint = GetElementTopLeftPointInCanvas(positionPoint, nowMovingPoint);
            drawCanvas.Children.Add(multiSelectRect);
            Canvas.SetLeft(multiSelectRect, topLeftPoint.X);
            Canvas.SetTop(multiSelectRect, topLeftPoint.Y);
        }

        private void MoveSelectedElements(double xOffset, double yOffset)
        {
            if (selectedElements.Count > 0)
            {
                foreach (var item in selectedElements)
                {
                    double xLocation = Canvas.GetLeft(item);
                    double yLocation = Canvas.GetTop(item);
                    double newXLocation = xLocation + xOffset;
                    double newYLocation = yLocation + yOffset;
                    Canvas.SetLeft(item, newXLocation);
                    Canvas.SetTop(item, newYLocation);
                }
            }
        }

        private void DrawLLine(Point nowMovingPoint)
        {
            var viewModel = this.DataContext as DrawBoardViewModel;
            if (null != tempLLinePolyline)
            {
                drawCanvas.Children.Remove(tempLLinePolyline);
            }

            Point positionPoint = (Point)mouseLeftButtonDownPoint;

            double xOffset = nowMovingPoint.X - positionPoint.X;
            double yOffset = nowMovingPoint.Y - positionPoint.Y;

            Point topLeftPoint = GetElementTopLeftPointInCanvas(positionPoint, nowMovingPoint);
            Point relativeStart = new Point(Math.Abs(positionPoint.X - topLeftPoint.X), Math.Abs(positionPoint.Y - topLeftPoint.Y));
            Point relativeEnd = new Point(Math.Abs(nowMovingPoint.X - topLeftPoint.X), Math.Abs(nowMovingPoint.Y - topLeftPoint.Y));


            Point middlePoint = new Point(relativeEnd.X, relativeStart.Y);




            tempLLinePolyline = CreateXamlPolyline(viewModel.SelectedLineBrush, 2, relativeStart, middlePoint, relativeEnd);
            drawCanvas.Children.Add(tempLLinePolyline);
            Canvas.SetLeft(tempLLinePolyline, topLeftPoint.X);
            Canvas.SetTop(tempLLinePolyline, topLeftPoint.Y);

        }

        private void DrawLine(Point nowMovingPoint)
        {
            var viewModel = this.DataContext as DrawBoardViewModel;
            if (null != tempLine)
            {
                drawCanvas.Children.Remove(tempLine);
            }

            Point positionPoint = (Point)mouseLeftButtonDownPoint;
            Point topLeftPoint = GetElementTopLeftPointInCanvas(positionPoint, nowMovingPoint);
            Point relativeStart = new Point(Math.Abs(positionPoint.X - topLeftPoint.X), Math.Abs(positionPoint.Y - topLeftPoint.Y));
            Point relativeEnd = new Point(Math.Abs(nowMovingPoint.X - topLeftPoint.X), Math.Abs(nowMovingPoint.Y - topLeftPoint.Y));

            //var stroke = this.TryFindResource("Shape.Line.Color") as SolidColorBrush;
            tempLine = CreateXamlLine(viewModel.SelectedLineBrush, 2, relativeStart, relativeEnd, false, false);
            drawCanvas.Children.Add(tempLine);
            Canvas.SetLeft(tempLine, topLeftPoint.X);
            Canvas.SetTop(tempLine, topLeftPoint.Y);

        }

        private void DrawStraightLine(Point nowMovingPoint, string brushResourceKey, int strokeThickness, bool isBus)
        {
            var viewModel = this.DataContext as DrawBoardViewModel;
            if (null != tempLine)
            {
                drawCanvas.Children.Remove(tempLine);
            }

            Point positionPoint = (Point)mouseLeftButtonDownPoint;
            double xOffset = nowMovingPoint.X - positionPoint.X;
            double yOffset = nowMovingPoint.Y - positionPoint.Y;

            Point topLeftPoint = GetElementTopLeftPointInCanvas(positionPoint, nowMovingPoint);
            Point relativeStart = new Point(Math.Abs(positionPoint.X - topLeftPoint.X), Math.Abs(positionPoint.Y - topLeftPoint.Y));
            Point relativeEnd = new Point(Math.Abs(nowMovingPoint.X - topLeftPoint.X), Math.Abs(nowMovingPoint.Y - topLeftPoint.Y));

            bool isVertical = false;

            if (Math.Abs(xOffset) < Math.Abs(yOffset))
            {
                isVertical = true;
                relativeEnd = new Point(Math.Abs(positionPoint.X - topLeftPoint.X), Math.Abs(nowMovingPoint.Y - topLeftPoint.Y));
            }
            else
            {
                isVertical = false;
                relativeEnd = new Point(Math.Abs(nowMovingPoint.X - topLeftPoint.X), Math.Abs(positionPoint.Y - topLeftPoint.Y));
            }

            tempLine = CreateXamlLine(viewModel.SelectedLineBrush, strokeThickness, relativeStart, relativeEnd, isBus, isVertical);
            drawCanvas.Children.Add(tempLine);
            Canvas.SetLeft(tempLine, topLeftPoint.X);
            Canvas.SetTop(tempLine, topLeftPoint.Y);
        }

        private Point GetElementTopLeftPointInCanvas(Point positionPoint, Point nowMovingPoint)
        {
            Point topLeftPoint = new Point();
            double xOffset = nowMovingPoint.X - positionPoint.X;
            double yOffset = nowMovingPoint.Y - positionPoint.Y;

            if (xOffset >= 0 && yOffset >= 0)
            {
                topLeftPoint = new Point(positionPoint.X, positionPoint.Y);
            }
            else if (xOffset >= 0 && yOffset < 0)
            {
                topLeftPoint = new Point(positionPoint.X, nowMovingPoint.Y);
            }
            else if (xOffset < 0 && yOffset <= 0)
            {
                topLeftPoint = new Point(nowMovingPoint.X, nowMovingPoint.Y);
            }
            else if (xOffset < 0 && yOffset > 0)
            {
                topLeftPoint = new Point(nowMovingPoint.X, positionPoint.Y);
            }

            return topLeftPoint;
        }

        private void DrawLabel(Point nowMovingPoint, string styleName)
        {
            if (null != tempLabel)
            {
                drawCanvas.Children.Remove(tempLabel);
            }

            Point positionPoint = (Point)mouseLeftButtonDownPoint;

            double xOffset = nowMovingPoint.X - positionPoint.X;
            double yOffset = nowMovingPoint.Y - positionPoint.Y;

            StringBuilder xamlUIElementBuilder = new StringBuilder();
            xamlUIElementBuilder.Append("<Label xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' ");
            xamlUIElementBuilder.Append("Style='{ DynamicResource " + styleName + "}' ");
            xamlUIElementBuilder.Append("Background='Transparent' BorderThickness='0' IsHitTestVisible='false' ");
            xamlUIElementBuilder.Append("Width='" + Math.Abs(xOffset) + "' ");
            xamlUIElementBuilder.Append("Height='" + Math.Abs(yOffset) + "'/>");
            tempLabel = XamlReader.Parse(xamlUIElementBuilder.ToString()) as Label;

            drawCanvas.Children.Add(tempLabel);
            SetElementInCanvas(tempLabel, xOffset, yOffset, positionPoint, nowMovingPoint);
        }

        private void DrawToggleButton(Point nowMovingPoint, string styleName)
        {
            if (tempToggleButton != null)
            {
                drawCanvas.Children.Remove(tempToggleButton);
            }

            Point positionPoint = (Point)mouseLeftButtonDownPoint;

            double xOffset = nowMovingPoint.X - positionPoint.X;
            double yOffset = nowMovingPoint.Y - positionPoint.Y;

            StringBuilder xamlUIElementBuilder = new StringBuilder();
            xamlUIElementBuilder.Append("<ToggleButton xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' ");
            xamlUIElementBuilder.Append("Style='{ DynamicResource " + styleName + "}' ");
            xamlUIElementBuilder.Append("Background='Transparent' BorderThickness='0' IsHitTestVisible='false' ");
            xamlUIElementBuilder.Append("Width='" + Math.Abs(xOffset) + "' ");
            xamlUIElementBuilder.Append("Height='" + Math.Abs(yOffset) + "'/>");
            tempToggleButton = XamlReader.Parse(xamlUIElementBuilder.ToString()) as ToggleButton;
            drawCanvas.Children.Add(tempToggleButton);

            SetElementInCanvas(tempToggleButton, xOffset, yOffset, positionPoint, nowMovingPoint);
        }

        private void AddDefaultToolItemToggleButton(Point mouseEnteredPoint, string styleName)
        {
            StringBuilder xamlUIElementBuilder = new StringBuilder();
            xamlUIElementBuilder.Append("<ToggleButton xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' ");
            xamlUIElementBuilder.Append("Style='{ DynamicResource " + styleName + "}' ");
            xamlUIElementBuilder.Append("Background='Transparent' BorderThickness='0' IsHitTestVisible='false' />");

            tempToggleButton = XamlReader.Parse(xamlUIElementBuilder.ToString()) as ToggleButton;
            drawCanvas.Children.Add(tempToggleButton);

            Canvas.SetLeft(tempToggleButton, mouseEnteredPoint.X - tempToggleButton.Width / 2);
            Canvas.SetTop(tempToggleButton, mouseEnteredPoint.Y - tempToggleButton.Height / 2);
        }

        public void MoveDefaultToggleButton(Point mouseMovePoint)
        {
            if (null != tempToggleButton)
            {
                Canvas.SetLeft(tempToggleButton, mouseMovePoint.X - tempToggleButton.Width / 2);
                Canvas.SetTop(tempToggleButton, mouseMovePoint.Y - tempToggleButton.Height / 2);
            }
        }

        public void MoveDefaultLable(Point mouseMovePoint)
        {
            if (null != tempLabel)
            {
                Canvas.SetLeft(tempLabel, mouseMovePoint.X - tempLabel.Width / 2);
                Canvas.SetTop(tempLabel, mouseMovePoint.Y - tempLabel.Height / 2);
            }
        }

        private void SetElementInCanvas(UIElement tempToggleButton, double xOffset, double yOffset, Point positionPoint, Point nowMovingPoint)
        {
            if (xOffset >= 0 && yOffset >= 0)
            {
                Canvas.SetLeft(tempToggleButton, positionPoint.X);
                Canvas.SetTop(tempToggleButton, positionPoint.Y);
            }
            else if (xOffset >= 0 && yOffset <= 0)
            {
                Canvas.SetLeft(tempToggleButton, positionPoint.X);
                Canvas.SetTop(tempToggleButton, nowMovingPoint.Y);
            }
            else if (xOffset <= 0 && yOffset >= 0)
            {
                Canvas.SetLeft(tempToggleButton, nowMovingPoint.X);
                Canvas.SetTop(tempToggleButton, positionPoint.Y);
            }
            else if (xOffset <= 0 && yOffset <= 0)
            {
                Canvas.SetLeft(tempToggleButton, nowMovingPoint.X);
                Canvas.SetTop(tempToggleButton, nowMovingPoint.Y);
            }
        }

        #region Add element Name
        private void AddElementName()
        {
            var viewModel = this.DataContext as DrawBoardViewModel;
            nameInputTextBox = new TextBox();
            nameInputTextBox.LostFocus += NameInputTextBox_LostFocus;
            nameInputTextBox.KeyDown += NameInputTextBox_KeyDown;
            nameInputTextBox.Width = 100;
            nameInputTextBox.Height = viewModel.SelectedFontSize + 10;
            nameInputTextBox.FontFamily = viewModel.SelectedFontFamily.FontFamilyEntity;
            nameInputTextBox.FontSize = viewModel.SelectedFontSize;
            nameInputTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(viewModel.SelectedColor));
            drawCanvas.Children.Add(nameInputTextBox);
            double left = Canvas.GetLeft(selectedElements[0]);
            double top = Canvas.GetTop(selectedElements[0]);
            Canvas.SetLeft(nameInputTextBox, left);
            Canvas.SetTop(nameInputTextBox, top - 30);

        }

        private void NameInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                drawCanvas.Focus();
            }
        }

        private void NameInputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as DrawBoardViewModel;
            if (selectedElements.Count == 1)
            {
                if (selectedElements[0] is ToggleButton)
                {
                    var element = selectedElements[0] as ToggleButton;

                    element.Content = nameInputTextBox.Text;
                    if (!string.IsNullOrEmpty(nameInputTextBox.Text))
                    {
                        TextBlock textBlock = new TextBlock();
                        textBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(viewModel.SelectedColor));
                        textBlock.IsHitTestVisible = false;
                        textBlock.Text = nameInputTextBox.Text;
                        textBlock.FontSize = viewModel.SelectedFontSize;
                        drawCanvas.Children.Add(textBlock);
                        double left = Canvas.GetLeft(selectedElements[0]);
                        double top = Canvas.GetTop(selectedElements[0]);
                        Canvas.SetLeft(textBlock, left);
                        Canvas.SetTop(textBlock, top - textBlock.FontSize - 10);
                    }

                }
            }

            if (null != nameInputTextBox)
            {
                drawCanvas.Children.Remove(nameInputTextBox);
                nameInputTextBox = null;
            }
        }
        #endregion


        private bool IsPointInSelectedElements(Point point)
        {
            var children = LogicalTreeHelper.GetChildren(drawCanvas);
            foreach (FrameworkElement item in children)
            {
                if (IsClickInUIElement(point, item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 修改页面元素大小，位置
        /// </summary>
        /// <param name="point">拖拽点</param>
        private void SelectSingleElement(Point point)
        {
            bool IsSelectedUIElement = false;
            ClearResizeMoveAdorner();
            var children = LogicalTreeHelper.GetChildren(drawCanvas);
            foreach (FrameworkElement item in children)
            {
                if (IsClickInUIElement(point, item))
                {
                    IsSelectedUIElement = true;
                    var left = Canvas.GetLeft(item);
                    var top = Canvas.GetTop(item);
                    selectedElements.Add(item);
                    break;
                }
            }

            if (!IsSelectedUIElement)
            {
                ClearResizeMoveAdorner();
                isDrawMultiSelectRect = true;
            }
        }

        private void SelectElementByControl(Point point)
        {
            bool IsSelectedUIElement = false;
            var children = LogicalTreeHelper.GetChildren(drawCanvas);
            foreach (FrameworkElement item in children)
            {
                if (IsClickInUIElement(point, item))
                {
                    IsSelectedUIElement = true;
                    if (!selectedElements.Contains(item))
                    {
                        selectedElements.Add(item);
                    }
                    break;
                }
            }

            if (!IsSelectedUIElement)
            {
                ClearResizeMoveAdorner();
                isDrawMultiSelectRect = true;
            }
        }

        private void SelectMultiElement(Rectangle rect)
        {
            ClearResizeMoveAdorner();
            var children = LogicalTreeHelper.GetChildren(drawCanvas);
            foreach (FrameworkElement item in children)
            {
                if (IsElementInSelectedRect(rect, item))
                {
                    resizeMoveAdorner = new ResizeMoveAdorner(item);
                    selectedUIElement = item;
                    drawCanvasAdornerLayer.Add(resizeMoveAdorner);
                }
            }
        }

        /// <summary>
        /// 点击点是否在元素内
        /// </summary>
        /// <param name="point"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool IsClickInUIElement(Point point, FrameworkElement element)
        {
            Rect elementRect = GetElementRect(element);
            if (elementRect.Contains(point))
            {
                return true;
            }

            return false;
        }

        private bool IsElementInSelectedRect(Rectangle rect, FrameworkElement element)
        {

            Rect elementRect = GetElementRect(element);

            Rect selectRect = GetElementRect(rect);
            if (selectRect.Contains(elementRect))
            {
                return true;
            }

            return false;
        }

        private Rect GetElementRect(FrameworkElement element)
        {
            Point position = new Point(Canvas.GetLeft(element), Canvas.GetTop(element));
            Size elementSize = new Size(element.RenderSize.Width, element.RenderSize.Height);
            Rect elementRect = new Rect(position, elementSize);

            return elementRect;
        }

        /// <summary>
        /// 当前画板清除修改元素装饰器
        /// </summary>
        private void ClearResizeMoveAdorner()
        {
            if (null != resizeMoveAdorner)
            {
                selectedUIElement = null;
                drawCanvasAdornerLayer.Remove(resizeMoveAdorner);
            }

            if (selectedElements.Count > 0)
            {
                foreach (var item in selectedElements)
                {
                    var elementAdorners = drawCanvasAdornerLayer.GetAdorners(item);
                    if (null != elementAdorners && elementAdorners.Length > 0)
                    {
                        for (int i = elementAdorners.Length - 1; i > -1; i--)
                        {
                            drawCanvasAdornerLayer.Remove(elementAdorners[i]);
                        }
                    }
                }

                selectedElements.Clear();
            }
        }

        private Line CreateXamlLine(string stroke, int strokeThickness, Point start, Point end, bool isBus, bool isVertical)
        {
            StringBuilder xamlUIElementBuilder = new StringBuilder();
            xamlUIElementBuilder.Append("<Line xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' ");
            xamlUIElementBuilder.Append("IsHitTestVisible='false' ");
            if (!isBus)
            {
                xamlUIElementBuilder.Append("Stroke='" + stroke + "' ");
            }
            xamlUIElementBuilder.Append("Stretch='Fill' ");
            xamlUIElementBuilder.Append("StrokeThickness='" + strokeThickness + "' ");
            xamlUIElementBuilder.Append("X1='" + start.X + "' ");
            xamlUIElementBuilder.Append("Y1='" + start.Y + "' ");

            xamlUIElementBuilder.Append("X2='" + end.X + "' ");
            xamlUIElementBuilder.Append("Y2='" + end.Y + "'>");
            if (isBus)
            {
                xamlUIElementBuilder.Append("<Line.Stroke>");
                xamlUIElementBuilder.Append(CreateLinearGradientBrush(stroke, isVertical));
                xamlUIElementBuilder.Append("</Line.Stroke>");
            }
            xamlUIElementBuilder.Append("</Line>");

            var line = XamlReader.Parse(xamlUIElementBuilder.ToString()) as Line;
            return line;
        }

        private string CreateLinearGradientBrush(string stroke, bool isVertical)
        {
            StringBuilder brushBuilder = new StringBuilder();
            if (!isVertical)
            {
                brushBuilder.Append("<LinearGradientBrush StartPoint='0.5,0' EndPoint='0.5,1'>");
            }
            else
            {
                brushBuilder.Append("<LinearGradientBrush StartPoint='0,0.5' EndPoint='1,0.5'>");
            }

            brushBuilder.Append("<GradientStop Color='" + stroke + "' Offset='0' />");

            var tempColor = (Color)ColorConverter.ConvertFromString(stroke);
            // tempColor.ScR = tempColor.ScR - (float)0.25;

            brushBuilder.Append("<GradientStop Color='" + stroke + "' Offset='0.3' />");

            tempColor.ScR = tempColor.ScR - (float)0.65;
            tempColor.A = 0x68;

            brushBuilder.Append("<GradientStop Color='" + tempColor.ToString() + "' Offset='1' />");
            brushBuilder.Append("</LinearGradientBrush>");
            return brushBuilder.ToString();
        }

        private Polyline CreateXamlPolyline(string stroke, int strokeThickness, Point start, Point middle, Point end)
        {
            StringBuilder xamlUIElementBuilder = new StringBuilder();
            xamlUIElementBuilder.Append("<Polyline xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' ");
            xamlUIElementBuilder.Append("IsHitTestVisible='false' ");
            xamlUIElementBuilder.Append("Stroke='" + stroke + "' ");
            xamlUIElementBuilder.Append("Stretch='Fill' ");
            xamlUIElementBuilder.Append("StrokeThickness='" + strokeThickness + "' ");
            xamlUIElementBuilder.Append("FillRule='EvenOdd'>");
            xamlUIElementBuilder.Append("<Polyline.Points>");

            xamlUIElementBuilder.Append("<Point ");
            xamlUIElementBuilder.Append("X='" + start.X + "' ");
            xamlUIElementBuilder.Append("Y='" + start.Y + "'/>");

            xamlUIElementBuilder.Append("<Point ");
            xamlUIElementBuilder.Append("X='" + middle.X + "' ");
            xamlUIElementBuilder.Append("Y='" + middle.Y + "'/>");

            xamlUIElementBuilder.Append("<Point ");
            xamlUIElementBuilder.Append("X='" + end.X + "' ");
            xamlUIElementBuilder.Append("Y='" + end.Y + "'/>");

            xamlUIElementBuilder.Append("</Polyline.Points>");

            xamlUIElementBuilder.Append("</Polyline>");

            var lline = XamlReader.Parse(xamlUIElementBuilder.ToString()) as Polyline;

            return lline;
        }

        private void SaveLShapeLine(string lineBrush)
        {
            var finalLShapeLine = CreateXamlPolyline(lineBrush, 2, tempLLinePolyline.Points[0], tempLLinePolyline.Points[1], tempLLinePolyline.Points[2]);
            drawCanvas.Children.Add(finalLShapeLine);
            Canvas.SetLeft(finalLShapeLine, Canvas.GetLeft(tempLLinePolyline));
            Canvas.SetTop(finalLShapeLine, Canvas.GetTop(tempLLinePolyline));

            drawCanvas.Children.Remove(tempLLinePolyline);
            tempLLinePolyline = null;
        }

        private void SaveLine(int strokeThickness, bool isBus)
        {
            var viewModel = this.DataContext as DrawBoardViewModel;
            bool isVertical = false;
            if (tempLine.X1 == tempLine.X2)
            {
                isVertical = true;
            }
            var finalLine = CreateXamlLine(viewModel.SelectedLineBrush, strokeThickness, new Point(tempLine.X1, tempLine.Y1), new Point(tempLine.X2, tempLine.Y2), isBus, isVertical);
            drawCanvas.Children.Add(finalLine);
            Canvas.SetLeft(finalLine, Canvas.GetLeft(tempLine));
            Canvas.SetTop(finalLine, Canvas.GetTop(tempLine));

            drawCanvas.Children.Remove(tempLine);
            tempLine = null;

        }

        private void SaveTrackLabel(string styleName)
        {
            var positionPoint = (Point)mouseLeftButtonDownPoint;

            StringBuilder xamlUIElementBuilder = new StringBuilder();
            xamlUIElementBuilder.Append("<Label xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' ");
            xamlUIElementBuilder.Append("Style='{ DynamicResource " + styleName + "}' ");
            xamlUIElementBuilder.Append("Background='Transparent' BorderThickness='0' IsHitTestVisible='false' ");
            xamlUIElementBuilder.Append("Width='" + tempLabel.Width + "' ");
            xamlUIElementBuilder.Append("Height='" + tempLabel.Height + "'/>");
            Label finalLabel = XamlReader.Parse(xamlUIElementBuilder.ToString()) as Label;

            drawCanvas.Children.Add(finalLabel);
            Canvas.SetLeft(finalLabel, Canvas.GetLeft(tempLabel));
            Canvas.SetTop(finalLabel, Canvas.GetTop(tempLabel));

            drawCanvas.Children.Remove(tempLabel);
            tempLabel = null;
        }

        private void SaveToggleButton(string styleName)
        {
            var positionPoint = (Point)mouseLeftButtonDownPoint;

            StringBuilder xamlUIElementBuilder = new StringBuilder();
            xamlUIElementBuilder.Append("<ToggleButton xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' ");
            xamlUIElementBuilder.Append("Style='{ DynamicResource " + styleName + "}' ");
            xamlUIElementBuilder.Append("Background='Transparent' BorderThickness='0' IsHitTestVisible='false' ");
            xamlUIElementBuilder.Append("Width='" + tempToggleButton.Width + "' ");
            xamlUIElementBuilder.Append("Height='" + tempToggleButton.Height + "'/>");
            ToggleButton finalToggleButton = XamlReader.Parse(xamlUIElementBuilder.ToString()) as ToggleButton;
            drawCanvas.Children.Add(finalToggleButton);
            Canvas.SetLeft(finalToggleButton, Canvas.GetLeft(tempToggleButton));
            Canvas.SetTop(finalToggleButton, Canvas.GetTop(tempToggleButton));

            drawCanvas.Children.Remove(tempToggleButton);
            tempToggleButton = null;
        }

        private string GetToolStyleName(ToolType toolType, Point nowMovingPoint)
        {
            string styleName = string.Empty;
            if (toolType == ToolType.StraightLine || toolType == ToolType.LShapeLine)
            {
                //没有用到资源
            }
            else
            {
                styleName = toolType.ToString() + "Style";
            }

            return styleName;
        }

        private void DeleteSelectedElements()
        {
            if (selectedElements.Count > 0)
            {
                foreach (var item in selectedElements)
                {
                    drawCanvas.Children.Remove(item);
                }
                selectedElements.Clear();
            }
        }
        private void CopySelectedElements()
        {
            if (selectedElements.Count > 0)
            {
                List<string> copyedElementList = new List<string>();
                foreach (var item in selectedElements)
                {
                    string copyedObj = XamlWriter.Save(item);
                    copyedElementList.Add(copyedObj);
                }

                string serializedObj = SerializeUtilities.Serialize(copyedElementList);

                Clipboard.SetDataObject(serializedObj);
            }
        }

        private void PasteSelectedElements()
        {
            string graphicText = string.Empty;

            IDataObject iData = Clipboard.GetDataObject();

            if (iData.GetDataPresent(DataFormats.Text))
            {
                graphicText = (String)iData.GetData(DataFormats.Text);
            }
            if (!string.IsNullOrEmpty(graphicText))
            {
                var elementList = SerializeUtilities.Desrialize<List<string>>(graphicText);

                if (null != elementList)
                {
                    foreach (var item in elementList)
                    {
                        FrameworkElement copyedElement = XamlReader.Parse(item) as FrameworkElement;
                        if (null != copyedElement)
                        {
                            drawCanvas.Children.Add(copyedElement);
                            double left = Canvas.GetLeft(copyedElement);
                            double top = Canvas.GetTop(copyedElement);
                            Canvas.SetLeft(copyedElement, left + 8);
                            Canvas.SetTop(copyedElement, top + 8);
                        }
                    }
                }
            }
        }

        private void drawCanvas_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            drawCanvasAdornerLayer = AdornerLayer.GetAdornerLayer(this.drawCanvas);
        }



        private void MicroAdjustElementLocation(Vector adjustVector)
        {
            if (selectedElements.Count > 0)
            {
                foreach (var item in selectedElements)
                {
                    double left = Canvas.GetLeft(item);
                    double top = Canvas.GetTop(item);
                    Canvas.SetLeft(item, left + adjustVector.X);
                    Canvas.SetTop(item, top + adjustVector.Y);
                }
            }
        }

        public void ExecuteCommand(string commandType)
        {
            switch (commandType)
            {
                case "ZoomOut":
                    drawCanvas.Scale += 0.2;
                    drawCanvas.Width = drawCanvas.RenderSize.Width * 1.2;
                    drawCanvas.Height = drawCanvas.RenderSize.Height * 1.2;
                    break;
                case "ZoomIn":
                    if (drawCanvas.Scale > 0.7)
                    {
                        drawCanvas.Scale -= 0.2;
                        drawCanvas.Width = drawCanvas.RenderSize.Width * 0.8;
                        drawCanvas.Height = drawCanvas.RenderSize.Height * 0.8;
                    }
                    break;
                case "ZoomRestore":

                    RestoreZoomedCanvas();
                    break;
                case "Copy":
                    CopySelectedElements();
                    break;
                case "Delete":
                    DeleteSelectedElements();
                    break;
                case "Paste":
                    PasteSelectedElements();
                    break;
                case "Rotate":
                    RotateSelectedElement();
                    break;
                case "VerticalCenterAlignment":
                    AlignSelectedElementVerticalCenter();
                    break;
                case "HorizontialCenterAlignment":
                    AlignSelectedElementHorizontialCenter();
                    break;
                default:
                    break;
            }
        }

        private void AlignSelectedElementHorizontialCenter()
        {
            if (selectedElements.Count > 1)
            {
                double top = Canvas.GetTop(selectedElements[0]);
                double height = selectedElements[0].RenderSize.Height;
                double centerY = top + height / 2;

                for (int i = 1; i < selectedElements.Count; i++)
                {
                    height = selectedElements[i].RenderSize.Height;
                    top = centerY - height / 2;
                    Canvas.SetTop(selectedElements[i], top);
                }
            }
        }

        private void AlignSelectedElementVerticalCenter()
        {
            if (selectedElements.Count > 1)
            {
                double left = Canvas.GetLeft(selectedElements[0]);
                double width = selectedElements[0].RenderSize.Width;
                double centerX = left + width / 2;

                for (int i = 1; i < selectedElements.Count; i++)
                {
                    width = selectedElements[i].RenderSize.Width;
                    left = centerX - width / 2;
                    Canvas.SetLeft(selectedElements[i], left);
                }
            }
        }

        public void ExecuteCommand(string commandType, object parameter)
        {
            switch (commandType)
            {
                case "ResizeCanvas":
                    double ratio = (double)parameter;
                    this.canvasSizeRatio = ratio;
                    ResizeCanvas(ratio);
                    break;
                default:
                    break;
            }
        }

        private void ResizeCanvas(double ratio)
        {
            RestoreZoomedCanvas();
            drawCanvas.Width = userControlGrid.RenderSize.Width * ratio;
            drawCanvas.Height = userControlGrid.RenderSize.Height * ratio;
        }

        /// <summary>
        /// 还原放大后画布大小
        /// </summary>
        private void RestoreZoomedCanvas()
        {
            drawCanvas.Scale = 1;
            drawCanvas.Offset = new Point(0, 0);
            double maxWidth = 0;
            double maxHeight = 0;
            GetMaxWidthHeightInCanvas(out maxWidth, out maxHeight);

            drawCanvas.Width = userControlGrid.RenderSize.Width * canvasSizeRatio;
            drawCanvas.Height = userControlGrid.RenderSize.Height * canvasSizeRatio;
        }

        private void RotateSelectedElement()
        {
            if (selectedElements.Count == 1)
            {
                double left = Canvas.GetLeft(selectedElements[0]);
                double top = Canvas.GetTop(selectedElements[0]);
                double centerX = selectedElements[0].RenderSize.Width / 2;
                double centerY = selectedElements[0].RenderSize.Height / 2;
                double angle = 0;
                if (null != selectedElements[0].RenderTransform)
                {
                    if (selectedElements[0].RenderTransform is RotateTransform)
                    {
                        var original = selectedElements[0].RenderTransform as RotateTransform;
                        angle = original.Angle;
                    }
                }
                angle += 90;
                RotateTransform rotateTransform = null;
                rotateTransform = new RotateTransform(angle, centerX, centerY);
                selectedElements[0].RenderTransform = rotateTransform;


                //Point newLocation = newLocation = new Point(left + centerX - centerY, top + centerY - centerX);               
                //Canvas.SetLeft(selectedElements[0], newLocation.X);
                //Canvas.SetTop(selectedElements[0], newLocation.Y);

                var adorners = drawCanvasAdornerLayer.GetAdorners(selectedElements[0]);
                if (null != adorners)
                {
                    for (int i = 0; i < adorners.Length; i++)
                    {
                        if (adorners[i] is ResizeMoveAdorner)
                        {
                            drawCanvasAdornerLayer.Remove(adorners[i]);
                        }
                    }
                }

                var refreshAdorner = new ResizeMoveAdorner(selectedElements[0]);
                drawCanvasAdornerLayer.Add(refreshAdorner);

            }
        }

        public void SaveGraphic(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    //当需要保存当前图形时，调用XamlWriter的以下方法将Canvas对象直接序列化到文件流中
                    XamlWriter.Save(userControlGrid.Children[0], fs);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void ImportFile(ScrollViewer scrollviewer)
        {
            userControlGrid.Children.Clear();
            userControlGrid.Children.Add(scrollviewer);

        }

        private void drawCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            ToolItem toolItem = ToolManager.GetInstance().Tools.Find(item => item.IsSelected);
            var point = e.GetPosition(drawCanvas);
            if (null != toolItem)
            {
                if (toolItem.ToolType == ToolType.CanvasMove)
                {
                    this.Cursor = Cursors.Hand;
                }
                else if (toolItem.IsDrawShape == true)
                {
                    if (toolItem.ControlType == ToolControlType.ToggleButton)
                    {
                        this.Cursor = Cursors.None;
                        AddElementItemInZoomableCanvas(toolItem, point);
                    }
                    else if (toolItem.ControlType == ToolControlType.Label && toolItem.ToolType.ToString() != "TrackLine")
                    {
                        this.Cursor = Cursors.None;
                        AddElementItemInZoomableCanvas(toolItem, point);
                    }
                    //  this.Cursor = Cursors.None;
                    //todo:加入一个图形元素和位置指示标志，并在鼠标移动的时候移动这个图形元素，等待鼠标左键按下，正式加入图元并移除位置指示标识，恢复指针样式
                }
            }


        }

        private void AddElementItemInZoomableCanvas(ToolItem toolItem, Point enteredPoint)
        {
            if (toolItem.ControlType == ToolControlType.ToggleButton)
            {
                AddDefaultToolItemToggleButton(enteredPoint, toolItem.ToolType.ToString() + "Style");
            }
            else if (toolItem.ControlType == ToolControlType.Label && toolItem.ToolType.ToString() != "TrackLine")
            {
                AddDefaultToolItemLabel(enteredPoint, toolItem.ToolType.ToString() + "Style");
            }
        }

        private void AddDefaultToolItemLabel(Point mouseEnteredPoint, string styleName)
        {
            StringBuilder xamlUIElementBuilder = new StringBuilder();
            xamlUIElementBuilder.Append("<Label xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' ");
            xamlUIElementBuilder.Append("Style='{ DynamicResource " + styleName + "}' ");
            xamlUIElementBuilder.Append("Background='Transparent' BorderThickness='0' IsHitTestVisible='false' />");
            tempLabel = XamlReader.Parse(xamlUIElementBuilder.ToString()) as Label;

            drawCanvas.Children.Add(tempLabel);
            Canvas.SetLeft(tempLabel, mouseEnteredPoint.X - tempLabel.Width / 2);
            Canvas.SetTop(tempLabel, mouseEnteredPoint.Y - tempLabel.Height / 2);
        }

        private void drawCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            ToolItem toolItem = ToolManager.GetInstance().Tools.Find(item => item.IsSelected);
            if (null != toolItem)
            {
                if (toolItem.ControlType == ToolControlType.ToggleButton)
                {
                    if (null != tempToggleButton)
                    {
                        drawCanvas.Children.Remove(tempToggleButton);
                    }
                }
                else if (toolItem.ControlType == ToolControlType.Label)
                {
                    if (null != tempLabel)
                    {
                        drawCanvas.Children.Remove(tempLabel);
                    }
                }
            }

            this.Cursor = Cursors.Arrow;
            if (multiSelectRect != null)
            {

                drawCanvas.Children.Remove(multiSelectRect);
                multiSelectRect = null;
            }
        }


        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteSelectedElements();
            }

            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.C)
            {
                CopySelectedElements();
            }

            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                PasteSelectedElements();
            }

            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                canMutiSelectByControl = true;
            }

        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            Vector adjustVector = new Vector();
            if (e.Key == Key.Up)
            {
                adjustVector = new Vector(0, -1);
                MicroAdjustElementLocation(adjustVector);
            }
            else if (e.Key == Key.Down)
            {
                adjustVector = new Vector(0, 1);
                MicroAdjustElementLocation(adjustVector);
            }
            else if (e.Key == Key.Left)
            {
                adjustVector = new Vector(-1, 0);
                MicroAdjustElementLocation(adjustVector);
            }
            else if (e.Key == Key.Right)
            {
                adjustVector = new Vector(1, 0);
                MicroAdjustElementLocation(adjustVector);
            }
            else if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                canMutiSelectByControl = false;
            }

        }

        private void drawCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeCanvas();
        }

        private void ResizeCanvas()
        {
            double maxWidth = 0;
            double maxHeight = 0;
            GetMaxWidthHeightInCanvas(out maxWidth, out maxHeight);

            if (maxWidth > drawCanvas.RenderSize.Width)
            {
                drawCanvas.Width = maxWidth;
            }
            else
            {
                if (drawCanvas.Width < userControlGrid.RenderSize.Width)
                {
                    drawCanvas.Width = userControlGrid.RenderSize.Width;
                }

            }

            if (maxHeight > drawCanvas.RenderSize.Height)
            {
                drawCanvas.Height = maxHeight;
            }
            else
            {
                if (drawCanvas.Height < userControlGrid.RenderSize.Height)
                {
                    drawCanvas.Height = userControlGrid.RenderSize.Height;
                }
            }
        }

        private void GetMaxWidthHeightInCanvas(out double maxWidth, out double maxHeight)
        {
            maxWidth = 0;
            maxHeight = 0;
            var children = LogicalTreeHelper.GetChildren(drawCanvas);
            if (children != null)
            {
                foreach (FrameworkElement item in children)
                {
                    double left = Canvas.GetLeft(item);
                    double top = Canvas.GetTop(item);
                    double areaWidth = left + item.RenderSize.Width;
                    double areaHeight = top + item.RenderSize.Height;
                    if (maxWidth < areaWidth)
                    {
                        maxWidth = areaWidth;
                    }
                    if (maxHeight < areaHeight)
                    {
                        maxHeight = areaHeight;
                    }
                }
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeCanvas();
        }
    }
}
