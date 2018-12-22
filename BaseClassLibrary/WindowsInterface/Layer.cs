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
        public MotionProfile Profile;
        public int SelectedSegmentIndex =0;
        public Canvas MainCanvas { get; private set; }

        public Layer(MotionProfile profile, double width, double height)
        {
            this.Profile = profile;
            MainCanvas = new Canvas();
            MainCanvas.Width = App.FieldCanvasWidth;
            MainCanvas.Height = App.FieldCanvasHeight;
            CompileCanvas();
        }

        public Layer(MotionProfile profile, double width, double height, String name)
        {
            this.Profile = profile;
            MainCanvas = new Canvas();
            MainCanvas.Width = App.FieldCanvasWidth;
            MainCanvas.Height = App.FieldCanvasHeight;
            MainCanvas.Name = name;
            CompileCanvas();
        }

        public void ResetCanvas()
        {
            MainCanvas = new Canvas();
            MainCanvas.Width = App.FieldCanvasWidth;
            MainCanvas.Height = App.FieldCanvasHeight;
        }

        //modifies the canvas 
        public void RefreshCanvas()
        {
            
            Profile.CalcProfile();
            if (Profile.Path.PathList.Count == 0) return;
            RefreshEllipses();
            RefreshPaths();
        }

        public Path GetPath()
        {
            return Profile.Path;
        }
        public void CompileCanvasNoEllipse()
        {
            MainCanvas.Children.Clear();
            if (Profile.Path.PathList.Count == 0) return;
            foreach (Windows.UI.Xaml.Shapes.Path path in GetUIPathObjects()) MainCanvas.Children.Add(path);
            RefreshCanvas();
        }
        public void CompileCanvas()
        {
            MainCanvas.Children.Clear();
            if (Profile.Path.PathList.Count == 0) return;
            foreach (Ellipse ellipse in GetUIEllipseObjects()) MainCanvas.Children.Add(ellipse);
            foreach (Windows.UI.Xaml.Shapes.Path path in GetUIPathObjects()) MainCanvas.Children.Add(path);
            RefreshCanvas();
        }
        public void AddPoint(double[] point)
        {
            Profile.Path.AddPoint(point);
            MainCanvas.Children.Clear();
            if (Profile.Path.PathList.Count == 0) return;
            foreach (Ellipse ellipse in GetUIEllipseObjects()) MainCanvas.Children.Add(ellipse);
            foreach (Windows.UI.Xaml.Shapes.Path path in GetUIPathObjects()) MainCanvas.Children.Add(path);
            RefreshCanvas();
        }
        
        //Gets and sets startpoints/endpoints as to prevent access to the profile
        private void SetStartPoint(double[] startPoint)
        {
            Profile.Path.ModifyPoint(0, startPoint);
        }
        private double[] GetStartPoint()
        {
            return Profile.Path.GetPoint(0);
        }
        private void SetEndPoint(double[] endPoint)
        {
            Profile.Path.ModifyPoint(Profile.Path.GetPoints().Length-1, endPoint);
        }
        public double[] GetEndPoint()
        {
            return Profile.Path.GetPoint(Profile.Path.GetPoints().Length - 1);
        }

        //Returns an array of UI ellipse objects to be displayed 
        private Ellipse[] GetUIEllipseObjects()
        {
            Ellipse[] EllipseOutputs = new Ellipse[Profile.Path.GetPoints().Length+2];
            for (int i = 0; i < Profile.Path.GetPoints().Length; i++)
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
                Canvas.SetTop(dataPt, MainCanvas.Height - (Profile.Path.GetPoints()[i][1] + 3));
                Canvas.SetLeft(dataPt, Profile.Path.GetPoints()[i][0] - 3);
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
                Canvas.SetTop(cpTwo, MainCanvas.Height - (Profile.Path.PathList[SelectedSegmentIndex].ControlptTwo[1] + 3));
                Canvas.SetLeft(cpTwo, Profile.Path.PathList[SelectedSegmentIndex].ControlptTwo[0] - 3);
                Canvas.SetZIndex(cpTwo, 1000);

                Ellipse cpThree = new Ellipse
                {
                    Width = 6,
                    Height = 6,
                    Fill = new SolidColorBrush(Windows.UI.Colors.Purple),
                    ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
                };
                cpThree.ManipulationDelta += ControlPointThreeManipulationDelta;
                Canvas.SetTop(cpThree, MainCanvas.Height - (Profile.Path.PathList[SelectedSegmentIndex].ControlptThree[1] + 3));
                Canvas.SetLeft(cpThree, Profile.Path.PathList[SelectedSegmentIndex].ControlptThree[0] - 3);
                Canvas.SetZIndex(cpThree, 1000);

                EllipseOutputs[Profile.Path.GetPoints().Length] = cpTwo;
                EllipseOutputs[Profile.Path.GetPoints().Length + 1] = cpThree;
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
                Canvas.SetTop(cpTwo, MainCanvas.Height - (Profile.Path.PathList[0].ControlptTwo[1] + 3));
                Canvas.SetLeft(cpTwo, Profile.Path.PathList[0].ControlptTwo[0] - 3);
                Canvas.SetZIndex(cpTwo, 1000);

                Ellipse cpThree = new Ellipse
                {
                    Width = 6,
                    Height = 6,
                    Fill = new SolidColorBrush(Windows.UI.Colors.Purple),
                    ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
                };
                cpThree.ManipulationDelta += ControlPointThreeManipulationDelta;
                Canvas.SetTop(cpThree, MainCanvas.Height - (Profile.Path.PathList[0].ControlptThree[1] + 3));
                Canvas.SetLeft(cpThree, Profile.Path.PathList[0].ControlptThree[0] - 3);
                Canvas.SetZIndex(cpThree, 1000);

                EllipseOutputs[Profile.Path.GetPoints().Length] = cpTwo;
                EllipseOutputs[Profile.Path.GetPoints().Length + 1] = cpThree;
            }
            ellipses = EllipseOutputs;
            return EllipseOutputs;

        }

        //Returns an array of UI path objects that can be displayed
        private Windows.UI.Xaml.Shapes.Path[] GetUIPathObjects()
        {
            Windows.UI.Xaml.Shapes.Path[] PathOutputs = new Windows.UI.Xaml.Shapes.Path[Profile.Path.PathList.Count];
            for (int i = 0; i < Profile.Path.PathList.Count; i++)
            {
                Windows.UI.Xaml.Shapes.Path segmentBezierPath = new Windows.UI.Xaml.Shapes.Path();
                PathGeometry geometry = new PathGeometry();
                PathFigure figure = new PathFigure();
                Point controlPtOnePoint = new Point(Profile.Path.PathList[i].ControlptOne[0], MainCanvas.Height - Profile.Path.PathList[i].ControlptOne[1]);
                figure.StartPoint = controlPtOnePoint;
                BezierSegment bezierSegment = new BezierSegment
                {
                    Point1 = new Point(Profile.Path.PathList[i].ControlptTwo[0], MainCanvas.Height - Profile.Path.PathList[i].ControlptTwo[1]),
                    Point2 = new Point(Profile.Path.PathList[i].ControlptThree[0], MainCanvas.Height - Profile.Path.PathList[i].ControlptThree[1]),
                    Point3 = new Point(Profile.Path.PathList[i].ControlptFour[0], MainCanvas.Height - Profile.Path.PathList[i].ControlptFour[1])
                };
                figure.Segments.Add(bezierSegment);
                geometry.Figures.Add(figure);
                segmentBezierPath.Data = geometry;
                segmentBezierPath.Stroke = new SolidColorBrush(Windows.UI.Colors.Purple);
                segmentBezierPath.StrokeThickness = (Profile.Robot.Width+ Profile.Robot.BumperThickness * 2);
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
            Profile.Path.ModifyPoint(pointTableNum, new double[] { Profile.Path.GetPoint(pointTableNum)[0] + e.Delta.Translation.X, Profile.Path.GetPoint(pointTableNum)[1] - e.Delta.Translation.Y });
            RefreshCanvas();
        }
        private void ControlPointTwoManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Segment selectedSegment = Profile.Path.PathList[SelectedSegmentIndex];
            double[] newControlPointTwo = new double[] { selectedSegment.ControlptTwo[0] + e.Delta.Translation.X, selectedSegment.ControlptTwo[1] - e.Delta.Translation.Y };
            selectedSegment.ControlptTwo = newControlPointTwo;
            Profile.Path.StandardizePath(SelectedSegmentIndex - 1);
            RefreshCanvas();
        }
        private void ControlPointThreeManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Segment selectedSegment = Profile.Path.PathList[SelectedSegmentIndex];
            double[] newControlPointThree = new double[] { selectedSegment.ControlptThree[0] + e.Delta.Translation.X, selectedSegment.ControlptThree[1] - e.Delta.Translation.Y };
            selectedSegment.ControlptThree = newControlPointThree;
            Profile.Path.StandardizePath(SelectedSegmentIndex);
            RefreshCanvas();
        }

        private void RefreshEllipses()
        {
            if(ellipses == null) return;
            for (int i = 0; i < Profile.Path.GetPoints().Length; i++)
            {
                Ellipse dataPt = ellipses[i];
                Canvas.SetTop(dataPt, MainCanvas.Height - (Profile.Path.GetPoints()[i][1] + 3));
                Canvas.SetLeft(dataPt, Profile.Path.GetPoints()[i][0] - 3);
                Canvas.SetZIndex(dataPt, 1000);
                
            }
            if (SelectedSegmentIndex == -1) return;
            Ellipse cpTwo = ellipses[Profile.Path.GetPoints().Length];
            Canvas.SetTop(cpTwo, MainCanvas.Height - (Profile.Path.PathList[SelectedSegmentIndex].ControlptTwo[1] + 3));
            Canvas.SetLeft(cpTwo, Profile.Path.PathList[SelectedSegmentIndex].ControlptTwo[0] - 3);
            Canvas.SetZIndex(cpTwo, 1000);

            Ellipse cpThree = ellipses[Profile.Path.GetPoints().Length+1];
            Canvas.SetTop(cpThree, MainCanvas.Height - (Profile.Path.PathList[SelectedSegmentIndex].ControlptThree[1] + 3));
            Canvas.SetLeft(cpThree, Profile.Path.PathList[SelectedSegmentIndex].ControlptThree[0] - 3);
            Canvas.SetZIndex(cpThree, 1000);

        }
        private void RefreshPaths()
        {
            for (int i = 0; i < Profile.Path.PathList.Count; i++)
            {
                Windows.UI.Xaml.Shapes.Path segmentBezierPath = paths[i];
                PathGeometry geometry = new PathGeometry();
                PathFigure figure = new PathFigure();
                Point controlPtOnePoint = new Point(Profile.Path.PathList[i].ControlptOne[0], MainCanvas.Height - Profile.Path.PathList[i].ControlptOne[1]);
                figure.StartPoint = controlPtOnePoint;
                BezierSegment bezierSegment = new BezierSegment
                {
                    Point1 = new Point(Profile.Path.PathList[i].ControlptTwo[0], MainCanvas.Height - Profile.Path.PathList[i].ControlptTwo[1]),
                    Point2 = new Point(Profile.Path.PathList[i].ControlptThree[0], MainCanvas.Height - Profile.Path.PathList[i].ControlptThree[1]),
                    Point3 = new Point(Profile.Path.PathList[i].ControlptFour[0], MainCanvas.Height - Profile.Path.PathList[i].ControlptFour[1])
                };
                figure.Segments.Add(bezierSegment);
                geometry.Figures.Add(figure);
                segmentBezierPath.Data = geometry;
                segmentBezierPath.Stroke = new SolidColorBrush(Windows.UI.Colors.Purple);
                segmentBezierPath.StrokeThickness = (Profile.Robot.Width + Profile.Robot.BumperThickness * 2);
                segmentBezierPath.Opacity = 0.5;
                segmentBezierPath.StrokeEndLineCap = PenLineCap.Square;
                segmentBezierPath.StrokeStartLineCap = PenLineCap.Square;
            }
            
        }

    }
}
