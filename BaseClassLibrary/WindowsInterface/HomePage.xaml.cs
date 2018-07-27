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
using BaseClassLibrary;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WindowsInterface
{

    public sealed partial class HomePage : Page{

        

        public HomePage(){
            this.InitializeComponent();
            if (App.currentPath == null) {
                App.currentPath = new Path();
            }
            else {
                importPath();
            }
        }
        public HomePage(Path p) {
            this.InitializeComponent();
            App.currentPath = p;
        }

        private void savePointBtn_Click(object sender, RoutedEventArgs e) {
            double[] newpt = new double[] { Double.Parse(xPointInputTextBox.Text), Double.Parse(yPointInputTextBox.Text) };
            App.currentPath.addPoint(newpt);
            ListBoxItem xValueListBoxItem = new ListBoxItem();
            ListBoxItem yValueListBoxItem = new ListBoxItem();
            ListBoxItem numberListBoxItem = new ListBoxItem();
            xValueListBoxItem.Content = xPointInputTextBox.Text;
            yValueListBoxItem.Content = yPointInputTextBox.Text;
            numberListBoxItem.Content = pointNumberListBox.Items.Count+1;
            xValueListBoxItem.FontSize = 30;
            yValueListBoxItem.FontSize = 30;
            numberListBoxItem.FontSize = 30;
            xValuesListBox.Items.Add(xValueListBoxItem);
            yValuesListBox.Items.Add(yValueListBoxItem);
            pointNumberListBox.Items.Add(numberListBoxItem);

        }

        private void pointsTableSelectionChanged(object sender, SelectionChangedEventArgs e) {
            ListBox senderListBox = (ListBox)sender;
            if(senderListBox == xValuesListBox) {
                yValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
                pointNumberListBox.SelectedIndex = senderListBox.SelectedIndex;
            }
            else if(senderListBox == yValuesListBox) {
                xValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
                pointNumberListBox.SelectedIndex = senderListBox.SelectedIndex;
            }
            else {
                xValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
                yValuesListBox.SelectedIndex = senderListBox.SelectedIndex;
            }
        }

        private void importPath() {
            if (App.currentPath.getPoints() == null) return;
            for (int i = 0; i < App.currentPath.getPoints().Length; i++) {
                ListBoxItem xValueListBoxItem = new ListBoxItem();
                ListBoxItem yValueListBoxItem = new ListBoxItem();
                ListBoxItem numberListBoxItem = new ListBoxItem();
                xValueListBoxItem.Content = App.currentPath.getPoints()[i][0].ToString();
                yValueListBoxItem.Content = App.currentPath.getPoints()[i][1].ToString();
                numberListBoxItem.Content = pointNumberListBox.Items.Count + 1;
                xValueListBoxItem.FontSize = 30;
                yValueListBoxItem.FontSize = 30;
                numberListBoxItem.FontSize = 30;
                xValuesListBox.Items.Add(xValueListBoxItem);
                yValuesListBox.Items.Add(yValueListBoxItem);
                pointNumberListBox.Items.Add(numberListBoxItem);
            }
        }
    }
}
