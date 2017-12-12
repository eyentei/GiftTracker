using GiftTrackerClasses;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PropertyChanged;

namespace GiftTracker
{
    /// <summary>
    /// Interaction logic for AddOrEditGiftWindow.xaml
    /// </summary>
    public partial class AddOrEditGiftWindow : Window
    {
        Gift CurrentGift { get; set; }
        Gift TemporaryGift { get; set; }
        GTRepository<Gift> GiftRepository { get; set; }
        bool IsEdited { get; set; }
        public AddOrEditGiftWindow(GTContext context, Gift gift = null)
        {
            InitializeComponent();
            GiftRepository = new GTRepository<Gift>(context);
            userImageItems.ItemsSource = Directory.EnumerateFiles(@"..\..\Images\DefaultGiftImages", "*.png");

            if (gift == null)
            {
                IsEdited = false;
                TemporaryGift = new Gift();
                userImageItems.SelectedIndex = 0;
                userImageItems.Focus();
            }
            else
            {
                CurrentGift = gift;
                TemporaryGift = new Gift() { Image = gift.Image, Name = gift.Name };
                IsEdited = true;
            }
            this.DataContext = TemporaryGift;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = OpenFileDialogHelper.OpenImageFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                userImageItems.UnselectAll();
                string fileName = dialog.FileName;
                TemporaryGift.Image = ImageHelper.BitmapSourceToByteArray(fileName);
            }
        }

        private void UserImageItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (userImageItems.SelectedItems.Count != 0)
            {
                TemporaryGift.Image = ImageHelper.BitmapSourceToByteArray(userImageItems.SelectedItem.ToString());
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            nameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (Validation.GetHasError(nameTextBox))
            {
                MessageBox.Show("Please provide correct data");
            }
            else
            {
                if (IsEdited)
                {
                    CurrentGift.Image = TemporaryGift.Image;
                    CurrentGift.Name = TemporaryGift.Name;
                    GiftRepository.Save();
                }
                else
                {
                    GiftRepository.Add(TemporaryGift);
                }
                this.Close();
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox text = sender as TextBox;
                BindingOperations.GetBindingExpression(text, TextBox.TextProperty).UpdateSource();
            }
        }
    }
}
