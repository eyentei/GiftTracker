using GiftTrackerClasses;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddOrEditOccasionWindow.xaml
    /// </summary>
    public partial class AddOrEditOccasionWindow : Window
    {
        Occasion CurrentOccasion { get; set; }
        Occasion TemporaryOccasion { get; set; }
        GTRepository<Occasion> OccasionRepository { get; set; }
        GTRepository<Person> PeopleRepository { get; set; }
        bool IsEdited { get; set; }
        public AddOrEditOccasionWindow(GTContext context, Occasion occasion = null)
        {
            InitializeComponent();
            OccasionRepository = new GTRepository<Occasion>(context);
            PeopleRepository = new GTRepository<Person>(context);
            var ppl = PeopleRepository.GetAll();
            userImageItems.ItemsSource = Directory.EnumerateFiles(@"..\..\Images\DefaultOccasionImages", "*.png");
            personComboBox.ItemsSource = ppl;

            if (occasion == null)
            {
                IsEdited = false;
                TemporaryOccasion = new Occasion();
                userImageItems.SelectedIndex = 0;
                userImageItems.Focus();
            }
            else
            {
                CurrentOccasion = occasion;
                TemporaryOccasion = new Occasion() { Image = occasion.Image, Name = occasion.Name, Date = occasion.Date };
                IsEdited = true;
            }
            this.DataContext = TemporaryOccasion;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = OpenFileDialogHelper.OpenImageFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                userImageItems.UnselectAll();
                string fileName = dialog.FileName;
                TemporaryOccasion.Image = ImageHelper.BitmapSourceToByteArray(fileName);
            }
        }

        private void UserImageItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (userImageItems.SelectedItems.Count != 0)
            {
                TemporaryOccasion.Image = ImageHelper.BitmapSourceToByteArray(userImageItems.SelectedItem.ToString());
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            nameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            dateDatePicker.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();


            if (Validation.GetHasError(nameTextBox) || Validation.GetHasError(dateDatePicker))
            {
                MessageBox.Show("Please provide correct data");
            }
            else
            {
                if (IsEdited)
                {
                    CurrentOccasion.Image = TemporaryOccasion.Image;
                    CurrentOccasion.Name = TemporaryOccasion.Name;
                    CurrentOccasion.Date = TemporaryOccasion.Date;
                    OccasionRepository.Save();
                }
                else
                {
                    OccasionRepository.Add(TemporaryOccasion);
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
            if (sender is DatePicker)
            {
                DatePicker date = sender as DatePicker;
                BindingOperations.GetBindingExpression(date, DatePicker.SelectedDateProperty).UpdateSource();
            }

        }

    }
}
