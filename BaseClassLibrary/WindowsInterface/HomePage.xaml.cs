using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BaseClassLibrary;
using Windows.UI.Xaml.Shapes;

namespace WindowsInterface
{

    public sealed partial class HomePage : Page{

        List<Ellipse> currentPointEllipses;
        List<Windows.UI.Xaml.Shapes.Path> bezierPathList;
        Ellipse currentControlPointTwoEllipse;
        Ellipse currentControlPointThreeEllipse;
        
        public HomePage(){
            this.InitializeComponent();
            currentPointEllipses = new List<Ellipse>();
            bezierPathList = new List<Windows.UI.Xaml.Shapes.Path>();
            if (App.currentPath == null) {
                App.currentPath = new Path();
            }
            else {
                importPath();
            }
            
        }
       
        private void savePointBtn_Click(object sender, RoutedEventArgs e) {
            double[] newpt = new double[] { Double.Parse(xPointInputTextBox.Text), Double.Parse(yPointInputTextBox.Text) };
            App.currentPath.addPoint(newpt);
            importPath();
        }

        private void pointsTableSelectionChanged(object sender, SelectionChangedEventArgs e) {
            ListBox senderListBox = (ListBox)sender;
            if(senderListBox == xValuesListBox) {
                yValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
                pointNumberListBox.SelectedIndex = senderListBox.SelectedIndex;
            }
            else if(senderListBox == yValuesListBox) {
                xValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
                pointNumberListBox.SelectedIndex = senderListBox.SelectedIndex;
            }
            else {
                xValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
                yValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
            }
        }

        private void importPath() {
            xValuesListBox.Items.Clear();
            yValuesListBox.Items.Clear();
            pointNumberListBox.Items.Clear();
            SegmentListComboBox.Items.Clear();
            for (int i = 0; i < currentPointEllipses.Count; i++) {
                FieldCanvas.Children.Remove(currentPointEllipses[i]);
            }

            currentPointEllipses.Clear();
            
            if (App.currentPath.getPoints() == null) return;
            for (int i = 0; i < App.currentPath.getPoints().Length; i++) {
                ListBoxItem xValueListBoxItem = new ListBoxItem();
                ListBoxItem yValueListBoxItem = new ListBoxItem();
                ListBoxItem numberListBoxItem = new ListBoxItem();
                xValueListBoxItem.Content = App.currentPath.getPoints()[i][0].ToString();
                yValueListBoxItem.Content = App.currentPath.getPoints()[i][1].ToString();
                numberListBoxItem.Content = pointNumberListBox.Items.Count + 1;
                xValueListBoxItem.FontSize = 30;
                yValueListBoxItem.FontSize = 30;
                numberListBoxItem.FontSize = 30;
                xValuesListBox.Items.Add(xValueListBoxItem);
                yValuesListBox.Items.Add(yValueListBoxItem);
                pointNumberListBox.Items.Add(numberListBoxItem);

                Ellipse dataPt = new Ellipse();
                dataPt.Width = 6;
                dataPt.Height = 6;
                dataPt.Fill = new SolidColorBrush(Windows.UI.Colors.Black);
                dataPt.Name = pointNumberListBox.Items.Count.ToString();
                dataPt.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                dataPt.ManipulationDelta += pointManipulationDelta;
                Canvas.SetTop(dataPt, FieldCanvas.Height - (App.currentPath.getPoints()[i][1]+3));
                Canvas.SetLeft(dataPt, App.currentPath.getPoints()[i][0]-3);
                Canvas.SetZIndex(dataPt, 1000);
                FieldCanvas.Children.Add(dataPt);
                currentPointEllipses.Add(dataPt);
            }

            for (int i = 0; i < App.currentPath.getPathList().Count; i++) {
                ComboBoxItem comboBoxItemSeg = new ComboBoxItem();
                comboBoxItemSeg.Content = (i + 1).ToString();
                comboBoxItemSeg.Name = i.ToString();
                comboBoxItemSeg.FontSize = 25;
                SegmentListComboBox.Items.Add(comboBoxItemSeg);
            }
            displayPath();
        }

