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
    public partial class AddOrEditOccasionWindow : Window
    {
        Occasion CurrentOccasion { get; set; }
        Occasion TemporaryOccasion { get; set; }
        GTRepository<Occasion> OccasionRepository { get; set; }
        GTRepository<Person> PeopleRepository { get; set; }
        ObservableCollection<Person> People { get; set; }
        bool IsEdited { get; set; }

        public AddOrEditOccasionWindow(GTContext context, Occasion occasion = null)
        {
            InitializeComponent();
            OccasionRepository = new GTRepository<Occasion>(context);
            PeopleRepository = new GTRepository<Person>(context);
            People = PeopleRepository.GetAll();
            severalPeopleListBox.ItemsSource = People;
            userImageItems.ItemsSource = Directory.EnumerateFiles(@"..\..\Images\DefaultOccasionImages", "*.png");

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
                TemporaryOccasion = new Occasion() { Image = occasion.Image, Name = occasion.Name, Date = occasion.Date};
                if (CurrentOccasion.People.Count < People.Count)
                {
                    severalPeopleCheckBox.IsChecked = true;
                    foreach (var person in People)
                    {
                        if (CurrentOccasion.People.Contains(person)) {
                            severalPeopleListBox.SelectedItems.Add(person);
                        }
                    }
                }         
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


            if (Validation.GetHasError(nameTextBox))
            {
                foreach (var error in Validation.GetErrors(nameTextBox))
                {
                    MessageBox.Show(error.ErrorContent.ToString());
                }
                
            }
            if (Validation.GetHasError(dateDatePicker))
            {
                foreach (var error in Validation.GetErrors(dateDatePicker))
                {
                    MessageBox.Show(error.ErrorContent.ToString());
                }
            }
            else
            {
                if (IsEdited)
                {
                    CurrentOccasion.Image = TemporaryOccasion.Image;
                    CurrentOccasion.Name = TemporaryOccasion.Name;
                    CurrentOccasion.Date = TemporaryOccasion.Date;
                    if (severalPeopleCheckBox.IsChecked ?? false)
                    {
                        CurrentOccasion.People = new ObservableCollection<Person>(severalPeopleListBox.SelectedItems.Cast<Person>());
                    }
                    else
                    {
                        CurrentOccasion.People = PeopleRepository.GetAll();
                    }
                    OccasionRepository.Save();
                }
                else
                {
                    if (severalPeopleCheckBox.IsChecked ?? false)
                    {
                        TemporaryOccasion.People = new ObservableCollection<Person>(severalPeopleListBox.SelectedItems.Cast<Person>());
                    }
                    else
                    {
                        TemporaryOccasion.People = PeopleRepository.GetAll();
                    }
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
