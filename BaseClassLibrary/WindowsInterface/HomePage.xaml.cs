using System;
using System.Collections.Generic;
using System.Threading;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WindowsInterface
{

    public sealed partial class HomePage : Page
    {
        private int selectedPlayerIndex = -1;
        private int selectedLayerIndex = -1;

        //constructs the page and initalizes variables needed for proper function
        public HomePage()
        {
            this.InitializeComponent();
            List<Player> temp = App.PlayerList;
            RefreshPlayerComboBox();
            App.FieldCanvasHeight = FieldCanvas.Height;
            App.FieldCanvasWidth = FieldCanvas.Width; ResetCanvases();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TimeSpan period = TimeSpan.FromSeconds(.1);

            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                //
                // TODO: Work
                //

                //
                // Update the UI thread by using the UI core dispatcher.
                //
                await Dispatcher.RunAsync(CoreDispatcherPriority.High,
                    () =>
                    {
                       RefreshSegmentModificationBoxes();
                       
            });

            }, period);
        }

        public void ResetCanvases()
        {
            foreach (Player player in App.PlayerList)
            {
                player.ResetPlayerCanvas();
            }
            RefreshHolderCanvas();

        }

        //METHODS FOR RIGHT SIDE MENU


        //Navigation Bar Methods
        //@param sender name  as a list box item is toggle the different windows on and off
        private void RightNavigationListBoxItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ListBoxItem item = sender as ListBoxItem;

            if (item.Name.Equals("SegmentListBoxItem"))
            {
                if (SegmentPopOutStackPanel.Visibility == Windows.UI.Xaml.Visibility.Visible)
                {
                    SegmentPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    LayerPopOutStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    ColumnTwoStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    RightNaviationListBox.SelectedIndex = -1;
                }
                else
                {
                    ColumnTwoStackPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
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
                    ColumnTwoStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {
                    ColumnTwoStackPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
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
            Segment selectedSegment = App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).GetPath().PathList[App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).SelectedSegmentIndex];

            //ControlPointOne
            selectedSegment.ControlptOne[0] = double.Parse(StartPointXTextBlock.Text);
            selectedSegment.ControlptOne[1] = double.Parse(StartPointYTextBlock.Text);

            //ControlPointTwo
            double xDist = Math.Abs(selectedSegment.ControlptOne[0] - selectedSegment.ControlptTwo[0]);
            double yDist = Math.Abs(selectedSegment.ControlptOne[1] - selectedSegment.ControlptTwo[1]);
            double distance = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
            selectedSegment.ControlptTwo[0] = distance * Math.Cos(Math.PI / 2.0 + -(Math.PI / 180.0) * double.Parse(StartingAngleTextBlock.Text)) + selectedSegment.ControlptOne[0];
            selectedSegment.ControlptTwo[1] = distance * Math.Sin(Math.PI / 2.0 + -(Math.PI / 180.0) * double.Parse(StartingAngleTextBlock.Text)) + selectedSegment.ControlptOne[1];

            //ControlPointThree
            xDist = Math.Abs(selectedSegment.ControlptThree[0] - selectedSegment.ControlptFour[0]);
            yDist = Math.Abs(selectedSegment.ControlptThree[1] - selectedSegment.ControlptFour[1]);
            distance = -Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
            selectedSegment.ControlptThree[0] = distance * Math.Cos(Math.PI / 2.0 + -(Math.PI / 180.0) * double.Parse(EndingAngleXTextBlock.Text)) + selectedSegment.ControlptFour[0];
            selectedSegment.ControlptThree[1] = distance * Math.Sin(Math.PI / 2.0 + -(Math.PI / 180.0) * double.Parse(EndingAngleXTextBlock.Text)) + selectedSegment.ControlptFour[1];

            //ControlPointFour
            selectedSegment.ControlptFour[0] = double.Parse(EndPointXTextBlock.Text);
            selectedSegment.ControlptFour[1] = double.Parse(EndPointYTextBlock.Text);
            App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).RefreshCanvas();

        }
        //saves the direct coordinates of the points to the path
        private void CoordinateSaveButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!(double.TryParse(ControlPointOneXTextBox.Text, out double cpOneX) && double.TryParse(ControlPointOneYTextBox.Text, out double cpOneY) && double.TryParse(ControlPointTwoXTextBox.Text, out double cpTwoX) && double.TryParse(ControlPointTwoYTextBox.Text, out double cpTwoY) && double.TryParse(ControlPointThreeXTextBox.Text, out double cpThreeX) && double.TryParse(ControlPointThreeYTextBox.Text, out double cpThreeY) && double.TryParse(ControlPointFourXTextBox.Text, out double cpFourX) && double.TryParse(ControlPointFourYTextBox.Text, out double cpFourY)))
            {
                WarningCD warning = new WarningCD("Please enter only numeric values", "");
                warning.Show();
                return;
            }
            else if (selectedPlayerIndex == -1 || selectedLayerIndex == -1 || App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).SelectedSegmentIndex == -1)
            {
                WarningCD warning = new WarningCD("Please select", "a Player, a Layer, a Segment");
                warning.Show();
                return;
            }
            Segment selectedSegment = App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).GetPath().PathList[App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).SelectedSegmentIndex];

            selectedSegment.ControlptOne[0] = cpOneX;
            selectedSegment.ControlptOne[1] = cpOneY;

            selectedSegment.ControlptTwo[0] = cpTwoX;
            selectedSegment.ControlptTwo[1] = cpTwoY;

            selectedSegment.ControlptThree[0] = cpThreeX;
            selectedSegment.ControlptThree[1] = cpThreeY;

            selectedSegment.ControlptFour[0] = cpFourX;
            selectedSegment.ControlptFour[1] = cpFourY;

            App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).RefreshCanvas();

        }


        //Segment Selector Menu Control Events

        //selects a segment and displays it on the screen
        private void SegmentSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int test = selectedLayerIndex;
            App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).SelectedSegmentIndex = SegmentSelectorComboBox.SelectedIndex;
            App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).CompileCanvas();
            RefreshSegmentModificationBoxes();
        }

        private void RefreshSegmentModificationBoxes()
        {

            if (selectedPlayerIndex == -1 || selectedLayerIndex == -1 || App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).SelectedSegmentIndex == -1) return;
            Segment selectedSegment = App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).GetPath().PathList[App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).SelectedSegmentIndex];

            //ControlPointOne
            ControlPointOneXTextBox.Text = ((int)selectedSegment.ControlptOne[0]).ToString();
            ControlPointOneYTextBox.Text = ((int)selectedSegment.ControlptOne[1]).ToString();
            StartPointXTextBlock.Text = ((int)selectedSegment.ControlptOne[0]).ToString();
            StartPointYTextBlock.Text = ((int)selectedSegment.ControlptOne[1]).ToString();

            //ControlPointTwo
            ControlPointTwoXTextBox.Text = ((int)selectedSegment.ControlptTwo[0]).ToString();
            ControlPointTwoYTextBox.Text = ((int)selectedSegment.ControlptTwo[1]).ToString();
            StartingAngleTextBlock.Text = ((int)selectedSegment.GetDirectionAt(0)).ToString();

            //ControlPointThree
            ControlPointThreeXTextBox.Text = ((int)selectedSegment.ControlptThree[0]).ToString();
            ControlPointThreeYTextBox.Text = ((int)selectedSegment.ControlptThree[1]).ToString();
            EndingAngleXTextBlock.Text = ((int)selectedSegment.GetDirectionAt(selectedSegment.SegmentLength)).ToString();

            //ControlPointFour
            ControlPointFourXTextBox.Text = ((int)selectedSegment.ControlptFour[0]).ToString();
            ControlPointFourYTextBox.Text = ((int)selectedSegment.ControlptFour[1]).ToString();
            EndPointXTextBlock.Text = ((int)selectedSegment.ControlptFour[0]).ToString();
            EndPointYTextBlock.Text = ((int)selectedSegment.ControlptFour[1]).ToString();

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

        private void RefreshSegmentComboBox()
        {
            if (selectedLayerIndex == -1) return;
            SegmentSelectorComboBox.Items.Clear();
            foreach (Segment segment in App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).GetPath().PathList)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Content = "Segment" + (SegmentSelectorComboBox.Items.Count + 1),
                    FontSize = 25
                };
                SegmentSelectorComboBox.Items.Add(comboBoxItem);
            }
        }

        private void RefreshLayerComboBox()
        {
            LayerSelectorComboBox.Items.Clear();
            if (selectedPlayerIndex == -1) return;
            for (int i = 0; i < App.PlayerList[selectedPlayerIndex].GetNumberOfLayers(); i++)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Content = "Layer" + (LayerSelectorComboBox.Items.Count + 1),
                    FontSize = 25
                };
                LayerSelectorComboBox.Items.Add(comboBoxItem);
            }
            if (LayerSelectorComboBox.Items.Count > 0)
            {
                LayerSelectorComboBox.SelectedIndex = 0;
                selectedLayerIndex = 0;
            }
            else
            {
                NewLayerButton_Click(null, null);
                LayerSelectorComboBox.SelectedIndex = 0;
                selectedLayerIndex = 0;
            }

        }
        private void RefreshPlayerComboBox()
        {
            PlayerSelectorComboBox.Items.Clear();
            for (int i = 0; i < App.PlayerList.Count; i++)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Content = "Team " + (App.PlayerList[i].TeamNumber),
                    FontSize = 25
                };
                PlayerSelectorComboBox.Items.Add(comboBoxItem);
            }

        }
        private void RefreshHolderCanvas()
        {
            HolderCanvas.Children.Clear();
            foreach (Player player in App.PlayerList)
            {
                player.CompileCanvas(-1);
                HolderCanvas.Children.Add(player.MainCanvas);
            }
        }


        private void LayerSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            selectedLayerIndex = box.SelectedIndex;
            if (selectedPlayerIndex == -1) return;
            if (selectedLayerIndex == -1) return;
            RefreshSegmentComboBox();
            App.PlayerList[selectedPlayerIndex].CompileCanvas(selectedLayerIndex);
            ReverseButton.IsOn = !App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).Profile.Path.IsReversed;


        }

        private void PlayerSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            selectedPlayerIndex = box.SelectedIndex;
            RefreshHolderCanvas();
            RefreshLayerComboBox();
        }

        private void NewPlayerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NewPlayerPopup.IsOpen = !NewPlayerPopup.IsOpen;
        }

        private async void DeletePlayerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete Primary Permanently?",
                Content = "If you delete this primary, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                App.PlayerList.RemoveAt(selectedPlayerIndex);
                RefreshHolderCanvas();
                RefreshPlayerComboBox();
                RefreshLayerComboBox();
            }

        }

        private void NewLayerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (selectedPlayerIndex == -1) return;
            App.PlayerList[selectedPlayerIndex].CreateLayer();
            ComboBoxItem comboBoxItem = new ComboBoxItem
            {
                Content = "Layer" + (LayerSelectorComboBox.Items.Count + 1),
                FontSize = 25
            };
            LayerSelectorComboBox.Items.Add(comboBoxItem);
            LayerSelectorComboBox.SelectedIndex = LayerSelectorComboBox.Items.Count - 1;
            selectedLayerIndex = LayerSelectorComboBox.SelectedIndex;
            App.PlayerList[selectedPlayerIndex].CompileCanvas(selectedLayerIndex);
        }

        private async void DeleteLayerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete Layer Permanently?",
                Content = "If you delete this layer, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                App.PlayerList[selectedPlayerIndex].DeleteLayer(selectedLayerIndex);
                RefreshLayerComboBox();
            }

        }

        private void FieldCanvas_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            if (selectedPlayerIndex == -1 || App.PlayerList[selectedPlayerIndex].GetNumberOfLayers() == 0) return;
            double x = e.GetPosition(FieldCanvas).X;
            double y = FieldCanvas.Height - e.GetPosition(FieldCanvas).Y;
            double[] newpt = new double[] { x, y };
            App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).AddPoint(newpt);
            RefreshSegmentComboBox();
            App.PlayerList[selectedPlayerIndex].CompileCanvas(selectedLayerIndex);
        }

        private void ConfirmNewPlayerBtnBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.PlayerList.Add(new Player(FieldCanvas.Width, FieldCanvas.Height, TeamNumberTextBox.Text, new Robot()));
            ComboBoxItem comboBoxItem = new ComboBoxItem
            {
                Content = "Team " + (TeamNumberTextBox.Text),
                FontSize = 25
            };
            PlayerSelectorComboBox.Items.Add(comboBoxItem);
            HolderCanvas.Children.Add(App.PlayerList[App.PlayerList.Count - 1].MainCanvas);
            PlayerSelectorComboBox.SelectedIndex = PlayerSelectorComboBox.Items.Count - 1;
            selectedPlayerIndex = PlayerSelectorComboBox.SelectedIndex;
            RefreshHolderCanvas();
            NewPlayerPopup.IsOpen = false;
        }

        private void ReverseButton_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (selectedLayerIndex == -1) return;
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            App.PlayerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).GetPath().IsReversed = !toggleSwitch.IsOn;
            RefreshSegmentComboBox();
            App.PlayerList[selectedPlayerIndex].CompileCanvas(selectedLayerIndex);

        }
    }
}


