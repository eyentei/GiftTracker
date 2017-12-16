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
        GTContext Context { get; set; }
        Gift TemporaryGift { get; set; }
        GTRepository<Gift> GiftRepository { get; set; }
        GTRepository<Occasion> OccasionRepository { get; set; }
        GTRepository<Person> PeopleRepository { get; set; }
        bool IsEdited { get; set; }
        bool IsPersonal { get; set; }

        public AddOrEditGiftWindow(GTContext context, Gift gift = null)
        {
            Initialize(context);
            if (gift == null)
            {
                TemporaryGift = new Gift();
                NewGift();
                IsPersonal = false;
                onePersonCheckBox.IsEnabled = true;
                deleteButton.IsEnabled = false;
            }
            else
            {
                CurrentGift = gift;
                TemporaryGift = new Gift() { Image = gift.Image, Name = gift.Name, WasGiven = gift.WasGiven, Description = gift.Description };
                IsEdited = true;
                if (CurrentGift.Owner != null && CurrentGift.Occasion != null)
                {
                    IsPersonal = true;
                    CurrentPerson = CurrentGift.Owner;
                    CurrentOccasion = CurrentGift.Occasion;
                }
                else
                {
                    IsPersonal = false;
                    onePersonCheckBox.IsEnabled = true;
                }
            }
            this.DataContext = TemporaryGift;
            
            OccasionRepository = new GTRepository<Occasion>(context);
            PeopleRepository = new GTRepository<Person>(context);            
            Context = context;

            if (CurrentGift != null && CurrentGift.WasGiven)
                hasBeenGivenCheckBox.IsChecked = true;
        }
        public AddOrEditGiftWindow(GTContext context, Occasion occasion, Person person)
        {
            Initialize(context);
            CurrentPerson = person;
            CurrentOccasion = occasion;
            IsPersonal = true;
            TemporaryGift = new Gift();
            NewGift();
            this.DataContext = TemporaryGift;
            personComboBox.IsEnabled = false;
            occasionComboBox.IsEnabled = false;
            onePersonCheckBox.IsEnabled = false;
            if (CurrentGift != null && CurrentGift.WasGiven)
                hasBeenGivenCheckBox.IsChecked = true;
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
                    if (IsPersonal)
                    {
                        CurrentGift.Owner = CurrentPerson;
                        CurrentGift.Occasion = CurrentOccasion;
                    }
                    CurrentGift.Image = TemporaryGift.Image;
                    CurrentGift.Name = TemporaryGift.Name;
                    CurrentGift.Description = TemporaryGift.Description;
                    CurrentGift.WasGiven = TemporaryGift.WasGiven;
                    GiftRepository.Save();
                }
                else
                {
                    if (IsPersonal)
                    {
                        TemporaryGift.Owner = CurrentPerson;
                        TemporaryGift.Occasion = CurrentOccasion;
                    }
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

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentGift != null)
            {
                GiftRepository.Delete(CurrentGift);
                this.Close();
            }
        }

        private void personComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsPersonal)
            {
                foreach (Person person in PeopleRepository.GetAll())
                {
                    if (person == personComboBox.SelectedItem)
                    {
                        CurrentPerson = person;
                    }
                }
                occasionComboBox.ItemsSource = new ListCollectionView(CurrentPerson.Occasions);
            }
        }

        private void onePersonCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            personComboBox.IsEnabled = true;
            occasionComboBox.IsEnabled = true;
            IsPersonal = true;
            var ppl = PeopleRepository.GetAll();
            personComboBox.ItemsSource = new ListCollectionView(ppl);
        }

        private void occasionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsPersonal)
            {
                foreach (Occasion occasion in OccasionRepository.GetAll())
                {
                    if (occasion == occasionComboBox.SelectedItem)
                    {
                        CurrentOccasion = occasion;
                    }
                }
            }
        }

        private void onePersonCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            IsPersonal = false;
            personComboBox.IsEnabled = false;
            occasionComboBox.IsEnabled = false;
            CurrentPerson = null;
            CurrentOccasion = null;
        }

        private void hasBeenGivenCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TemporaryGift.WasGiven = true;
        }

        private void hasBeenGivenCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TemporaryGift.WasGiven = false;
        }
    }
}
