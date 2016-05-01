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
using Logic;
using Logic.Points;
using System.Threading;

namespace KG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double n = 40;
        Point4D t = new Point4D() { X = n, Y = n, Z = 2*n,  W = 1, Name = "T"},
            c = new Point4D() { X = 2*n, Y = 3*n, Z = 2*n, W = 0, Name = "C" };
        bool Rewrite = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void valueChanged(object sender, RoutedEventArgs e)
        {
            Input();
        }

        private void T_Checked(object sender, RoutedEventArgs e)
        {
            Rewrite = true;
            X.CurrentValue = t.X;
            _Y.CurrentValue = t.Y;
            _Z.CurrentValue = t.Z;
            Rewrite = false;

        }

        private void C_Checked(object sender, RoutedEventArgs e)
        {
            Rewrite = true;
            X.CurrentValue = c.X;
            _Y.CurrentValue = c.Y;
            _Z.CurrentValue = c.Z;
            Rewrite = false;
        }

        private void valueChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeMaxims();
            Input();
        }

        private void RefreshPointsLabels()
        {
            T.Content = "T" + string.Format("({0},{1},{2})", t.X, t.Y,t.Z);
            C.Content = "C" + string.Format("({0},{1},{2})", c.X, c.Y, c.Z);
        }

        private void ChangeMaxims()
        {
            var tmp = Math.Floor(PerspectivePlot.ActualWidth/4);
            X.Maximum = _Y.Maximum = _Z.Maximum = tmp;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            ChangeMaxims();
            Input();
        }

        private void Centr_Checked(object sender, RoutedEventArgs e)
        {
            c.W = 1;
            Input();
        }

        private void MultipleMaximun(double v)
        {
            X.CurrentValue *= v;
            _Y.CurrentValue *= v;
            _Z.CurrentValue *= v;
        }

        private void Centr_Unchecked(object sender, RoutedEventArgs e)
        {
            c.W = 0;
            Input();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void Input()
        {
            if (Window.IsInitialized && !Rewrite)
            {
                if (C.IsChecked == true)
                {
                    c.X = X.CurrentValue;
                    c.Y = _Y.CurrentValue;
                    c.Z = _Z.CurrentValue;

                }
                else
                {
                    t.X = X.CurrentValue;
                    t.Y = _Y.CurrentValue;
                    t.Z = _Z.CurrentValue;
                }
                var listPoints = new List<Point4D>() {
                        new Point4D() { Name = "O"},//O
                        new Point4D() {X= 0.5, Name = "X"},
                        new Point4D() {Z= 0.5, Name = "Z" },
                        new Point4D() {Y = 0.5, Name ="Y" },
                        t.Clone(),
                        new Point4D() {X= t.X, Y=t.Y, Name="T1" },
                        new Point4D() {X= t.X, Z=t.Z, Name="T2" },
                        new Point4D() {Z= t.Z, Y=t.Y, Name="T3" },
                        new Point4D() {X = t.X, Name = "Tx" },
                        new Point4D() {Y = t.Y, Name = "Ty" },
                        new Point4D() {Z = t.Z, Name = "Tz" },
                    };
                List<List<Point2D>> coordinatesComplex;

                try
                {
                    
                    List<Point2D> coordinates;
                    if (Centr.IsChecked == true)
                    {
                        coordinates = Logic.CreateMapPerspetiveGeometryPlot.CentralProjection(
                               PerspectivePlot.ActualHeight,
                               PerspectivePlot.ActualWidth,
                               c,
                               listPoints
                           );
                    }
                    else
                    {
                        coordinates = Logic.CreateMapPerspetiveGeometryPlot.OrtProjection(
                               PerspectivePlot.ActualHeight,
                               PerspectivePlot.ActualWidth,
                               c,
                               listPoints
                           );
                    }

                    PerspectivePlot.DrawPerspective(coordinates);


                }
                catch (Exception e)
                {
                    PerspectivePlot.Clear();
                    ComplexPlot.Clear();
                    PerspectivePlot.ShowMessage(e.Message);
                    GarbageColllect();
                }

                coordinatesComplex = CreateMapComplexPlot.getPointsOnWindow(
                           PerspectivePlot.ActualHeight,
                           PerspectivePlot.ActualWidth,
                           new List<Logic.Points._Point>() { c, t }
                        );


                ComplexPlot.DrawComplex(coordinatesComplex);
                RefreshPointsLabels();

                GarbageColllect();
            }
        }

        void GarbageColllect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            GC.Collect();
        }
    }
}