        private void displayPath() {
            for (int i = 0; i < bezierPathList.Count; i++) {
                FieldCanvas.Children.Remove(bezierPathList[i]);
            }
            bezierPathList.Clear();
            for (int i = 0; i < App.currentPath.getPathList().Count; i++) {
                Windows.UI.Xaml.Shapes.Path segmentBezierPath = new Windows.UI.Xaml.Shapes.Path();
                PathGeometry geometry = new PathGeometry();
                PathFigure figure = new PathFigure();
                Point controlPtOnePoint = new Point(App.currentPath.getPathList()[i].getControlptOne()[0], FieldCanvas.Height - App.currentPath.getPathList()[i].getControlptOne()[1]);
                figure.StartPoint = controlPtOnePoint;
                BezierSegment bezierSegment = new BezierSegment();
                bezierSegment.Point1 = new Point(App.currentPath.getPathList()[i].getControlptTwo()[0], FieldCanvas.Height - App.currentPath.getPathList()[i].getControlptTwo()[1]);
                bezierSegment.Point2 = new Point(App.currentPath.getPathList()[i].getControlptThree()[0], FieldCanvas.Height- App.currentPath.getPathList()[i].getControlptThree()[1]);
                bezierSegment.Point3 = new Point(App.currentPath.getPathList()[i].getControlptFour()[0], FieldCanvas.Height - App.currentPath.getPathList()[i].getControlptFour()[1]);
                figure.Segments.Add(bezierSegment);
                geometry.Figures.Add(figure);
                segmentBezierPath.Data = geometry;
                segmentBezierPath.Stroke = new SolidColorBrush(Windows.UI.Colors.Purple);
                segmentBezierPath.StrokeThickness = 3;
                FieldCanvas.Children.Add(segmentBezierPath);
                bezierPathList.Add(segmentBezierPath);
            }
        }

        private void displayPath(Segment selectedSegment) {
            for (int i = 0; i < bezierPathList.Count; i++) {
                FieldCanvas.Children.Remove(bezierPathList[i]);
            }
            for (int i = 0; i < App.currentPath.getPathList().Count; i++) {
                Windows.UI.Xaml.Shapes.Path segmentBezierPath = new Windows.UI.Xaml.Shapes.Path();
                PathGeometry geometry = new PathGeometry();
                PathFigure figure = new PathFigure();
                Point controlPtOnePoint = new Point(App.currentPath.getPathList()[i].getControlptOne()[0], FieldCanvas.Height - App.currentPath.getPathList()[i].getControlptOne()[1]);
                figure.StartPoint = controlPtOnePoint;
                BezierSegment bezierSegment = new BezierSegment();
                bezierSegment.Point1 = new Point(App.currentPath.getPathList()[i].getControlptTwo()[0], FieldCanvas.Height - App.currentPath.getPathList()[i].getControlptTwo()[1]);
                bezierSegment.Point2 = new Point(App.currentPath.getPathList()[i].getControlptThree()[0], FieldCanvas.Height - App.currentPath.getPathList()[i].getControlptThree()[1]);
                bezierSegment.Point3 = new Point(App.currentPath.getPathList()[i].getControlptFour()[0], FieldCanvas.Height - App.currentPath.getPathList()[i].getControlptFour()[1]);
                figure.Segments.Add(bezierSegment);
                geometry.Figures.Add(figure);
                segmentBezierPath.Data = geometry;
                if (selectedSegment == App.currentPath.getPathList()[i]) {
                    segmentBezierPath.Stroke = new SolidColorBrush(Windows.UI.Colors.Green);
                }
                else segmentBezierPath.Stroke = new SolidColorBrush(Windows.UI.Colors.Purple);
                segmentBezierPath.StrokeThickness = 3;
                FieldCanvas.Children.Add(segmentBezierPath);
                bezierPathList.Add(segmentBezierPath);
            }
        }
        

        private void pointManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e) {
            Ellipse sentEllipse = sender as Ellipse;
            int pointTableNum = Int32.Parse(sentEllipse.Name);
            App.currentPath.modifyPoint(pointTableNum - 1, new double[] { App.currentPath.getPoint(pointTableNum - 1)[0] + e.Delta.Translation.X, App.currentPath.getPoint(pointTableNum - 1)[1] - e.Delta.Translation.Y });
            Canvas.SetLeft(sentEllipse, App.currentPath.getPoint(pointTableNum-1)[0]-3);
            Canvas.SetTop(sentEllipse, FieldCanvas.Height-(App.currentPath.getPoint(pointTableNum - 1)[1]+3));

            
            ListBoxItem xValueListBoxItem = (ListBoxItem)xValuesListBox.Items[pointTableNum - 1];
            ListBoxItem yValueListBoxItem = (ListBoxItem)yValuesListBox.Items[pointTableNum - 1];

            xValueListBoxItem.Content = Canvas.GetLeft(sentEllipse);
            yValueListBoxItem.Content = FieldCanvas.Height - Canvas.GetTop(sentEllipse);
            
            if(SegmentListComboBox.SelectedItem != null) {
                displayPath(App.currentPath.getPathList()[Int32.Parse(((ComboBoxItem)SegmentListComboBox.SelectedItem).Name)]);
            }
            else
            displayPath();

        }

