using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MathGraphics
{
    class ResizeMoveAdorner : Adorner
    {
        const double THUMB_SIZE = 15;
        const double MINIMAL_SIZE = 20;
        const double MOVE_OFFSET = 10;
        const double THUMB_BORDER_THICKNESS = 4;

        Thumb tl, tr, bl, br;
        Thumb leftThumb, topThumb, rightThumb, bottomThumb;
        Thumb mov;
        VisualCollection visCollec;

        public ResizeMoveAdorner(UIElement adorned)
            : base(adorned)
        {
            visCollec = new VisualCollection(this);


            visCollec.Add(tl = GetResizeThumb(Cursors.SizeNWSE, HorizontalAlignment.Left, VerticalAlignment.Top));
            visCollec.Add(tr = GetResizeThumb(Cursors.SizeNESW, HorizontalAlignment.Right, VerticalAlignment.Top));
            visCollec.Add(bl = GetResizeThumb(Cursors.SizeNESW, HorizontalAlignment.Left, VerticalAlignment.Bottom));
            visCollec.Add(br = GetResizeThumb(Cursors.SizeNWSE, HorizontalAlignment.Right, VerticalAlignment.Bottom));
            visCollec.Add(leftThumb = GetResizeHorizontalThumb(Cursors.SizeWE, HorizontalAlignment.Left));
            visCollec.Add(rightThumb = GetResizeHorizontalThumb(Cursors.SizeWE, HorizontalAlignment.Right));
            visCollec.Add(topThumb = GetResizeVerticalThumb(Cursors.SizeNS, VerticalAlignment.Top));
            visCollec.Add(bottomThumb = GetResizeVerticalThumb(Cursors.SizeNS, VerticalAlignment.Bottom));

            visCollec.Add(mov = GetMoveThumb());

            /*
            if (AdornedElement.RenderSize.Width < THUMB_SIZE * 2)
            {
                visCollec.Remove(tl);
                visCollec.Remove(tr);
                visCollec.Remove(bl);
                visCollec.Remove(br);
                visCollec.Remove(leftThumb);
                visCollec.Remove(rightThumb);
            }
            else if (AdornedElement.RenderSize.Width < THUMB_SIZE * 3)
            {
                visCollec.Remove(topThumb);
                visCollec.Remove(bottomThumb);
            }

            if (AdornedElement.RenderSize.Height < THUMB_SIZE * 2)
            {
                visCollec.Remove(tl);
                visCollec.Remove(tr);
                visCollec.Remove(bl);
                visCollec.Remove(br);
                visCollec.Remove(topThumb);
                visCollec.Remove(bottomThumb);
            }
            else if (AdornedElement.RenderSize.Height < THUMB_SIZE * 3)
            {
                visCollec.Remove(leftThumb);
                visCollec.Remove(rightThumb);
            }
            */
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double offset = THUMB_SIZE / 2;
            Size sz = new Size(THUMB_SIZE, THUMB_SIZE);
            var element = AdornedElement as FrameworkElement;
            if (element.RenderTransform is RotateTransform)
            {
                var rotateTransform = element.RenderTransform as RotateTransform;
                if (rotateTransform.Angle % 360 == 0)
                {
                    tl.Arrange(new Rect(new Point(-offset, -offset), sz));
                    tr.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, -offset), sz));
                    bl.Arrange(new Rect(new Point(-offset, AdornedElement.RenderSize.Height - offset), sz));
                    br.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, AdornedElement.RenderSize.Height - offset), sz));
                }
                else if (rotateTransform.Angle % 360 == 90)
                {
                    tl.Arrange(new Rect(new Point(-offset, AdornedElement.RenderSize.Height - offset), sz));
                    tr.Arrange(new Rect(new Point(-offset, -offset), sz));
                    bl.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, AdornedElement.RenderSize.Height - offset), sz));
                    br.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, -offset), sz));
                }
                else if (rotateTransform.Angle % 360 == 180)
                {
                    tl.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, AdornedElement.RenderSize.Height - offset), sz));
                    tr.Arrange(new Rect(new Point(-offset, AdornedElement.RenderSize.Height - offset), sz));
                    bl.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, -offset), sz));
                    br.Arrange(new Rect(new Point(-offset, -offset), sz));
                }
                else if (rotateTransform.Angle % 360 == 270)
                {
                    tl.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, -offset), sz));
                    tr.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, AdornedElement.RenderSize.Height - offset), sz));
                    bl.Arrange(new Rect(new Point(-offset, -offset), sz));
                    br.Arrange(new Rect(new Point(-offset, AdornedElement.RenderSize.Height - offset), sz));
                }
            }
            else
            {
                tl.Arrange(new Rect(new Point(-offset, -offset), sz));
                tr.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, -offset), sz));
                bl.Arrange(new Rect(new Point(-offset, AdornedElement.RenderSize.Height - offset), sz));
                br.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, AdornedElement.RenderSize.Height - offset), sz));
            }



            mov.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width / 2 - offset, AdornedElement.RenderSize.Height / 2 - offset), sz));

            leftThumb.Arrange(new Rect(new Point(-offset, AdornedElement.RenderSize.Height / 2 - offset), sz));
            rightThumb.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width - offset, AdornedElement.RenderSize.Height / 2 - offset), sz));
            topThumb.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width / 2 - offset, -offset), sz));
            bottomThumb.Arrange(new Rect(new Point(AdornedElement.RenderSize.Width / 2 - offset, AdornedElement.RenderSize.Height - offset), sz));

            return finalSize;
        }


        void Resize(FrameworkElement ff)
        {
            if (Double.IsNaN(ff.Width))
                ff.Width = ff.RenderSize.Width;
            if (Double.IsNaN(ff.Height))
                ff.Height = ff.RenderSize.Height;
        }

        Thumb GetMoveThumb()
        {
            var thumb = new Thumb()
            {
                Width = THUMB_SIZE,
                Height = THUMB_SIZE,
                Cursor = Cursors.SizeAll,
                Template = new ControlTemplate(typeof(Thumb))
                {
                    VisualTree = GetFactory(GetMoveEllipseBack())
                }
            };
            thumb.DragDelta += (s, e) =>
            {
                var element = AdornedElement as FrameworkElement;
                if (element == null)
                    return;

                SetElementLocation(element, e.VerticalChange, e.HorizontalChange);

            };
            return thumb;
        }

        private void SetElementLocation(FrameworkElement element, double verticalChange, double horizontalChange)
        {
            if (element.RenderTransform is RotateTransform)
            {
                var rotateTransform = element.RenderTransform as RotateTransform;
                if (rotateTransform.Angle % 360 == 0)
                {
                    Canvas.SetLeft(element, Canvas.GetLeft(element) + horizontalChange);
                    Canvas.SetTop(element, Canvas.GetTop(element) + verticalChange);
                }
                else if (rotateTransform.Angle % 360 == 90)
                {
                    Canvas.SetLeft(element, Canvas.GetLeft(element) - verticalChange);
                    Canvas.SetTop(element, Canvas.GetTop(element) + horizontalChange);
                }
                else if (rotateTransform.Angle % 360 == 180)
                {
                    Canvas.SetLeft(element, Canvas.GetLeft(element) - horizontalChange);
                    Canvas.SetTop(element, Canvas.GetTop(element) - verticalChange);
                }
                else if (rotateTransform.Angle % 360 == 270)
                {
                    Canvas.SetLeft(element, Canvas.GetLeft(element) + verticalChange);
                    Canvas.SetTop(element, Canvas.GetTop(element) - horizontalChange);
                }

            }
            else
            {
                Canvas.SetLeft(element, Canvas.GetLeft(element) + horizontalChange);
                Canvas.SetTop(element, Canvas.GetTop(element) + verticalChange);
            }
        }

        Thumb GetResizeHorizontalThumb(Cursor cur, HorizontalAlignment hor)
        {
            var thumb = new Thumb()
            {
                Background = Brushes.Red,
                Width = THUMB_SIZE,
                Height = THUMB_SIZE,
                HorizontalAlignment = hor,
                Cursor = cur,
                Template = new ControlTemplate(typeof(Thumb))
                {
                    VisualTree = GetFactory(new SolidColorBrush(Colors.Green))
                }
            };
            thumb.DragDelta += (s, e) =>
            {
                var element = AdornedElement as FrameworkElement;
                if (element == null)
                    return;

                Resize(element);

                switch (thumb.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        if (element.Width - e.HorizontalChange > MINIMAL_SIZE)
                        {
                            element.Width -= e.HorizontalChange;
                            Canvas.SetLeft(element, Canvas.GetLeft(element) + e.HorizontalChange);
                        }
                        break;
                    case HorizontalAlignment.Right:
                        if (element.Width + e.HorizontalChange > MINIMAL_SIZE)
                        {
                            element.Width += e.HorizontalChange;
                        }
                        break;
                }
                e.Handled = true;
            };
            return thumb;
        }


        Thumb GetResizeVerticalThumb(Cursor cur, VerticalAlignment ver)
        {
            var thumb = new Thumb()
            {
                Background = Brushes.Red,
                Width = THUMB_SIZE,
                Height = THUMB_SIZE,
                VerticalAlignment = ver,
                Cursor = cur,
                Template = new ControlTemplate(typeof(Thumb))
                {
                    VisualTree = GetFactory(new SolidColorBrush(Colors.Green))
                }
            };
            thumb.DragDelta += (s, e) =>
            {
                var element = AdornedElement as FrameworkElement;
                if (element == null)
                    return;

                Resize(element);

                switch (thumb.VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        if (element.Height + e.VerticalChange > MINIMAL_SIZE)
                        {
                            element.Height += e.VerticalChange;
                        }
                        break;
                    case VerticalAlignment.Top:
                        if (element.Height - e.VerticalChange > MINIMAL_SIZE)
                        {
                            element.Height -= e.VerticalChange;
                            Canvas.SetTop(element, Canvas.GetTop(element) + e.VerticalChange);
                        }
                        break;
                }

                e.Handled = true;
            };
            return thumb;
        }

        Thumb GetResizeThumb(Cursor cur, HorizontalAlignment hor, VerticalAlignment ver)
        {
            var thumb = new Thumb()
            {
                Background = Brushes.Red,
                Width = THUMB_SIZE,
                Height = THUMB_SIZE,
                HorizontalAlignment = hor,
                VerticalAlignment = ver,
                Cursor = cur,
                Template = new ControlTemplate(typeof(Thumb))
                {
                    VisualTree = GetFactory(new SolidColorBrush(Colors.Green))
                }
            };
            thumb.DragDelta += (s, e) =>
            {
                var element = AdornedElement as FrameworkElement;
                if (element == null)
                    return;

                Resize(element);

                if (element.RenderTransform is RotateTransform)
                {
                    var rotateTransform = element.RenderTransform as RotateTransform;
                    if (rotateTransform.Angle % 360 == 0)
                    {
                        ResizeNormalElement(element, e.VerticalChange, e.HorizontalChange, thumb);
                    }
                    else if (rotateTransform.Angle % 360 == 90)
                    {
                        Resize90RotateElement(element, e.VerticalChange, e.HorizontalChange, thumb);
                    }
                    else if (rotateTransform.Angle % 360 == 180)
                    {
                        Resize180RotateElement(element, e.VerticalChange, e.HorizontalChange, thumb);
                    }
                    else if (rotateTransform.Angle % 360 == 270)
                    {
                        Resize270RotateElement(element, e.VerticalChange, e.HorizontalChange, thumb);
                    }
                }
                else
                {
                    ResizeNormalElement(element, e.VerticalChange, e.HorizontalChange, thumb);
                }



                e.Handled = true;
            };
            return thumb;
        }

        private void ResizeNormalElement(FrameworkElement element, double verticalChange, double horizontalChange, Thumb thumb)
        {
            switch (thumb.VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    if (element.Height + verticalChange > MINIMAL_SIZE)
                    {
                        element.Height += verticalChange;
                    }
                    break;
                case VerticalAlignment.Top:
                    if (element.Height - verticalChange > MINIMAL_SIZE)
                    {
                        element.Height -= verticalChange;
                        Canvas.SetTop(element, Canvas.GetTop(element) + verticalChange);
                    }
                    break;
            }
            switch (thumb.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    if (element.Width - horizontalChange > MINIMAL_SIZE)
                    {
                        element.Width -= horizontalChange;
                        Canvas.SetLeft(element, Canvas.GetLeft(element) + horizontalChange);
                    }
                    break;
                case HorizontalAlignment.Right:
                    if (element.Width + horizontalChange > MINIMAL_SIZE)
                    {
                        element.Width += horizontalChange;
                    }
                    break;
            }
        }

        private void Resize90RotateElement(FrameworkElement element, double verticalChange, double horizontalChange, Thumb thumb)
        {
            switch (thumb.VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    if (element.Width + horizontalChange > MINIMAL_SIZE)
                    {
                        element.Width += horizontalChange;
                    }
                    break;
                case VerticalAlignment.Top:
                    if (element.Width - horizontalChange > MINIMAL_SIZE)
                    {
                        element.Width -= horizontalChange;
                        Canvas.SetTop(element, Canvas.GetTop(element) + horizontalChange);
                    }
                    break;
            }
            switch (thumb.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    if (element.Height + verticalChange > MINIMAL_SIZE)
                    {
                        element.Height += verticalChange;
                    }
                    break;
                case HorizontalAlignment.Right:
                    if (element.Height - verticalChange > MINIMAL_SIZE)
                    {
                        element.Height -= verticalChange;
                        Canvas.SetLeft(element, Canvas.GetLeft(element) - verticalChange);
                    }
                    break;
            }
        }

        private void Resize180RotateElement(FrameworkElement element, double verticalChange, double horizontalChange, Thumb thumb)
        {
            switch (thumb.VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    if (element.Height - verticalChange > MINIMAL_SIZE)
                    {
                        element.Height -= verticalChange;

                        Canvas.SetTop(element, Canvas.GetTop(element) - verticalChange);
                    }
                    break;
                case VerticalAlignment.Top:
                    if (element.Height + verticalChange > MINIMAL_SIZE)
                    {
                        element.Height += verticalChange;
                    }
                    break;
            }
            switch (thumb.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    if (element.Width + horizontalChange > MINIMAL_SIZE)
                    {
                        element.Width += horizontalChange;
                    }
                    break;
                case HorizontalAlignment.Right:
                    if (element.Width - horizontalChange > MINIMAL_SIZE)
                    {
                        element.Width -= horizontalChange;
                        Canvas.SetLeft(element, Canvas.GetLeft(element) - horizontalChange);
                    }
                    break;
            }
        }

        private void Resize270RotateElement(FrameworkElement element, double verticalChange, double horizontalChange, Thumb thumb)
        {
            switch (thumb.VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    if (element.Width - horizontalChange > MINIMAL_SIZE)
                    {
                        element.Width -= horizontalChange;

                        Canvas.SetTop(element, Canvas.GetTop(element) - horizontalChange);
                    }

                    break;
                case VerticalAlignment.Top:
                    if (element.Width + horizontalChange > MINIMAL_SIZE)
                    {
                        element.Width += horizontalChange;

                    }
                    break;
            }
            switch (thumb.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    if (element.Height - verticalChange > MINIMAL_SIZE)
                    {
                        element.Height -= verticalChange;
                        Canvas.SetLeft(element, Canvas.GetLeft(element) + verticalChange);
                    }
                    break;
                case HorizontalAlignment.Right:

                    if (element.Height + verticalChange > MINIMAL_SIZE)
                    {
                        element.Height += verticalChange;

                    }
                    break;
            }
        }

        Brush GetMoveEllipseBack()
        {
            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
            var geometry = new RectangleGeometry();
            TileBrush bsh = new DrawingBrush(new GeometryDrawing(Brushes.Red, new Pen(Brushes.Red, 2), geometry));
            bsh.Stretch = Stretch.Fill;
            return bsh;
        }

        FrameworkElementFactory GetFactory(Brush back)
        {
            back.Opacity = 0.6;
            var fef = new FrameworkElementFactory(typeof(Rectangle));
            fef.SetValue(Ellipse.FillProperty, back);
            fef.SetValue(Ellipse.StrokeProperty, Brushes.White);
            fef.SetValue(Ellipse.StrokeThicknessProperty, (double)1);
            return fef;
        }

        protected override Visual GetVisualChild(int index)
        {
            return visCollec[index];
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return visCollec.Count;
            }
        }

    }
}
