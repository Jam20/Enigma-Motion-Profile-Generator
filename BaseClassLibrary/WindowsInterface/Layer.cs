using System;
using System.Collections.Generic;
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
        private Ellipse[] ellipses;
        private Windows.UI.Xaml.Shapes.Path[] paths;
        public MotionProfile profile;
        public int SelectedSegmentIndex =0;
        public Canvas MainCanvas { get; private set; }

        public Layer(MotionProfile profile, double width, double height)
        {
            this.profile = profile;
            Canvas newCanvas = new Canvas();
            newCanvas.Width = width;
            newCanvas.Height = height;
            newCanvas.Name = "layer1";
            MainCanvas = newCanvas;
            RefreshCanvas();
        }

        public Layer(MotionProfile profile, double width, double height, String name)
        {
            this.profile = profile;
            Canvas newCanvas = new Canvas();
            newCanvas.Width = width;
            newCanvas.Height = height;
            newCanvas.Name = name;
            MainCanvas = newCanvas;
            CompileCanvas();
        }

        //modifies the canvas 
        public void RefreshCanvas()
        {
            
            profile.CalcProfile();
            if (profile.Path.PathList.Count == 0) return;
            RefreshEllipses();
            RefreshPaths();
        }

        public Path GetPath()
        {
            return profile.Path;
        }
        public void CompileCanvasNoEllipse()
        {
            MainCanvas.Children.Clear();
            if (profile.Path.PathList.Count == 0) return;
            foreach (Windows.UI.Xaml.Shapes.Path path in GetUIPathObjects()) MainCanvas.Children.Add(path);
            RefreshCanvas();
        }
        public void CompileCanvas()
        {
            MainCanvas.Children.Clear();
            if (profile.Path.PathList.Count == 0) return;
            foreach (Ellipse ellipse in GetUIEllipseObjects()) MainCanvas.Children.Add(ellipse);
            foreach (Windows.UI.Xaml.Shapes.Path path in GetUIPathObjects()) MainCanvas.Children.Add(path);
            RefreshCanvas();
        }
        public void AddPoint(double[] point)
        {
            profile.Path.AddPoint(point);
            MainCanvas.Children.Clear();
            if (profile.Path.PathList.Count == 0) return;
            foreach (Ellipse ellipse in GetUIEllipseObjects()) MainCanvas.Children.Add(ellipse);
            foreach (Windows.UI.Xaml.Shapes.Path path in GetUIPathObjects()) MainCanvas.Children.Add(path);
            RefreshCanvas();
        }
        
        //Gets and sets startpoints/endpoints as to prevent access to the profile
        private void SetStartPoint(double[] startPoint)
        {
            profile.Path.ModifyPoint(0, startPoint);
        }
        private double[] GetStartPoint()
        {
            return profile.Path.GetPoint(0);
        }
        private void SetEndPoint(double[] endPoint)
        {
            profile.Path.ModifyPoint(profile.Path.GetPoints().Length-1, endPoint);
        }
        public double[] GetEndPoint()
        {
            return profile.Path.GetPoint(profile.Path.GetPoints().Length - 1);
        }

        //Returns an array of UI ellipse objects to be displayed 
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
            if (SelectedSegmentIndex !=-1)
            {
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
                cpThree.ManipulationDelta += ControlPointThreeManipulationDelta;
                Canvas.SetTop(cpThree, MainCanvas.Height - (profile.Path.PathList[SelectedSegmentIndex].ControlptThree[1] + 3));
                Canvas.SetLeft(cpThree, profile.Path.PathList[SelectedSegmentIndex].ControlptThree[0] - 3);
                Canvas.SetZIndex(cpThree, 1000);

                EllipseOutputs[profile.Path.GetPoints().Length] = cpTwo;
                EllipseOutputs[profile.Path.GetPoints().Length + 1] = cpThree;
            }
            else
            {
                Ellipse cpTwo = new Ellipse
                {
                    Width = 6,
                    Height = 6,
                    Fill = new SolidColorBrush(Windows.UI.Colors.Green),
                    ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
                };
                cpTwo.ManipulationDelta += ControlPointTwoManipulationDelta;
                Canvas.SetTop(cpTwo, MainCanvas.Height - (profile.Path.PathList[0].ControlptTwo[1] + 3));
                Canvas.SetLeft(cpTwo, profile.Path.PathList[0].ControlptTwo[0] - 3);
                Canvas.SetZIndex(cpTwo, 1000);

                Ellipse cpThree = new Ellipse
                {
                    Width = 6,
                    Height = 6,
                    Fill = new SolidColorBrush(Windows.UI.Colors.Purple),
                    ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
                };
                cpThree.ManipulationDelta += ControlPointThreeManipulationDelta;
                Canvas.SetTop(cpThree, MainCanvas.Height - (profile.Path.PathList[0].ControlptThree[1] + 3));
                Canvas.SetLeft(cpThree, profile.Path.PathList[0].ControlptThree[0] - 3);
                Canvas.SetZIndex(cpThree, 1000);

                EllipseOutputs[profile.Path.GetPoints().Length] = cpTwo;
                EllipseOutputs[profile.Path.GetPoints().Length + 1] = cpThree;
            }
            ellipses = EllipseOutputs;
            return EllipseOutputs;

        }

        //Returns an array of UI path objects that can be displayed
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
            paths = PathOutputs;
            return PathOutputs;
        }

        //Code for when the points are moved around by dragging
        private void PointManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Ellipse sentEllipse = sender as Ellipse;
            int pointTableNum = int.Parse(sentEllipse.Name);
            profile.Path.ModifyPoint(pointTableNum, new double[] { profile.Path.GetPoint(pointTableNum)[0] + e.Delta.Translation.X, profile.Path.GetPoint(pointTableNum)[1] - e.Delta.Translation.Y });
            RefreshCanvas();
        }
        private void ControlPointTwoManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Segment selectedSegment = profile.Path.PathList[SelectedSegmentIndex];
            double[] newControlPointTwo = new double[] { selectedSegment.ControlptTwo[0] + e.Delta.Translation.X, selectedSegment.ControlptTwo[1] - e.Delta.Translation.Y };
            selectedSegment.ControlptTwo = newControlPointTwo;
            profile.Path.StandardizePath(SelectedSegmentIndex - 1);
            RefreshCanvas();
        }
        private void ControlPointThreeManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Segment selectedSegment = profile.Path.PathList[SelectedSegmentIndex];
            double[] newControlPointThree = new double[] { selectedSegment.ControlptThree[0] + e.Delta.Translation.X, selectedSegment.ControlptThree[1] - e.Delta.Translation.Y };
            selectedSegment.ControlptThree = newControlPointThree;
            profile.Path.StandardizePath(SelectedSegmentIndex);
            RefreshCanvas();
        }

        private void RefreshEllipses()
        {
            for (int i = 0; i < profile.Path.GetPoints().Length; i++)
            {
                Ellipse dataPt = ellipses[i];
                Canvas.SetTop(dataPt, MainCanvas.Height - (profile.Path.GetPoints()[i][1] + 3));
                Canvas.SetLeft(dataPt, profile.Path.GetPoints()[i][0] - 3);
                Canvas.SetZIndex(dataPt, 1000);
                
            }
            if (SelectedSegmentIndex == -1) return;
            Ellipse cpTwo = ellipses[profile.Path.GetPoints().Length];
            Canvas.SetTop(cpTwo, MainCanvas.Height - (profile.Path.PathList[SelectedSegmentIndex].ControlptTwo[1] + 3));
            Canvas.SetLeft(cpTwo, profile.Path.PathList[SelectedSegmentIndex].ControlptTwo[0] - 3);
            Canvas.SetZIndex(cpTwo, 1000);

            Ellipse cpThree = ellipses[profile.Path.GetPoints().Length+1];
            Canvas.SetTop(cpThree, MainCanvas.Height - (profile.Path.PathList[SelectedSegmentIndex].ControlptThree[1] + 3));
            Canvas.SetLeft(cpThree, profile.Path.PathList[SelectedSegmentIndex].ControlptThree[0] - 3);
            Canvas.SetZIndex(cpThree, 1000);

        }
        private void RefreshPaths()
        {
            for (int i = 0; i < profile.Path.PathList.Count; i++)
            {
                Windows.UI.Xaml.Shapes.Path segmentBezierPath = paths[i];
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
                segmentBezierPath.StrokeThickness = (profile.Robot.Width + profile.Robot.BumperThickness * 2);
                segmentBezierPath.Opacity = 0.5;
                segmentBezierPath.StrokeEndLineCap = PenLineCap.Square;
                segmentBezierPath.StrokeStartLineCap = PenLineCap.Square;
            }
            
        }

    }
}
