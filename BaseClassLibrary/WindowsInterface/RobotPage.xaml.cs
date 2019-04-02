
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WindowsInterface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RobotPage : Page
    {
        Robot selectedRobot;
        public RobotPage()
        {
           this.InitializeComponent();
            RefreshPlayerComboBox();
        }
        private void RefreshPlayerComboBox()
        {
            PlayerComboBox.Items.Clear();
            foreach (Player player in App.PlayerList)
            {
                ComboBoxItem item = new ComboBoxItem()
                {
                    FontSize = 25,
                    Content = "Team " + player.TeamNumber,
                };
                PlayerComboBox.Items.Add(item);
            }
        }

        private void RobotSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRobot == null) return;
            double vel;
            double accel;
            if (Double.TryParse(MaxVelbox.Text, out vel) && Double.TryParse(MaxAccelBox.Text, out accel))
            {
                selectedRobot.MaxVel = vel;
                selectedRobot.MaxAccel = accel;
            }
            else new WarningCD("NaN", "Please enter numeric values for both the acceleration and the velocity");
            selectedRobot.Width = RobotWidthSlider.Value;
            selectedRobot.Length = RobotLengthSlider.Value;
        }

        
        private void PlayerComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (PlayerComboBox.SelectedIndex == -1) return;
            LayerComboBox.Items.Clear();
            if (PlayerComboBox.SelectedIndex == -1) return;
            for (int i = 0; i < App.PlayerList[PlayerComboBox.SelectedIndex].GetNumberOfLayers(); i++)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Content = "Layer" + (LayerComboBox.Items.Count + 1),
                    FontSize = 25
                };
                LayerComboBox.Items.Add(comboBoxItem);
            }
            if (LayerComboBox.Items.Count > 0)
            {
                LayerComboBox.SelectedIndex = 0;
                LayerComboBox.SelectedIndex = 0;
            }
            else
            {
                LayerComboBox.SelectedIndex = -1;
            }
        }

        private void LayerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlayerComboBox.SelectedIndex == -1 || LayerComboBox.SelectedIndex == -1) return;
            selectedRobot = App.PlayerList[PlayerComboBox.SelectedIndex].GetLayer(LayerComboBox.SelectedIndex).Profile.Robot;

            MaxVelbox.Text = selectedRobot.MaxVel.ToString();
            MaxAccelBox.Text = selectedRobot.MaxAccel.ToString();
            RobotWidthSlider.Value = selectedRobot.Width;
            RobotLengthSlider.Value = selectedRobot.Length;
        }
    }
}
