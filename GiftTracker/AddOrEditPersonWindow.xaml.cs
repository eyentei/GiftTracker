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
using System.Collections.ObjectModel;

namespace GiftTracker
{
    public partial class AddOrEditPersonWindow : Window
    {
        Person CurrentPerson { get; set; }
        Person TemporaryPerson { get; set; }
        GTRepository<Person> PeopleRepository { get; set; }
        GTRepository<Occasion> OccasionsRepository { get; set; }
        bool IsEdited { get; set; }
        public AddOrEditPersonWindow(GTContext context, Person person = null)
        {
            InitializeComponent();
            PeopleRepository = new GTRepository<Person>(context);
            OccasionsRepository = new GTRepository<Occasion>(context);
            userImageItems.ItemsSource = Directory.EnumerateFiles(@"..\..\Images\DefaultUserImages", "*.png");

            if (person == null)
            {
                IsEdited = false;
                TemporaryPerson = new Person();
                userImageItems.SelectedIndex = 0;
                userImageItems.Focus();
            }
            else
            {
                CurrentPerson = person;
                TemporaryPerson = new Person() { Image = person.Image, Name = person.Name, Birthday = person.Birthday };
                IsEdited = true;
            }
            this.DataContext = TemporaryPerson;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = OpenFileDialogHelper.OpenImageFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                userImageItems.UnselectAll();
                string fileName = dialog.FileName;
                TemporaryPerson.Image = ImageHelper.BitmapSourceToByteArray(fileName);
            }
        }

        private void UserImageItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (userImageItems.SelectedItems.Count != 0)
            {
                TemporaryPerson.Image = ImageHelper.BitmapSourceToByteArray(userImageItems.SelectedItem.ToString());
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            nameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            birthdayDatePicker.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();

            if (Validation.GetHasError(nameTextBox) || Validation.GetHasError(birthdayDatePicker))
            {
                MessageBox.Show("Please provide correct data");
            }
            else
            {
                if (IsEdited)
                {
                    CurrentPerson.Image = TemporaryPerson.Image;
                    CurrentPerson.Name = TemporaryPerson.Name;
                    CurrentPerson.Birthday = TemporaryPerson.Birthday;
                    PeopleRepository.Save();
                }
                else
                {
                    var defaultOccasions = new ObservableCollection<Occasion>() {
                        OccasionsRepository.GetAll().SingleOrDefault(x => x.Name == "New Year"),
                        new Occasion {
                            Date = TemporaryPerson.Birthday,
                            Image = ImageHelper.BitmapSourceToByteArray(@"..\..\Images\DefaultOccasionImages\gift.png"),
                            Name = TemporaryPerson.Name + "'s Birthday",
                            IsForEveryone = false
                        } };
                    TemporaryPerson.Occasions = defaultOccasions;
                    PeopleRepository.Add(TemporaryPerson);
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
