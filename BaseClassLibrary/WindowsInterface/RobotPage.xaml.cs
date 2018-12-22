
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
        public RobotPage()
        {
           this.InitializeComponent();
            List<Player> temp = App.PlayerList;
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
            if(PlayerComboBox.SelectedIndex == -1)
            {
                WarningCD warning = new WarningCD("Please select which robot you wish to change", "");
                warning.Show();
                return;
            }
            Player selectedPlayer = App.PlayerList[PlayerComboBox.SelectedIndex];
            if (double.TryParse(MaxAccelBox.Text, out double accel) && double.TryParse(MaxVelbox.Text, out double vel) && double.TryParse(WheelSizeTextBox.Text, out double wheels))
            {
                selectedPlayer.Robot.MaxAccel = accel;
                selectedPlayer.Robot.MaxVel = vel;
                selectedPlayer.Robot.TimeIncrementInSec = .01;
                selectedPlayer.Robot.WheelSize = wheels;
            }
            else
            {
                MaxAccelBox.Text = "";
                MaxVelbox.Text = "";
                WarningCD warning = new WarningCD("Error: Bad Input", "This field may only contain numeric input.");
                warning.Show();
            }

            selectedPlayer.Robot.Width = RobotWidthSlider.Value;
            selectedPlayer.Robot.Length = RobotLengthSlider.Value;
            selectedPlayer.Robot.UsingBumpers = bumperToggleSwitch.IsOn;
            if (selectedPlayer.Robot.UsingBumpers) selectedPlayer.Robot.BumperThickness = bumperThicknessSwitch.Value;
            else selectedPlayer.Robot.BumperThickness = 0;
            RefreshPlayerComboBox();
        }

        
        private void PlayerComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (PlayerComboBox.SelectedIndex == -1) return;
            Robot selectedRobot = App.PlayerList[PlayerComboBox.SelectedIndex].Robot;
            MaxAccelBox.Text = selectedRobot.MaxAccel.ToString();
            MaxVelbox.Text = selectedRobot.MaxVel.ToString();
            WheelSizeTextBox.Text = selectedRobot.WheelSize.ToString();
            RobotWidthSlider.Value = selectedRobot.Width;
            RobotLengthSlider.Value = selectedRobot.Length;
            bumperToggleSwitch.IsOn = selectedRobot.UsingBumpers;
            bumperThicknessSwitch.Value = selectedRobot.BumperThickness;
        }
    }
}
