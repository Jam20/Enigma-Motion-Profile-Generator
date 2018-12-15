
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
            if (App.CurrentRobot == null) App.CurrentRobot = new Robot();
            else
            {
                MaxAccelBox.Text = App.CurrentRobot.MaxAccel.ToString();
                MaxVelbox.Text = App.CurrentRobot.MaxVel.ToString();
                RobotWidthSlider.Value = App.CurrentRobot.Width;
                RobotLengthSlider.Value = App.CurrentRobot.Length;
                WheelSizeTextBox.Text = App.CurrentRobot.WheelSize.ToString();
                bumperToggleSwitch.IsOn = App.CurrentRobot.UsingBumpers;
                bumperThicknessSwitch.Value = App.CurrentRobot.BumperThickness;
            }
        }

        private async void RobotSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(MaxAccelBox.Text, out double accel) && double.TryParse(MaxVelbox.Text, out double vel) && double.TryParse(WheelSizeTextBox.Text, out double wheels))
            {
                App.CurrentRobot.MaxAccel = accel;
                App.CurrentRobot.MaxVel = vel;
                App.CurrentRobot.TimeIncrementInSec = .01;
                App.CurrentRobot.WheelSize = wheels;
            }
            else
            {
                MaxAccelBox.Text = "";
                MaxVelbox.Text = "";
                WarningCD warning = new WarningCD("Error: Bad Input", "This field may only contain numeric input.");
                warning.Show();
            }

            App.CurrentRobot.Width = RobotWidthSlider.Value;
            App.CurrentRobot.Length = RobotLengthSlider.Value;
            App.CurrentRobot.UsingBumpers = bumperToggleSwitch.IsOn;
            if (App.CurrentRobot.UsingBumpers) App.CurrentRobot.BumperThickness = bumperThicknessSwitch.Value;
            else App.CurrentRobot.BumperThickness = 0;
            StorageFile robotSaveFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("robotSaveFile.csv", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(robotSaveFile, App.CurrentRobot.ToString());
        }

    }
}
