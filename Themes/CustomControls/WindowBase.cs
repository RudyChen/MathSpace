using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Themes.CustomControls
{
    /// <summary>
    /// WindowBase.xaml 的交互逻辑
    /// </summary>
    public partial class WindowBase : Window
    {
        public WindowBase()
        {
            InitializeStyle();
            FullScreenManager.RepairWpfWindowFullScreenBehavior(this);
            this.SourceInitialized += delegate (object sender, EventArgs e)
            {
                this._HwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            };
            this.Loaded += delegate
            {
                InitializeEvent();
            };
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ResourceDictionary resource = new ResourceDictionary() { Source = new Uri("Themes;component/CustomControls/WindowBaseStyle.xaml", UriKind.Relative) };
            this.Style = (Style)resource["CustomWindowStyle"];
        }

        protected virtual void MinWin()
        {
            this.WindowState = WindowState.Minimized;
        }
        private void InitializeEvent()
        {
            DockPanel titleBarDockPanel = null;
            Border dropShadowBorder = null;
            Grid resizeWindowGrid = null;          
            Button closeBtn = null;
            ToggleButton windowStateButton = null;
            Button minBtn = null;
            foreach (Setter item in this.Style.Setters)
            {
                if (item.Property.Name == "Template")
                {
                    ControlTemplate controlTemplate = item.Value as ControlTemplate;                  
                    titleBarDockPanel = (DockPanel)controlTemplate.FindName("WinTitleBarDockPanel", this);
                    dropShadowBorder = (Border)controlTemplate.FindName("WinShandowBorder", this);
                    resizeWindowGrid = (Grid)controlTemplate.FindName("ResizeWindowGrid", this);                  
                    closeBtn = (Button)controlTemplate.FindName("WindowCloseButton", this);
                    windowStateButton = (ToggleButton)controlTemplate.FindName("WindowStateButton", this);
                    minBtn = (Button)controlTemplate.FindName("WindowMinizeButton", this);
                    break;
                }
            }
            windowStateButton.Click += delegate
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    resizeWindowGrid.RowDefinitions[0].Height = new GridLength(6, GridUnitType.Pixel);
                    resizeWindowGrid.RowDefinitions[2].Height = new GridLength(6, GridUnitType.Pixel);

                    resizeWindowGrid.ColumnDefinitions[0].Width = new GridLength(6, GridUnitType.Pixel);
                    resizeWindowGrid.ColumnDefinitions[2].Width = new GridLength(6, GridUnitType.Pixel);

                    dropShadowBorder.BorderThickness = new Thickness(1);

                    this.WindowState = WindowState.Normal;
                }
                else if (this.WindowState == WindowState.Normal)
                {
                    resizeWindowGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                    resizeWindowGrid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Pixel);

                    resizeWindowGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                    resizeWindowGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);

                    dropShadowBorder.BorderThickness = new Thickness(0);
                    this.WindowState = WindowState.Maximized;
                }

               
            };

            this.SizeChanged += delegate
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    resizeWindowGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);
                    resizeWindowGrid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Pixel);

                    resizeWindowGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                    resizeWindowGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);

                    dropShadowBorder.BorderThickness = new Thickness(0);

                   // this.WindowState = WindowState.Normal;
                }
                else if (this.WindowState == WindowState.Normal)
                {
                    resizeWindowGrid.RowDefinitions[0].Height = new GridLength(6, GridUnitType.Pixel);
                    resizeWindowGrid.RowDefinitions[2].Height = new GridLength(6, GridUnitType.Pixel);

                    resizeWindowGrid.ColumnDefinitions[0].Width = new GridLength(6, GridUnitType.Pixel);
                    resizeWindowGrid.ColumnDefinitions[2].Width = new GridLength(6, GridUnitType.Pixel);

                    dropShadowBorder.BorderThickness = new Thickness(1);
                   // this.WindowState = WindowState.Maximized;
                }
            };

            minBtn.Click += delegate
            {
                MinWin();
            };

            closeBtn.Click += delegate
            {
                this.Close();
            };

            titleBarDockPanel.MouseMove += delegate (object sender, MouseEventArgs e)
            {
                this.Cursor = Cursors.Arrow;
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                    e.Handled = true;
                }
            };

            resizeWindowGrid.PreviewMouseMove+= ResizeBorder_PreviewMouseMove;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void InitializeStyle()
        {
            this.Style = (Style)Application.Current.Resources["CustomWindowStyle"];
        }


        #region ResizeWindow
        ResizeDirection direction;
        private HwndSource _HwndSource;
        private const int WM_SYSCOMMAND = 0x112;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        bool IsTop = false;
        bool IsLeft = false;
        bool IsRight = false;
        bool IsBottom = false;
        private void ChangeResizeCursor(object sender, MouseEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.Cursor = Cursors.Arrow;
                return;
            }
            var resizeGrid=sender as Grid;
            var position = e.GetPosition(resizeGrid);
            IsTop = position.Y >= 0 && position.Y <= 6;
            IsLeft = position.X >= 0 && position.X <= 6;
             IsRight = position.X >= this.Width - 6 && position.X <= this.Width;
            IsBottom = position.Y >= this.Height - 6 && position.Y <= this.Height;
            if (IsTop && IsLeft)
            {
                this.Cursor = Cursors.SizeNWSE;
                direction = ResizeDirection.TopLeft;
            }
            else if (IsRight && IsBottom)
            {
                this.Cursor = Cursors.SizeNWSE;
                direction = ResizeDirection.BottomRight;
            }
            else if (IsTop && IsRight)
            {
                this.Cursor = Cursors.SizeNESW;
                direction = ResizeDirection.TopRight;
            }
            else if (IsLeft && IsBottom)
            {
                this.Cursor = Cursors.SizeNESW;
                direction = ResizeDirection.BottomLeft;
            }
            else if (IsTop)
            {
                this.Cursor = Cursors.SizeNS;
                direction = ResizeDirection.Top;
            }
            else if (IsBottom)
            {
                this.Cursor = Cursors.SizeNS;
                direction = ResizeDirection.Bottom;
            }
            else if (IsLeft)
            {
                this.Cursor = Cursors.SizeWE;
                direction = ResizeDirection.Left;
            }
            else if (IsRight)
            {
                this.Cursor = Cursors.SizeWE;
                direction = ResizeDirection.Right;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
                direction = ResizeDirection.Default;
            }
        }

        private void ResizeBorder_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ChangeResizeCursor(sender, e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (direction == ResizeDirection.Default) return;
                ResizeWindow(direction);
            }
        }

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(_HwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + direction), IntPtr.Zero);
        }
        #endregion
    }

    /// <summary>
    /// 鼠标方向
    /// </summary>
    public enum ResizeDirection
    {
        Default = 0,
        /// <summary>
        /// 左
        /// </summary>
        Left = 1,
        /// <summary>
        /// 右
        /// </summary>
        Right = 2,
        /// <summary>
        /// 上
        /// </summary>
        Top = 3,
        /// <summary>
        /// 左上
        /// </summary>
        TopLeft = 4,
        /// <summary>
        /// 右上
        /// </summary>
        TopRight = 5,
        /// <summary>
        /// 下
        /// </summary>
        Bottom = 6,
        /// <summary>
        /// 左下
        /// </summary>
        BottomLeft = 7,
        /// <summary>
        /// 右下
        /// </summary>
        BottomRight = 8,
    }

}
