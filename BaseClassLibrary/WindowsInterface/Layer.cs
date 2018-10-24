using BaseClassLibrary;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WindowsInterface
{
    class Layer
    {
        private const int POINTSIZE = 4;

        private MotionProfile profile;
        public int SelectedSegmentIndex;
        public Canvas MainCanvas { get; private set; }

        private Layer(MotionProfile profile)
        {
            this.profile = profile;
        }

        private void CompileCanvas()
        {
            Canvas newCanvas = new Canvas();
            newCanvas.Height = App.FieldHeight;
            newCanvas.Width = App.FieldWidth;
            Ellipse[] ellipses = GetUIEllipseObjects();
            Windows.UI.Xaml.Shapes.Path[] paths = GetUIPathObjects();
            foreach (Ellipse ellipse in ellipses) newCanvas.Children.Add(ellipse);
            foreach (Windows.UI.Xaml.Shapes.Path path in paths) newCanvas.Children.Add(path);
            MainCanvas = newCanvas;
        }

        private Ellipse[] GetUIEllipseObjects()
        {
            Ellipse[] EllipseOutputs = new Ellipse[profile.Path.GetPoints().Length+2];
            for (int i = 0; i < profile.Path.GetPoints().Length; i++)
            {
                Ellipse dataPt = new Ellipse
                {
                    Width = 6,
                    Height = 6,
                    Fill = new SolidColorBrush(Windows.UI.Colors.Black),
                    Name = i.ToString(),
                    ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
                };
                dataPt.ManipulationDelta += PointManipulationDelta;
                Canvas.SetTop(dataPt, MainCanvas.Height - (profile.Path.GetPoints()[i][1] + 3));
                Canvas.SetLeft(dataPt, profile.Path.GetPoints()[i][0] - 3);
                Canvas.SetZIndex(dataPt, 1000);
                EllipseOutputs[i] = dataPt;
            }

            Ellipse cpTwo = new Ellipse
            {
                Width = 6,
                Height = 6,
                Fill = new SolidColorBrush(Windows.UI.Colors.Green),
                ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
            };
            cpTwo.ManipulationDelta += ControlPointTwoManipulationDelta;
            Canvas.SetTop(cpTwo, MainCanvas.Height - (profile.Path.PathList[SelectedSegmentIndex].ControlptTwo[1] + 3));
            Canvas.SetLeft(cpTwo, profile.Path.PathList[SelectedSegmentIndex].ControlptTwo[0] - 3);
            Canvas.SetZIndex(cpTwo, 1000);

            Ellipse cpThree = new Ellipse
            {
                Width = 6,
                Height = 6,
                Fill = new SolidColorBrush(Windows.UI.Colors.Purple),
                ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
            };
            cpThree.ManipulationDelta += ControlPointTwoManipulationDelta;
            Canvas.SetTop(cpThree, MainCanvas.Height - (profile.Path.PathList[SelectedSegmentIndex].ControlptThree[1] + 3));
            Canvas.SetLeft(cpThree, profile.Path.PathList[SelectedSegmentIndex].ControlptThree[0] - 3);
            Canvas.SetZIndex(cpThree, 1000);

            EllipseOutputs[profile.Path.GetPoints().Length] = cpTwo;
            EllipseOutputs[profile.Path.GetPoints().Length + 1] = cpThree;
            
            return EllipseOutputs;

        }
        private Windows.UI.Xaml.Shapes.Path[] GetUIPathObjects()
        {
            Windows.UI.Xaml.Shapes.Path[] PathOutputs = new Windows.UI.Xaml.Shapes.Path[profile.Path.PathList.Count];
            for (int i = 0; i < profile.Path.PathList.Count; i++)
            {
                Windows.UI.Xaml.Shapes.Path segmentBezierPath = new Windows.UI.Xaml.Shapes.Path();
                PathGeometry geometry = new PathGeometry();
                PathFigure figure = new PathFigure();
                Point controlPtOnePoint = new Point(profile.Path.PathList[i].ControlptOne[0], MainCanvas.Height - profile.Path.PathList[i].ControlptOne[1]);
                figure.StartPoint = controlPtOnePoint;
                BezierSegment bezierSegment = new BezierSegment
                {
                    Point1 = new Point(profile.Path.PathList[i].ControlptTwo[0], MainCanvas.Height - profile.Path.PathList[i].ControlptTwo[1]),
                    Point2 = new Point(profile.Path.PathList[i].ControlptThree[0], MainCanvas.Height - profile.Path.PathList[i].ControlptThree[1]),
                    Point3 = new Point(profile.Path.PathList[i].ControlptFour[0], MainCanvas.Height - profile.Path.PathList[i].ControlptFour[1])
                };
                figure.Segments.Add(bezierSegment);
                geometry.Figures.Add(figure);
                segmentBezierPath.Data = geometry;
                segmentBezierPath.Stroke = new SolidColorBrush(Windows.UI.Colors.Purple);
                segmentBezierPath.StrokeThickness = (profile.Robot.Width+ profile.Robot.BumperThickness * 2);
                segmentBezierPath.Opacity = 0.5;
                segmentBezierPath.StrokeEndLineCap = PenLineCap.Square;
                segmentBezierPath.StrokeStartLineCap = PenLineCap.Square;
                PathOutputs[i] = segmentBezierPath;
            }
            return PathOutputs;
        }

        private void PointManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Ellipse sentEllipse = sender as Ellipse;
            int pointTableNum = int.Parse(sentEllipse.Name);
            profile.Path.ModifyPoint(pointTableNum, new double[] { profile.Path.GetPoint(pointTableNum)[0] + e.Delta.Translation.X, profile.Path.GetPoint(pointTableNum)[1] - e.Delta.Translation.Y });
            CompileCanvas();
        }
        private void ControlPointTwoManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Segment selectedSegment = App.currentPath.PathList[SelectedSegmentIndex];
            double[] newControlPointTwo = new double[] { selectedSegment.ControlptTwo[0] + e.Delta.Translation.X, selectedSegment.ControlptTwo[1] - e.Delta.Translation.Y };
            selectedSegment.ControlptTwo = newControlPointTwo;
            App.currentPath.StandardizePath(SelectedSegmentIndex - 1);
            CompileCanvas();
        }
        private void ControlPointThreeManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Segment selectedSegment = App.currentPath.PathList[SelectedSegmentIndex];
            double[] newControlPointThree = new double[] { selectedSegment.ControlptThree[0] + e.Delta.Translation.X, selectedSegment.ControlptThree[1] - e.Delta.Translation.Y };
            selectedSegment.ControlptThree = newControlPointThree;
            profile.Path.StandardizePath(SelectedSegmentIndex);
            CompileCanvas();
        }
    }
}
