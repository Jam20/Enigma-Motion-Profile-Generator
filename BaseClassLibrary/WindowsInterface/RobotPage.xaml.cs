using System;
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
           /* this.InitializeComponent();
            if (App.currentRobot == null) App.currentRobot = new Robot();
            else
            {
                MaxAccelBox.Text = App.currentRobot.MaxAccel.ToString();
                MaxVelbox.Text = App.currentRobot.MaxVel.ToString();
                RobotWidthSlider.Value = App.currentRobot.Width;
                RobotLengthSlider.Value = App.currentRobot.Length;
                WheelSizeTextBox.Text = App.currentRobot.WheelSize.ToString();
                bumperToggleSwitch.IsOn = App.currentRobot.UsingBumpers;
                bumperThicknessSwitch.Value = App.currentRobot.BumperThickness;
            }*/
        }

        private async void RobotSaveBtn_Click(object sender, RoutedEventArgs e)
        {
           /* if (double.TryParse(MaxAccelBox.Text, out double accel) && double.TryParse(MaxVelbox.Text, out double vel) && double.TryParse(WheelSizeTextBox.Text, out double wheels))
            {
                App.currentRobot.MaxAccel = accel;
                App.currentRobot.MaxVel = vel;
                App.currentRobot.TimeIncrementInSec = .01;
                App.currentRobot.WheelSize = wheels;
            }
            else
            {
                MaxAccelBox.Text = "";
                MaxVelbox.Text = "";
                WarningCD warning = new WarningCD("Error: Bad Input", "This field may only contain numeric input.");
                warning.Show();
            }

            App.currentRobot.Width = RobotWidthSlider.Value;
            App.currentRobot.Length = RobotLengthSlider.Value;
            App.currentRobot.UsingBumpers = bumperToggleSwitch.IsOn;
            if (App.currentRobot.UsingBumpers) App.currentRobot.BumperThickness = bumperThicknessSwitch.Value;
            else App.currentRobot.BumperThickness = 0;
            StorageFile robotSaveFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("robotSaveFile.csv", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(robotSaveFile, App.currentRobot.ToString());*/
        }

    }
}
