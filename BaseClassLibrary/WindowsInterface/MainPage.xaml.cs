using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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

        //Navigates to the selected page in the Hamburger Navigation
        private void NavigationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectedItem = NavigationListBox.SelectedItem as ListBoxItem;
            RobotListItem.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
            HomeListItem.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
            FieldListItem.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
            SettingsListItem.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 255, 255, 255));
            selectedItem.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 3, 255, 0));

            if (selectedItem == HomeListItem) MainFrame.Navigate(typeof(HomePage));
            else if (selectedItem == FieldListItem) MainFrame.Navigate(typeof(Field));
            else if (selectedItem == RobotListItem) MainFrame.Navigate(typeof(RobotPage));
            else if (selectedItem == SettingsListItem) MainFrame.Navigate(typeof(Settings));
        }

        //Opens the pane when the burger button is clicked
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationSplitView.IsPaneOpen = !NavigationSplitView.IsPaneOpen;
        }

        //Exports the current path to a csv file including the path itself and a motion profile based on it
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerExportComboBox.Items.Clear();
            foreach (Player player in App.PlayerList)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    FontSize = 25,
                    Content = player.TeamNumber,
                };
                PlayerExportComboBox.Items.Add(item);
            }
            SavePlayerPopup.IsOpen = true;
        }

        //imports a path file with and sets the current path equal to it than reloads to the homePage
        private async void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileSelector = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            fileSelector.FileTypeFilter.Add(".csv");

            StorageFile file = await fileSelector.PickSingleFileAsync();

            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                IList<string> lines = await FileIO.ReadLinesAsync(file);
                App.PlayerList.Add(SaveSystem.LoadSaveFile(lines));
                MainFrame.Navigate(typeof(HomePage));
            }
            else
            {
                WarningCD warning = new WarningCD("Error: File not found", "No file was selected.");
                warning.Show();
            }

        }
        
        private async void PlayerSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker fileSelector = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            fileSelector.FileTypeChoices.Add("csv", new List<string>() { ".csv" });
            fileSelector.SuggestedFileName = "Profile0";

            StorageFile file = await fileSelector.PickSaveFileAsync();
            //More safety needs to be added. Popups for exported and failed to export should also eventually appear.
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                String[] lines = SaveSystem.MakeSaveFile(App.PlayerList[PlayerExportComboBox.SelectedIndex]);
                await FileIO.WriteLinesAsync(file, lines);

                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);

                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    //Yay it saved.
                    WarningCD warning = new WarningCD("Alert", "File Successfully saved.");
                    warning.Show();
                }
            }
            else
            {
                WarningCD warning = new WarningCD("Error: File not found", "No file was selected.");
                warning.Show();
            }
            SavePlayerPopup.IsOpen = false;
        }
    }
}