        private void controlPointTwoManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e) {
            Segment selectedSegment = App.currentPath.getPathList()[Int32.Parse(((ComboBoxItem)SegmentListComboBox.SelectedItem).Name)];
            Ellipse sentControlPoint = sender as Ellipse;
            double[] newControlPointTwo = new double[] { selectedSegment.getControlptTwo()[0] + e.Delta.Translation.X, selectedSegment.getControlptTwo()[1] - e.Delta.Translation.Y };
            selectedSegment.setControlptTwo(newControlPointTwo);
            App.currentPath.standardizePath();
            Canvas.SetTop(sentControlPoint,FieldCanvas.Height - (selectedSegment.getControlptTwo()[1]+3));
            Canvas.SetLeft(sentControlPoint, selectedSegment.getControlptTwo()[0]-3);
            ControlPoint2Textblock.Text = "(" + selectedSegment.getControlptTwo()[0] + "," + selectedSegment.getControlptTwo()[1] + ")";

            displayPath(selectedSegment);
        }

        private void controlPointThreeManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e) {
            Segment selectedSegment = App.currentPath.getPathList()[Int32.Parse(((ComboBoxItem)SegmentListComboBox.SelectedItem).Name)];
            Ellipse sentControlPoint = sender as Ellipse;
            double[] newControlPointThree = new double[] { selectedSegment.getControlptThree()[0] + e.Delta.Translation.X, selectedSegment.getControlptThree()[1] - e.Delta.Translation.Y };
            selectedSegment.setControlptThree(newControlPointThree);
            App.currentPath.standardizePath();
            Canvas.SetTop(sentControlPoint, FieldCanvas.Height - (selectedSegment.getControlptThree()[1]+3));
            Canvas.SetLeft(sentControlPoint, selectedSegment.getControlptThree()[0]-3);
            ControlPoint3Textblock.Text = "(" + selectedSegment.getControlptThree()[0] + "," + selectedSegment.getControlptThree()[1] + ")";
            displayPath(selectedSegment);
        }

        private void SegmentListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            FieldCanvas.Children.Remove(currentControlPointTwoEllipse);
            FieldCanvas.Children.Remove(currentControlPointThreeEllipse);

            ComboBox comboBoxSender = sender as ComboBox;
            if (comboBoxSender.SelectedItem == null) return;
            Segment selectedSegment = App.currentPath.getPathList()[Int32.Parse(((ComboBoxItem)comboBoxSender.SelectedItem).Name)];
            ControlPoint2Textblock.Text = "("+ selectedSegment.getControlptTwo()[0] +"," + selectedSegment.getControlptTwo()[1] +")";
            ControlPoint3Textblock.Text = "(" + selectedSegment.getControlptThree()[0] + "," + selectedSegment.getControlptThree()[1] + ")";

            Ellipse controlPointTwoEllipse = new Ellipse();
            controlPointTwoEllipse.Width = 6;
            controlPointTwoEllipse.Height = 6;
            controlPointTwoEllipse.Fill = new SolidColorBrush(Windows.UI.Colors.Red);
            controlPointTwoEllipse.Name = "CurrentControlPointTwo";
            controlPointTwoEllipse.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            controlPointTwoEllipse.ManipulationDelta += controlPointTwoManipulationDelta;
            Canvas.SetTop(controlPointTwoEllipse, FieldCanvas.Height - (selectedSegment.getControlptTwo()[1]+3));
            Canvas.SetLeft(controlPointTwoEllipse, selectedSegment.getControlptTwo()[0]-3);
            Canvas.SetZIndex(controlPointTwoEllipse, 1001);
            FieldCanvas.Children.Add(controlPointTwoEllipse);
            currentControlPointTwoEllipse = controlPointTwoEllipse;

            Ellipse controlPointThreeEllipse = new Ellipse();
            controlPointThreeEllipse.Width = 5;
            controlPointThreeEllipse.Height = 5;
            controlPointThreeEllipse.Fill = new SolidColorBrush(Windows.UI.Colors.Yellow);
            controlPointThreeEllipse.Name = "CurrentControlPointThree";
            controlPointThreeEllipse.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            controlPointThreeEllipse.ManipulationDelta += controlPointThreeManipulationDelta;
            Canvas.SetTop(controlPointThreeEllipse, FieldCanvas.Height - selectedSegment.getControlptThree()[1]);
            Canvas.SetLeft(controlPointThreeEllipse, selectedSegment.getControlptThree()[0]);
            Canvas.SetZIndex(controlPointThreeEllipse, 1001);
            FieldCanvas.Children.Add(controlPointThreeEllipse);
            currentControlPointThreeEllipse = controlPointThreeEllipse;

            displayPath(selectedSegment);
        }
    }
}
