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
        Person CurrentPerson { get; set; }
        Occasion CurrentOccasion { get; set; }
        Gift TemporaryGift { get; set; }
        GTRepository<Gift> GiftRepository { get; set; }
        bool IsEdited { get; set; }

        public AddOrEditGiftWindow(GTContext context, Gift gift = null) {

            Initialize(context);
            if (gift == null)
            {
                NewGift();
            }
            else
            {
                CurrentGift = gift;
                TemporaryGift = new Gift() { Image = gift.Image, Name = gift.Name, IsGiven = gift.IsGiven, Description = gift.Description };
                IsEdited = true;
            }
            this.DataContext = TemporaryGift;
        }
        public AddOrEditGiftWindow(GTContext context, Occasion occasion, Person person) {

            Initialize(context);
            CurrentPerson = person;
            CurrentOccasion = occasion;
            TemporaryGift = new Gift();
            NewGift();
            this.DataContext = TemporaryGift;
        }

        private void Initialize(GTContext context)
        {
            InitializeComponent();
            GiftRepository = new GTRepository<Gift>(context);
            giftImageItems.ItemsSource = Directory.EnumerateFiles(@"..\..\Images\DefaultGiftImages", "*.png");
        }

        private void NewGift()
        {
            IsEdited = false;
            giftImageItems.SelectedIndex = 0;
            giftImageItems.Focus();
        }
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = OpenFileDialogHelper.OpenImageFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                giftImageItems.UnselectAll();
                string fileName = dialog.FileName;
                TemporaryGift.Image = ImageHelper.BitmapSourceToByteArray(fileName);
            }
        }

        private void GiftImageItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (giftImageItems.SelectedItems.Count != 0)
            {
                TemporaryGift.Image = ImageHelper.BitmapSourceToByteArray(giftImageItems.SelectedItem.ToString());
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
                    CurrentGift.Description = TemporaryGift.Description;
                    CurrentGift.IsGiven = TemporaryGift.IsGiven;
                    GiftRepository.Save();
                }
                else
                {
                    TemporaryGift.Owner = CurrentPerson;
                    TemporaryGift.Occasion = CurrentOccasion;
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
