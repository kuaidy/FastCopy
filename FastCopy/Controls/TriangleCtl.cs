using FastCopy.Common;
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

namespace FastCopy.Controls
{
    public class TriangleCtl : Button
    {
        public static readonly DependencyProperty DrawTypeProperty = DependencyProperty.Register("DrawType", typeof(TriangleType), typeof(TriangleCtl));
        /// <summary>
        /// 三角形类型
        /// </summary>
        public TriangleType DrawType
        {
            get
            {
                return (TriangleType)GetValue(DrawTypeProperty);
            }
            set
            {
                SetValue(DrawTypeProperty, value);
            }
        }
        private Color TriangleColor = Colors.Black;

        static TriangleCtl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TriangleCtl), new FrameworkPropertyMetadata(typeof(TriangleCtl)));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            switch (DrawType)
            {
                case TriangleType.Fill:
                    drawingContext.DrawGeometry(new LinearGradientBrush(TriangleColor, TriangleColor, 0), new Pen(new SolidColorBrush(TriangleColor), 1.000), DrawTriangle());
                    break;
                case TriangleType.NotFill:
                    drawingContext.DrawGeometry(new SolidColorBrush(Colors.Transparent), new Pen(new SolidColorBrush(TriangleColor), 1.000), DrawTriangle());
                    break;
                case TriangleType.None:
                    break;
            }
        }
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            TriangleColor = Colors.CadetBlue;
            this.InvalidateVisual();
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            TriangleColor = Colors.Black;
            this.InvalidateVisual();
        }
        protected override void OnClick()
        {
            base.OnClick();
            this.InvalidateVisual();
        }

        private StreamGeometry DrawTriangle()
        {
            switch (DrawType)
            {
                case TriangleType.Fill:
                    return DrawFillTriangle();
                case TriangleType.NotFill:
                    return DrawNotFillTriangle();
                case TriangleType.None:
                    return null;
                default: return null;
            }
        }
        private StreamGeometry DrawFillTriangle()
        {
            StreamGeometry streamGeometry = new StreamGeometry();
            var point0 = new Point(0, this.ActualHeight);
            var point1 = new Point(this.ActualWidth / 3 * 2, this.ActualHeight);
            var point2 = new Point(this.ActualWidth / 3 * 2, this.ActualHeight / 3);
            using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
            {
                streamGeometryContext.BeginFigure(point0, true, false);
                streamGeometryContext.LineTo(point1, true, true);
                streamGeometryContext.LineTo(point2, true, true);
                streamGeometryContext.LineTo(point0, true, true);
            }
            return streamGeometry;
        }
        private StreamGeometry DrawNotFillTriangle()
        {
            StreamGeometry streamGeometry = new StreamGeometry();
            var point0 = new Point(0, 0);
            var point1 = new Point(this.ActualWidth / 2, this.ActualHeight / 2);
            var point2 = new Point(0, this.ActualHeight);
            using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
            {
                streamGeometryContext.BeginFigure(point0, true, false);
                streamGeometryContext.LineTo(point1, true, true);
                streamGeometryContext.LineTo(point2, true, true);
                streamGeometryContext.LineTo(point0, true, true);
            }
            return streamGeometry;
        }
    }

}
