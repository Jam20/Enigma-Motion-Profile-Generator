using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using System;
using Windows.UI.Xaml.Shapes;
using BaseClassLibrary;
namespace WindowsInterface
{

    public sealed partial class HomePage : Page
    {
        private List<Player> playerList = App.PlayerList;
        private int selectedPlayerIndex = -1;
        private int selectedLayerIndex = -1;

        //constructs the page and initalizes variables needed for proper function
        public HomePage()
        {
            playerList = new List<Player>();
            this.InitializeComponent();
            App.FieldCanvasHeight = FieldCanvas.Height;
            App.FieldCanvasWidth = FieldCanvas.Width;
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
            Segment selectedSegment = playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).GetPath().PathList[playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).SelectedSegmentIndex];

            //ControlPointOne
            selectedSegment.ControlptOne[0] = Double.Parse(StartPointXTextBlock.Text);
            selectedSegment.ControlptOne[1] = Double.Parse(StartPointYTextBlock.Text);

            //ControlPointTwo
            double xDist = Math.Abs(selectedSegment.ControlptOne[0] - selectedSegment.ControlptTwo[0]);
            double yDist = Math.Abs(selectedSegment.ControlptOne[1] - selectedSegment.ControlptTwo[1]);
            double distance = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
            selectedSegment.ControlptTwo[0] = distance * Math.Cos(Math.PI/2.0+ -(Math.PI/180.0)*Double.Parse(StartingAngleTextBlock.Text)) + selectedSegment.ControlptOne[0];
            selectedSegment.ControlptTwo[1] = distance * Math.Sin(Math.PI/2.0+ -(Math.PI/180.0)*Double.Parse(StartingAngleTextBlock.Text)) + selectedSegment.ControlptOne[1];

            //ControlPointThree
            xDist = Math.Abs(selectedSegment.ControlptThree[0] - selectedSegment.ControlptFour[0]);
            yDist = Math.Abs(selectedSegment.ControlptThree[1] - selectedSegment.ControlptFour[1]);
            distance = -Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
            selectedSegment.ControlptThree[0] = distance * Math.Cos(Math.PI/2.0+ -(Math.PI/180.0)*Double.Parse(EndingAngleXTextBlock.Text)) + selectedSegment.ControlptFour[0];
            selectedSegment.ControlptThree[1] = distance * Math.Sin(Math.PI/2.0+ -(Math.PI/180.0)*Double.Parse(EndingAngleXTextBlock.Text)) + selectedSegment.ControlptFour[1];

