using Logic.Points;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System;
using System.Linq;

namespace Plot
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Plot : UserControl
    {
        double labelHeight = 20;
        double labelWigth = 20;
        double r = 4;

        public Plot()
        {
            InitializeComponent();            
        }

        public void DrawPerspective(List<Point2D> points)
        {
            _Plot.Children.Clear();

            Polyline l = new Polyline() { StrokeThickness = 1, Stroke = Brushes.Black, StrokeLineJoin = PenLineJoin.Bevel, StrokeEndLineCap = PenLineCap.Triangle };
            
            List<System.Windows.Point> list;
            List<string> names = new List<string>()
            {
                "O","X","Tx","T1","Ty","Y","O","Z","Tz","T2", "T","T3","Tz","T2","T","T1","Tx","T2","T","T3","Ty"
            };

            list = GiveSequens(names, points);
            foreach(System.Windows.Point a in list)
            {
                l.Points.Add(a);
            }
            _Plot.Children.Add(l);

            foreach (Point2D a in points)
            {
                var b = new Label() { Content = a.Name };
                Canvas.SetLeft(b, a.X);
                Canvas.SetTop(b, a.Y);

                _Plot.Children.Add(b);


                if (a.Name != "X" && a.Name != "Y" && a.Name != "Z")
                {
                    var c = new Ellipse()
                    {
                        Fill = (a.Name != "O" ? Brushes.Blue : Brushes.BurlyWood),
                        Height = r,
                        Width = r
                    };
                    Canvas.SetLeft(c, a.X - c.Width / 2);
                    Canvas.SetTop(c, a.Y - c.Height / 2);

                    _Plot.Children.Add(c);
                }

            }

            
        }

        public void DrawComplex(List<List<Point2D>> points)
        {
            _Plot.Children.Clear();
            var axisPoints = points[0];
            points.Remove(axisPoints);
            DrawAxis(axisPoints);

            foreach (List<Point2D> point in points)
            {
                DrawPoint(point);
            }

        }

        private void DrawPoint(List<Point2D> point)
        {
            Random rand = new Random();
            SolidColorBrush s = new SolidColorBrush(Color.FromRgb((byte)rand.Next(150,200),
                                                                   (byte)rand.Next(150,220),
                                                                   (byte)rand.Next(100,150)));
            Polyline l = new Polyline() { StrokeThickness = 1.5, Stroke = s, StrokeLineJoin = PenLineJoin.Bevel, StrokeEndLineCap = PenLineCap.Triangle };
            var nP = point[0].Name[0];
            var seq = GiveSequens
            (
                new List<string>()
                {
                nP + "y2",
                nP + "1",
                nP + "x",
                nP + "2",
                nP + "z",
                nP + "3",
                nP + "y1",
                }, 
                point
            );

            foreach(Point a in seq)
            {
                l.Points.Add(a);
            }
            _Plot.Children.Add(l);

            CreateArc(point, s);
            DrawElipseAndLabel(point);

        }

        private void CreateArc(List<Point2D> point, SolidColorBrush s)
        {
            Path arc_path = new Path() { Stroke = s};
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            ArcSegment arcSegment = new ArcSegment();
            char nP = point[0].Name[0];
            Point2D y1 = point.First(x => x.Name == nP + "y1"), y2 = point.First(x => x.Name == nP + "y2");
            Point r1 = new Point(_Plot.ActualWidth / 2, _Plot.ActualHeight / 2),
                  r2 = new Point(y1.X, y1.Y);
            Vector r = r2 - r1;
                    
            
            pathFigure.StartPoint = new Point(y1.X, y1.Y);
            arcSegment.Point = new Point(y2.X, y2.Y);
            arcSegment.Size = new Size(r.Length, r.Length);
            arcSegment.SweepDirection = SweepDirection.Clockwise;

            pathFigure.Segments.Add(arcSegment);
            pathGeometry.Figures.Add(pathFigure);
            arc_path.Data = pathGeometry;

            _Plot.Children.Add(arc_path);
        }

        private void DrawElipseAndLabel(List<Point2D> point)
        {
            foreach(Point2D a in point)
            {
                var el = new Ellipse()
                {
                    Fill =  Brushes.Blue,
                    Height = r,
                    Width = r
                };

                var label = new Label()
                {
                    Content = a.Name
                };

                CanvasSet(label,a.X, a.Y);
                CanvasSet(el, a.X - el.Width / 2, a.Y - el.Height / 2);
                _Plot.Children.Add(label);
                _Plot.Children.Add(el);

            }
        }

        private void CanvasSet(UIElement c ,double x, double y)
        {
            Canvas.SetLeft(c, x);
            Canvas.SetTop(c, y);
        }

        private void DrawAxis(List<Point2D> axisPoints)
        {
            var seq = GiveSequens(new List<string>() { "X","-X","O","-Z","Z"}, axisPoints);

            var l = new Polyline() { StrokeThickness = 1, Stroke = Brushes.Black, StrokeLineJoin = PenLineJoin.Bevel, StrokeEndLineCap = PenLineCap.Triangle };
            foreach(Point a in seq)
            {
                l.Points.Add(a);
            }
            _Plot.Children.Add(l);

            foreach(Point2D a in axisPoints)
            {
                var labl1 = new Label() { Content = a.Name};
                var labl2 = new Label() { Content = a.Name.Any(x => x == '-') ? "Y" : "-Y"};

                switch (a.Name)
                {
                    case "X":
                        Canvas.SetLeft(labl1, a.X);
                        Canvas.SetTop(labl1, a.Y);

                        Canvas.SetLeft(labl2, a.X);
                        Canvas.SetTop(labl2, a.Y - labelHeight);
                        break;
                    case "-X":
                        Canvas.SetLeft(labl1, a.X - labelWigth);
                        Canvas.SetTop(labl1, a.Y);

                        Canvas.SetLeft(labl2, a.X - labelWigth);
                        Canvas.SetTop(labl2, a.Y - labelHeight);
                        break;
                    case "Z":
                        Canvas.SetLeft(labl1, a.X);
                        Canvas.SetTop(labl1, a.Y);

                        Canvas.SetLeft(labl2, a.X - labelWigth);
                        Canvas.SetTop(labl2, a.Y);
                        break;
                    case "-Z":
                        Canvas.SetLeft(labl1, a.X);
                        Canvas.SetTop(labl1, a.Y - labelHeight);

                        Canvas.SetLeft(labl2, a.X - labelWigth);
                        Canvas.SetTop(labl2, a.Y - labelHeight);
                        break;
                    case "O":
                        Canvas.SetLeft(labl1, a.X);
                        Canvas.SetTop(labl1, a.Y);

                        _Plot.Children.Add(labl1);
                        continue;
                }              

                _Plot.Children.Add(labl1);
                _Plot.Children.Add(labl2);
            }
        }

        private List<System.Windows.Point> GiveSequens(List<string> names, List<Point2D> points)
        {
            List<System.Windows.Point> list = new List<System.Windows.Point>();
        
            foreach (string a in names)
            {
                var tmp = points.First(x => string.Equals(x.Name, a));
                list.Add(new System.Windows.Point(tmp.X, tmp.Y));
            }
            return list;
        }

        public void Clear()
        {
            _Plot.Children.Clear();
        }

        public void ShowMessage(string message)
        {
            var b = new Label() { Content = message };
            Canvas.SetLeft(b, _Plot.ActualWidth/5);
            Canvas.SetTop(b, _Plot.ActualHeight/2);

            _Plot.Children.Add(b);
        }
    }
}
