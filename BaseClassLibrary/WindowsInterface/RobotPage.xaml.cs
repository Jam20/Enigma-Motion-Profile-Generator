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
            this.InitializeComponent();
            if (App.currentRobot == null) App.currentRobot = new Robot();
            else {
                MaxAccelBox.Text = App.currentRobot.maxAccel.ToString();
                MaxVelbox.Text = App.currentRobot.maxVel.ToString();
                RobotWidthSlider.Value = App.currentRobot.width;
                RobotLengthSlider.Value = App.currentRobot.length;
                WheelSizeTextBox.Text = App.currentRobot.wheelSize.ToString();
                bumperToggleSwitch.IsOn = App.currentRobot.usingBumpers;
                bumperThicknessSwitch.Value = App.currentRobot.bumperThickness;
            }
        }

        private async void RobotSaveBtn_Click(object sender, RoutedEventArgs e) {
            if(double.TryParse(MaxAccelBox.Text, out double accel) && double.TryParse(MaxVelbox.Text, out double vel) && double.TryParse(WheelSizeTextBox.Text, out double wheels)) {
                App.currentRobot.maxAccel = accel;
                App.currentRobot.maxVel = vel;
                App.currentRobot.timeIncrementInSec = .01;
                App.currentRobot.wheelSize = wheels;
            } else {
                MaxAccelBox.Text = "";
                MaxVelbox.Text = "";
                WarningCD warning = new WarningCD("Error: Bad Input", "This field may only contain numeric input.");
                warning.Show();
            }

            App.currentRobot.width = RobotWidthSlider.Value;
            App.currentRobot.length = RobotLengthSlider.Value;
            App.currentRobot.usingBumpers = bumperToggleSwitch.IsOn;
            if (App.currentRobot.usingBumpers) App.currentRobot.bumperThickness = bumperThicknessSwitch.Value;
            else App.currentRobot.bumperThickness = 0;
            StorageFile robotSaveFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("robotSaveFile.csv", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(robotSaveFile, App.currentRobot.ToString());
        }
        
    }
}