            //ControlPointFour
            selectedSegment.ControlptFour[0] = Double.Parse(EndPointXTextBlock.Text);
            selectedSegment.ControlptFour[1] = Double.Parse(EndPointYTextBlock.Text);
            playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).RefreshCanvas();

        }
        //saves the direct coordinates of the points to the path
        private void CoordinateSaveButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Segment selectedSegment = playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).GetPath().PathList[playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).SelectedSegmentIndex];

            selectedSegment.ControlptOne[0] = Double.Parse(ControlPointOneXTextBox.Text);
            selectedSegment.ControlptOne[1] = Double.Parse(ControlPointOneYTextBox.Text);

            selectedSegment.ControlptTwo[0] = Double.Parse(ControlPointTwoXTextBox.Text);
            selectedSegment.ControlptTwo[1] = Double.Parse(ControlPointTwoYTextBox.Text);

            selectedSegment.ControlptThree[0] = Double.Parse(ControlPointThreeXTextBox.Text);
            selectedSegment.ControlptThree[1] = Double.Parse(ControlPointThreeYTextBox.Text);

            selectedSegment.ControlptFour[0] = Double.Parse(ControlPointFourXTextBox.Text);
            selectedSegment.ControlptFour[1] = Double.Parse(ControlPointFourYTextBox.Text);

            playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).RefreshCanvas();

        }


        //Segment Selector Menu Control Events

        //selects a segment and displays it on the screen
        private void SegmentSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int test = selectedLayerIndex;
            playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).SelectedSegmentIndex = SegmentSelectorComboBox.SelectedIndex;
            playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).CompileCanvas();
            RefreshSegmentModificationBoxes();
        }

        private void RefreshSegmentModificationBoxes()
        {
            if (playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).SelectedSegmentIndex == -1) return;
            Segment selectedSegment = playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).GetPath().PathList[playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).SelectedSegmentIndex];

            //ControlPointOne
            ControlPointOneXTextBox.Text = selectedSegment.ControlptOne[0].ToString();
            ControlPointOneYTextBox.Text = selectedSegment.ControlptOne[1].ToString();
            StartPointXTextBlock.Text = selectedSegment.ControlptOne[0].ToString();
            StartPointYTextBlock.Text = selectedSegment.ControlptOne[1].ToString();

            //ControlPointTwo
            ControlPointTwoXTextBox.Text = selectedSegment.ControlptTwo[0].ToString();
            ControlPointTwoYTextBox.Text = selectedSegment.ControlptTwo[1].ToString();
            StartingAngleTextBlock.Text = selectedSegment.GetDirectionAt(0).ToString();

            //ControlPointThree
            ControlPointThreeXTextBox.Text = selectedSegment.ControlptThree[0].ToString();
            ControlPointThreeYTextBox.Text = selectedSegment.ControlptThree[1].ToString();
            EndingAngleXTextBlock.Text = selectedSegment.GetDirectionAt(selectedSegment.SegmentLength).ToString();

            //ControlPointFour
            ControlPointFourXTextBox.Text = selectedSegment.ControlptFour[0].ToString();
            ControlPointFourYTextBox.Text = selectedSegment.ControlptFour[1].ToString();
            EndPointXTextBlock.Text = selectedSegment.ControlptFour[0].ToString();
            EndPointYTextBlock.Text = selectedSegment.ControlptFour[1].ToString();

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
            foreach (Segment segment in playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).GetPath().PathList)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Content = "Segment" + (SegmentSelectorComboBox.Items.Count+1),
                    FontSize = 25
                };
                SegmentSelectorComboBox.Items.Add(comboBoxItem);
            }
            
        }

        private void RefreshLayerComboBox()
        {
            LayerSelectorComboBox.Items.Clear();
            if (selectedPlayerIndex == -1) return;
            for (int i = 0; i < playerList[selectedPlayerIndex].GetNumberOfLayers(); i++)
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

        }
        private void RefreshPlayerComboBox()
        {
            PlayerSelectorComboBox.Items.Clear();
            for (int i = 0; i < playerList.Count; i++)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Content = "Team " + (TeamNumberTextBox.Text),
                    FontSize = 25
                };
                PlayerSelectorComboBox.Items.Add(comboBoxItem);
            }

        }
        private void RefreshHolderCanvas()
        {
            HolderCanvas.Children.Clear();
            foreach (Player player in playerList)
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
            RefreshSegmentComboBox();
            playerList[selectedPlayerIndex].CompileCanvas(selectedLayerIndex);
        }

        private void PlayerSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            selectedPlayerIndex = box.SelectedIndex;
            RefreshLayerComboBox();
            RefreshHolderCanvas();
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
            if(result == ContentDialogResult.Primary)
            {
                playerList.RemoveAt(selectedPlayerIndex);
                RefreshHolderCanvas();
                RefreshPlayerComboBox();
                RefreshLayerComboBox();
            }

        }

        private void NewLayerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (selectedPlayerIndex == -1) return;
            playerList[selectedPlayerIndex].CreateLayer();
            ComboBoxItem comboBoxItem = new ComboBoxItem
            {
                Content = "Layer" +(LayerSelectorComboBox.Items.Count+1),
                FontSize = 25
            };
            LayerSelectorComboBox.Items.Add(comboBoxItem);
            LayerSelectorComboBox.SelectedIndex = LayerSelectorComboBox.Items.Count - 1;
            selectedLayerIndex = LayerSelectorComboBox.SelectedIndex;
            playerList[selectedPlayerIndex].CompileCanvas(selectedLayerIndex);
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

            if(result == ContentDialogResult.Primary)
            {
                playerList[selectedPlayerIndex].DeleteLayer(selectedLayerIndex);
                RefreshLayerComboBox();
            }

        }

        private void FieldCanvas_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            if (selectedPlayerIndex == -1 || playerList[selectedPlayerIndex].GetNumberOfLayers() == 0) return;
            double x = e.GetPosition(FieldCanvas).X;
            double y = FieldCanvas.Height - e.GetPosition(FieldCanvas).Y;
            double[] newpt = new double[] { x, y };
            playerList[selectedPlayerIndex].GetLayer(selectedLayerIndex).AddPoint(newpt);
            RefreshSegmentComboBox();
        }

        private void ConfirmNewPlayerBtnBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            playerList.Add(new Player(FieldCanvas.Width, FieldCanvas.Height, TeamNumberTextBox.Text,App.CurrentRobot));
            ComboBoxItem comboBoxItem = new ComboBoxItem
            {
                Content = "Team " + (TeamNumberTextBox.Text),
                FontSize = 25
            };
            PlayerSelectorComboBox.Items.Add(comboBoxItem);
            HolderCanvas.Children.Add(playerList[playerList.Count-1].MainCanvas);
            PlayerSelectorComboBox.SelectedIndex = PlayerSelectorComboBox.Items.Count-1;
            selectedPlayerIndex = PlayerSelectorComboBox.SelectedIndex;
            RefreshHolderCanvas();
            NewPlayerPopup.IsOpen = false;
        }

    }
}


