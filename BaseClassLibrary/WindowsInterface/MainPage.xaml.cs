using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
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
            MainFrame.Navigate(typeof(HomePage));
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
            else if (selectedItem == RobotListItem) MainFrame.Navigate(typeof(RobotPage));
            else if (selectedItem == SettingsListItem) MainFrame.Navigate(typeof(Settings));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationSplitView.IsPaneOpen = !NavigationSplitView.IsPaneOpen;
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e) {
            FileSavePicker fileSelector = new FileSavePicker();
            fileSelector.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            fileSelector.FileTypeChoices.Add("csv", new List<string>() { ".csv" });
            fileSelector.SuggestedFileName = "Profile0";

            StorageFile file = await fileSelector.PickSaveFileAsync();
            //More safety needs to be added. Popups for exported and failed to export should also eventually appear.
            if(file != null) {
                CachedFileManager.DeferUpdates(file);

                await FileIO.WriteLinesAsync(file, SaveFile.GetSaveFile(new BaseClassLibrary.MotionProfile(App.currentPath, App.currentRobot), "placeholder"));

                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);

                if(status == Windows.Storage.Provider.FileUpdateStatus.Complete) {
                    //Yay it saved.
                }
            }
            
            //SaveFile.GetSaveFile(new BaseClassLibrary.MotionProfile(App.currentPath, App.currentRobot),path);
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e) {

        }
    }
}
