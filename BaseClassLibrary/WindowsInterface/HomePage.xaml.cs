using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace WindowsInterface
{

    public sealed partial class HomePage : Page
    {

        List<Ellipse> currentPointEllipses;
        List<Windows.UI.Xaml.Shapes.Path> bezierPathList;
        Ellipse currentControlPointTwoEllipse;
        Ellipse currentControlPointThreeEllipse;
        private int pointTableSelectedIndex = 0;
        private Segment selectedSegment;

        //constructs the page and initalizes variables needed for proper function
        public HomePage()
        {
            this.InitializeComponent();

            currentPointEllipses = new List<Ellipse>();
            bezierPathList = new List<Windows.UI.Xaml.Shapes.Path>();
            if (App.currentPath == null)
            {
                App.currentPath = new Path();
            }
            else
            {
                //ImportPath();
            }

        }


        //edits a point in the pathlist to the contents of the text boxes above it
        /*  private void SavePointBtn_Click(object sender, RoutedEventArgs e)
          {
              //Added number check to prevent crashes from bad input
              if (double.TryParse(xPointInputTextBox.Text, out double x) && double.TryParse(yPointInputTextBox.Text, out double y) && pointNumberListBox.Items.Count > 0)
              {
                  App.currentPath.ModifyPoint(pointTableSelectedIndex, new double[] { x, y });
                  ImportPath();
              }
              else
              {
                  xPointInputTextBox.Text = "";
                  yPointInputTextBox.Text = "";
                  WarningCD warning = new WarningCD("Error: Bad Input", "This field may only contain numeric input.");
                  warning.Show();
              }

          }

          //selects the list box items that are in line with the one selected and readies the point to be modified
          private void PointsTableSelectionChanged(object sender, SelectionChangedEventArgs e)
          {
              ListBox senderListBox = (ListBox)sender;
              if (senderListBox == xValuesListBox)
              {
                  yValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
                  pointNumberListBox.SelectedIndex = senderListBox.SelectedIndex;
              }
              else if (senderListBox == yValuesListBox)
              {
                  xValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
                  pointNumberListBox.SelectedIndex = senderListBox.SelectedIndex;
              }
              else
              {
                  xValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
                  yValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
              }
              pointTableSelectedIndex = xValuesListBox.SelectedIndex;
              if (pointTableSelectedIndex == -1)
              {
                  xPointInputTextBox.Text = "";
                  yPointInputTextBox.Text = "";
                  return;
              }
              xPointInputTextBox.Text = "" + App.currentPath.GetPoint(pointTableSelectedIndex)[0];
              yPointInputTextBox.Text = "" + App.currentPath.GetPoint(pointTableSelectedIndex)[1];
          }

          //imports the path items and creates a list box item and an ellipse for each
          private void ImportPath()
          {
              xValuesListBox.Items.Clear();
              yValuesListBox.Items.Clear();
              pointNumberListBox.Items.Clear();
              SegmentListComboBox.Items.Clear();
              for (int i = 0; i < currentPointEllipses.Count; i++)
              {
                  FieldCanvas.Children.Remove(currentPointEllipses[i]);
              }

              currentPointEllipses.Clear();

              if (App.currentPath.GetPoints() == null) return;
              for (int i = 0; i < App.currentPath.GetPoints().Length; i++)
              {
                  ListBoxItem xValueListBoxItem = new ListBoxItem();
                  ListBoxItem yValueListBoxItem = new ListBoxItem();
                  ListBoxItem numberListBoxItem = new ListBoxItem();
                  xValueListBoxItem.Content = App.currentPath.GetPoints()[i][0].ToString();
                  yValueListBoxItem.Content = App.currentPath.GetPoints()[i][1].ToString();
                  numberListBoxItem.Content = pointNumberListBox.Items.Count + 1;
                  xValueListBoxItem.FontSize = 30;
                  yValueListBoxItem.FontSize = 30;
                  numberListBoxItem.FontSize = 30;
                  xValuesListBox.Items.Add(xValueListBoxItem);
                  yValuesListBox.Items.Add(yValueListBoxItem);
                  pointNumberListBox.Items.Add(numberListBoxItem);

                  Ellipse dataPt = new Ellipse
                  {
                      Width = 6,
                      Height = 6,
                      Fill = new SolidColorBrush(Windows.UI.Colors.Black),
                      Name = pointNumberListBox.Items.Count.ToString(),
                      ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
                  };
                  dataPt.ManipulationDelta += PointManipulationDelta;
                  Canvas.SetTop(dataPt, FieldCanvas.Height - (App.currentPath.GetPoints()[i][1] + 3));
                  Canvas.SetLeft(dataPt, App.currentPath.GetPoints()[i][0] - 3);
                  Canvas.SetZIndex(dataPt, 1000);
                  FieldCanvas.Children.Add(dataPt);
                  currentPointEllipses.Add(dataPt);
              }

              for (int i = 0; i < App.currentPath.PathList.Count; i++)
              {
                  ComboBoxItem comboBoxItemSeg = new ComboBoxItem
                  {
                      Content = (i + 1).ToString(),
                      Name = i.ToString(),
                      FontSize = 25
                  };
                  SegmentListComboBox.Items.Add(comboBoxItemSeg);
              }
              DisplayPath();
          }

          //displays the visual path on the screen by importing the geometry of each segment
          public void DisplayPath()
          {
              FieldCanvas.Children.Remove(currentControlPointTwoEllipse);
              FieldCanvas.Children.Remove(currentControlPointThreeEllipse);
              for (int i = 0; i < bezierPathList.Count; i++)
              {
                  FieldCanvas.Children.Remove(bezierPathList[i]);
              }
              bezierPathList.Clear();
              for (int i = 0; i < App.currentPath.PathList.Count; i++)
              {
                  Windows.UI.Xaml.Shapes.Path segmentBezierPath = new Windows.UI.Xaml.Shapes.Path();
                  PathGeometry geometry = new PathGeometry();
                  PathFigure figure = new PathFigure();
                  Point controlPtOnePoint = new Point(App.currentPath.PathList[i].ControlptOne[0], FieldCanvas.Height - App.currentPath.PathList[i].ControlptOne[1]);
                  figure.StartPoint = controlPtOnePoint;
                  BezierSegment bezierSegment = new BezierSegment
                  {
                      Point1 = new Point(App.currentPath.PathList[i].ControlptTwo[0], FieldCanvas.Height - App.currentPath.PathList[i].ControlptTwo[1]),
                      Point2 = new Point(App.currentPath.PathList[i].ControlptThree[0], FieldCanvas.Height - App.currentPath.PathList[i].ControlptThree[1]),
                      Point3 = new Point(App.currentPath.PathList[i].ControlptFour[0], FieldCanvas.Height - App.currentPath.PathList[i].ControlptFour[1])
                  };
                  figure.Segments.Add(bezierSegment);
                  geometry.Figures.Add(figure);
                  segmentBezierPath.Data = geometry;
                  segmentBezierPath.Stroke = new SolidColorBrush(Windows.UI.Colors.Purple);
                  segmentBezierPath.StrokeThickness = (App.currentRobot.Width + App.currentRobot.BumperThickness * 2);
                  segmentBezierPath.Opacity = 0.5;
                  segmentBezierPath.StrokeEndLineCap = PenLineCap.Square;
                  segmentBezierPath.StrokeStartLineCap = PenLineCap.Square;
                  //Make the segments selectable
                  segmentBezierPath.Tag = App.currentPath.PathList[i];
                  segmentBezierPath.Tapped += delegate (object sender, TappedRoutedEventArgs e)
                  {
                      selectedSegment = segmentBezierPath.Tag as Segment;
                      SetSelectedSegment(this.selectedSegment);
                      e.Handled = true;
                  };
                  FieldCanvas.Children.Add(segmentBezierPath);
                  bezierPathList.Add(segmentBezierPath);
              }
          }

          //overload with the difference of having the selected segment have a green coloring
          private void DisplayPath(Segment selectedSegment)
          {
              for (int i = 0; i < bezierPathList.Count; i++)
              {
                  FieldCanvas.Children.Remove(bezierPathList[i]);
              }
              for (int i = 0; i < App.currentPath.PathList.Count; i++)
              {
                  Windows.UI.Xaml.Shapes.Path segmentBezierPath = new Windows.UI.Xaml.Shapes.Path();
                  PathGeometry geometry = new PathGeometry();
                  PathFigure figure = new PathFigure();
                  Point controlPtOnePoint = new Point(App.currentPath.PathList[i].ControlptOne[0], FieldCanvas.Height - App.currentPath.PathList[i].ControlptOne[1]);
                  figure.StartPoint = controlPtOnePoint;
                  BezierSegment bezierSegment = new BezierSegment
                  {
                      Point1 = new Point(App.currentPath.PathList[i].ControlptTwo[0], FieldCanvas.Height - App.currentPath.PathList[i].ControlptTwo[1]),
                      Point2 = new Point(App.currentPath.PathList[i].ControlptThree[0], FieldCanvas.Height - App.currentPath.PathList[i].ControlptThree[1]),
                      Point3 = new Point(App.currentPath.PathList[i].ControlptFour[0], FieldCanvas.Height - App.currentPath.PathList[i].ControlptFour[1])
                  };
                  figure.Segments.Add(bezierSegment);
                  geometry.Figures.Add(figure);
                  segmentBezierPath.Data = geometry;
                  if (selectedSegment == App.currentPath.PathList[i])
                  {
                      segmentBezierPath.Stroke = new SolidColorBrush(Windows.UI.Colors.Green);
                  }
                  else segmentBezierPath.Stroke = new SolidColorBrush(Windows.UI.Colors.Purple);
                  segmentBezierPath.Opacity = 0.5;
                  segmentBezierPath.StrokeEndLineCap = PenLineCap.Square;
                  segmentBezierPath.StrokeStartLineCap = PenLineCap.Square;
                  segmentBezierPath.StrokeThickness = (App.currentRobot.Width + App.currentRobot.BumperThickness * 2);
                  segmentBezierPath.Tag = App.currentPath.PathList[i];
                  segmentBezierPath.Tapped += delegate (object sender, TappedRoutedEventArgs e)
                  {
                      selectedSegment = segmentBezierPath.Tag as Segment;
                      SetSelectedSegment(this.selectedSegment);
                  };
                  FieldCanvas.Children.Add(segmentBezierPath);
                  bezierPathList.Add(segmentBezierPath);
              }
          }

          //Method to be called when a major control point i.g One or Four is manipulated 
          private void PointManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
          {
              Ellipse sentEllipse = sender as Ellipse;
              int pointTableNum = int.Parse(sentEllipse.Name);
              App.currentPath.ModifyPoint(pointTableNum - 1, new double[] { App.currentPath.GetPoint(pointTableNum - 1)[0] + e.Delta.Translation.X, App.currentPath.GetPoint(pointTableNum - 1)[1] - e.Delta.Translation.Y });
              Canvas.SetLeft(sentEllipse, App.currentPath.GetPoint(pointTableNum - 1)[0] - 3);
              Canvas.SetTop(sentEllipse, FieldCanvas.Height - (App.currentPath.GetPoint(pointTableNum - 1)[1] + 3));


              ListBoxItem xValueListBoxItem = (ListBoxItem)xValuesListBox.Items[pointTableNum - 1];
              ListBoxItem yValueListBoxItem = (ListBoxItem)yValuesListBox.Items[pointTableNum - 1];

              xValueListBoxItem.Content = Canvas.GetLeft(sentEllipse);
              yValueListBoxItem.Content = FieldCanvas.Height - Canvas.GetTop(sentEllipse);

              if (SegmentListComboBox.SelectedItem != null)
              {
                  DisplayPath(selectedSegment);
              }
              else
                  DisplayPath();



          }

          //Method to be called when a control point two is manipulated
          private void ControlPointTwoManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
          {
              int segmentIndex = int.Parse(((ComboBoxItem)SegmentListComboBox.SelectedItem).Name);
              Segment selectedSegment = App.currentPath.PathList[segmentIndex];
              Ellipse sentControlPoint = sender as Ellipse;
              double[] newControlPointTwo = new double[] { selectedSegment.ControlptTwo[0] + e.Delta.Translation.X, selectedSegment.ControlptTwo[1] - e.Delta.Translation.Y };
              selectedSegment.ControlptTwo = newControlPointTwo;
              App.currentPath.StandardizePath(segmentIndex-1);
              Canvas.SetTop(sentControlPoint, FieldCanvas.Height - (selectedSegment.ControlptTwo[1] + 3));
              Canvas.SetLeft(sentControlPoint, selectedSegment.ControlptTwo[0] - 3);
              ControlPoint2TextboxX.Text = "" + selectedSegment.ControlptTwo[0];
              ControlPoint2TextboxY.Text = "" + selectedSegment.ControlptTwo[1];

              DisplayPath(selectedSegment);
          }

          //Method to be called when a control point three is manipulated
          private void ControlPointThreeManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
          {
              int segmentIndex = int.Parse(((ComboBoxItem)SegmentListComboBox.SelectedItem).Name);
              Segment selectedSegment = App.currentPath.PathList[segmentIndex];
              Ellipse sentControlPoint = sender as Ellipse;
              double[] newControlPointThree = new double[] { selectedSegment.ControlptThree[0] + e.Delta.Translation.X, selectedSegment.ControlptThree[1] - e.Delta.Translation.Y };
              selectedSegment.ControlptThree = newControlPointThree;
              App.currentPath.StandardizePath(segmentIndex);
              Canvas.SetTop(sentControlPoint, FieldCanvas.Height - (selectedSegment.ControlptThree[1] + 3));
              Canvas.SetLeft(sentControlPoint, selectedSegment.ControlptThree[0] - 3);
              ControlPoint3TextboxX.Text = "" + selectedSegment.ControlptThree[0];
              ControlPoint3TextboxY.Text = "" + selectedSegment.ControlptThree[1];
              DisplayPath(selectedSegment);
          }

          //Changes the selected segment based on the combo box selection
          private void SegmentListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
          {
              ComboBox comboBoxSender = sender as ComboBox;
              if (comboBoxSender.SelectedItem == null) return;
              this.selectedSegment = App.currentPath.PathList[int.Parse(((ComboBoxItem)comboBoxSender.SelectedItem).Name)];
              SetSelectedSegment(this.selectedSegment);
              return;
          }

          private void SetSelectedSegment(Segment selectedSegment)
          {
              /// <summary>
              ///     Takes a segment and sets it to be the currently selected segment
              /// </summary>

              if (selectedSegment == null) return;

              FieldCanvas.Children.Remove(currentControlPointTwoEllipse);
              FieldCanvas.Children.Remove(currentControlPointThreeEllipse);

              ComboBox comboBoxSender = SegmentListComboBox;
              if (comboBoxSender.SelectedItem == null) return;
              selectedSegment = App.currentPath.PathList[int.Parse(((ComboBoxItem)comboBoxSender.SelectedItem).Name)];
              ControlPoint2TextboxX.Text = "" + selectedSegment.ControlptTwo[0];
              ControlPoint2TextboxY.Text = "" + selectedSegment.ControlptTwo[1];
              ControlPoint3TextboxX.Text = "" + selectedSegment.ControlptThree[0];
              ControlPoint3TextboxY.Text = "" + selectedSegment.ControlptThree[1];

              Ellipse controlPointTwoEllipse = new Ellipse
              {
                  Width = 6,
                  Height = 6,
                  Fill = new SolidColorBrush(Windows.UI.Colors.Red),
                  Name = "CurrentControlPointTwo",
                  ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
              };
              controlPointTwoEllipse.ManipulationDelta += ControlPointTwoManipulationDelta;
              Canvas.SetTop(controlPointTwoEllipse, FieldCanvas.Height - (selectedSegment.ControlptTwo[1] + 3));
              Canvas.SetLeft(controlPointTwoEllipse, selectedSegment.ControlptTwo[0] - 3);
              Canvas.SetZIndex(controlPointTwoEllipse, 1001);
              FieldCanvas.Children.Add(controlPointTwoEllipse);
              currentControlPointTwoEllipse = controlPointTwoEllipse;

              Ellipse controlPointThreeEllipse = new Ellipse
              {
                  Width = 5,
                  Height = 5,
                  Fill = new SolidColorBrush(Windows.UI.Colors.Yellow),
                  Name = "CurrentControlPointThree",
                  ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY
              };
              controlPointThreeEllipse.ManipulationDelta += ControlPointThreeManipulationDelta;
              Canvas.SetTop(controlPointThreeEllipse, FieldCanvas.Height - selectedSegment.ControlptThree[1]);
              Canvas.SetLeft(controlPointThreeEllipse, selectedSegment.ControlptThree[0]);
              Canvas.SetZIndex(controlPointThreeEllipse, 1001);
              FieldCanvas.Children.Add(controlPointThreeEllipse);
              currentControlPointThreeEllipse = controlPointThreeEllipse;

              DisplayPath(selectedSegment);
          }

          //Makes a point when the mouse is doubleTapped on the canvas
          private void FieldCanvas_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
          {
              double x = e.GetPosition(FieldCanvas).X;
              double y = FieldCanvas.Height - e.GetPosition(FieldCanvas).Y;
              double[] newpt = new double[] { x, y };
              App.currentPath.AddPoint(newpt);
              ImportPath();
          }

          //modifies the control point two based on the text boxes above
          private void ControlPointTwoSaveBtn_Click(object sender, RoutedEventArgs e)
          {
              int selectedSegmentIndex = int.Parse(((ComboBoxItem)SegmentListComboBox.SelectedItem).Name);
              if (SegmentListComboBox.SelectedItem == null) return;
              Segment selectedSegment = App.currentPath.PathList[selectedSegmentIndex];
              if (this.selectedSegment == null) return;
              Ellipse sentControlPoint = currentControlPointTwoEllipse;
              double[] newControlPointTwo = new double[] { double.Parse(ControlPoint2TextboxX.Text), double.Parse(ControlPoint2TextboxY.Text) };
              selectedSegment.ControlptTwo = newControlPointTwo;
              App.currentPath.StandardizePath(selectedSegmentIndex-1);
              Canvas.SetTop(sentControlPoint, FieldCanvas.Height - (selectedSegment.ControlptTwo[1] + 3));
              Canvas.SetLeft(sentControlPoint, selectedSegment.ControlptTwo[0] - 3);
              ControlPoint2TextboxX.Text = "" + selectedSegment.ControlptTwo[0];
              ControlPoint2TextboxY.Text = "" + selectedSegment.ControlptTwo[1];

              DisplayPath(selectedSegment);
          }

          //modifies the control point three based on the text boxes above
          private void ControlPointThreeSaveBtn_Click(object sender, RoutedEventArgs e)
          {
              int selectedSegmentIndex = int.Parse(((ComboBoxItem)SegmentListComboBox.SelectedItem).Name);
              if (SegmentListComboBox.SelectedItem == null) return;
              Segment selectedSegment = App.currentPath.PathList[selectedSegmentIndex];

              if (this.selectedSegment == null) return;
              Ellipse sentControlPoint = currentControlPointThreeEllipse;
              double[] newControlPointThree = new double[] { double.Parse(ControlPoint3TextboxX.Text), double.Parse(ControlPoint3TextboxY.Text) };
              selectedSegment.ControlptThree = newControlPointThree;
              App.currentPath.StandardizePath(selectedSegmentIndex);
              Canvas.SetTop(sentControlPoint, FieldCanvas.Height - (selectedSegment.ControlptThree[1] + 3));
              Canvas.SetLeft(sentControlPoint, selectedSegment.ControlptThree[0] - 3);
              ControlPoint3TextboxX.Text = "" + selectedSegment.ControlptThree[0];
              ControlPoint3TextboxY.Text = "" + selectedSegment.ControlptThree[1];
              DisplayPath(selectedSegment);
          }

          void DeselectSegment(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
          {
              this.selectedSegment = null;
              DisplayPath();
              args.Handled = true;
          }

          private void Button_Click(object sender, RoutedEventArgs e)
          {

          }

          private void Button_Click_1(object sender, RoutedEventArgs e)
          {

          }*/


        



        //METHODS FOR RIGHT SIDE MENU

        
        //Navigation Bar Methods
        //@param sender name  as a list box item is toggle the different windows on and off
        private void RightNavigationListBoxItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ListBoxItem item = sender as ListBoxItem;

            if (item == null)
            {
                SegmentPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                LayerPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else if (item.Name.Equals("SegmentListBoxItem"))
            {
                if (SegmentPopOutStackPanel.Visibility == Windows.UI.Xaml.Visibility.Visible)
                {
                    SegmentPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    LayerPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    RightNaviationListBox.SelectedIndex = -1;
                }
                else
                {

                    SegmentPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    LayerPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
            else if (item.Name.Equals("LayerListBoxItem"))
            {
                if (LayerPopOutStackPanel.Visibility == Windows.UI.Xaml.Visibility.Visible)
                {
                    SegmentPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    LayerPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {

                    SegmentPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    LayerPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    RightNaviationListBox.SelectedIndex = -1;
                }
            }
            else
            {
                SegmentPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                LayerPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        
        //Save Button Click Events For The Segment Selector Menu

        //saves the degrees and points off of the menu to the path
        private void SaveDegreeButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
        //saves the direct coordinates of the points to the path
        private void CoordinateSaveButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        
        //Segment Selector Menu Control Events

        //selects a segment and displays it on the screen
        private void SegmentSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //switches between coordinate and degree mode
        private void ToggleSwitch_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ToggleSwitch switchSender = sender as ToggleSwitch;
            if (switchSender.IsOn)
            {
                DegreeModeStackPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
                CoordinateModeStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                DegreeModeStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                CoordinateModeStackPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void LayerSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}


