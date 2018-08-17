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
            }
        }

        private void RobotSaveBtn_Click(object sender, RoutedEventArgs e) {
            if(double.TryParse(MaxAccelBox.Text, out double accel) && double.TryParse(MaxVelbox.Text, out double vel)) {
                App.currentRobot.maxAccel = accel;
                App.currentRobot.maxVel = vel;
                App.currentRobot.timeIncrementInSec = .01;
            } else {
                MaxAccelBox.Text = "";
                MaxVelbox.Text = "";
                DisplayWarningDialog("Error: Bad Input", "This field only accepts numeric input.");
            }
        }

        private async void DisplayWarningDialog(string title, string content) {
            //Generic Warning box
            ContentDialog warning = new ContentDialog() {
                Title = title,
                Content = content,
                CloseButtonText = "OK"
            };

            await warning.ShowAsync();
        }
    }
}
