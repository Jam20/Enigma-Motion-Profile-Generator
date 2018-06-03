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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WindowsInterface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void NavigationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectedItem = NavigationListBox.SelectedItem as ListBoxItem;
            RobotListItem.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
            HomeListItem.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
            FieldListItem.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
            SettingsListItem.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
            selectedItem.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 3, 255, 0));
            
            if (selectedItem == HomeListItem) MainFrame.Navigate(typeof(HomePage));
            else if(selectedItem == FieldListItem) MainFrame.Navigate(typeof(Field));
            else if (selectedItem == RobotListItem) MainFrame.Navigate(typeof(Robot));
            else if (selectedItem == SettingsListItem) MainFrame.Navigate(typeof(Settings));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationSplitView.IsPaneOpen = !NavigationSplitView.IsPaneOpen;
        }
    }
}
