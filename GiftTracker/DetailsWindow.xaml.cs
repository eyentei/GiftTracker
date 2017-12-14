using GiftTrackerClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GiftTracker
{
    public partial class DetailsWindow : Window
    {
        Person CurrentPerson { get; set; }
        Occasion CurrentOccasion { get; set; }
        Gift CurrentGift { get; set; }
        GTContext Context { get; set; }
        GTRepository<Person> PeopleRepository { get; set; }
        GTRepository<Occasion> OccasionsRepository { get; set; }
        GTRepository<Gift> GiftsRepository { get; set; }

        public DetailsWindow(object item, GTContext context)
        {
            InitializeComponent();
            this.SizeToContent = SizeToContent.WidthAndHeight;
            Context = context;
            PeopleRepository = new GTRepository<Person>(context);
            OccasionsRepository = new GTRepository<Occasion>(context);
            GiftsRepository = new GTRepository<Gift>(context);

            if (item is Person person)
            {
                CurrentPerson = person;
            }
            else if (item is Occasion occasion)
            {
                CurrentOccasion = occasion;
                if (CurrentOccasion.Name == "New Year")
                {
                    deleteButton.Visibility = Visibility.Hidden;
                    editButton.Visibility = Visibility.Hidden;
                } else
                {
                    deleteButton.Content = "Delete occasion";
                    editButton.Content = "Edit occasion";
                    deleteButton.Tag = "Occasion";
                    editButton.Tag = "Occasion";
                }
                
            }
            else if (item is Gift gift)
            {
                CurrentGift = gift;
                deleteButton.Content = "Delete gift";
                editButton.Content = "Edit gift";
                deleteButton.Tag = "Gift";
                editButton.Tag = "Gift";
            }
            Load();

        }

        private void Load()
        {
            if (CurrentPerson != null)
            {
                this.DataContext = CurrentPerson;
                var gifts = CurrentPerson.Gifts ?? new ObservableCollection<Gift>();
                var occasions = CurrentPerson.Occasions;

                ListCollectionView lcv = new ListCollectionView(gifts);

                var groupDescription = new PropertyGroupDescription("Occasion.Name");
                foreach (var occasion in occasions)
                    groupDescription.GroupNames.Add(occasion.Name);

                lcv.GroupDescriptions.Add(groupDescription);
                detailsDataGrid.ItemsSource = lcv;
            }
            else if (CurrentOccasion != null)
            {
                this.DataContext = CurrentOccasion;
                var gifts = CurrentOccasion.Gifts ?? new ObservableCollection<Gift>();
                var people = CurrentOccasion.People;

                ListCollectionView lcv = new ListCollectionView(gifts);

                var groupDescription = new PropertyGroupDescription("Owner.Name");
                foreach (var person in people)
                    groupDescription.GroupNames.Add(person.Name);

                lcv.GroupDescriptions.Add(groupDescription);
                detailsDataGrid.ItemsSource = lcv;

            }
            else if (CurrentGift != null)
            {
                this.DataContext = CurrentGift;
            }
            detailsDataGrid.UnselectAll();

        }
        private void PlusButtonClicked(object sender, RoutedEventArgs e)
        {
            var groupName = (((sender as Button).TemplatedParent as GroupItem).Content as CollectionViewGroup).Name.ToString();
            // what is written next to a plus button that was clicked

            if (CurrentPerson != null)
            {
                var currentOccasion = OccasionsRepository.GetAll().SingleOrDefault(x => x.Name == groupName && x.People.Contains(CurrentPerson));
                new AddOrEditGiftWindow(Context, currentOccasion, CurrentPerson).ShowDialog();
            }
            else if (CurrentOccasion != null)
            {
                var currentPerson = PeopleRepository.GetAll().SingleOrDefault(x => x.Name == groupName && x.Occasions.Contains(CurrentOccasion));
                new AddOrEditGiftWindow(Context, CurrentOccasion, currentPerson).ShowDialog();
            }
        }
        private void EditButtonClicked(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag.ToString();
            switch (tag)
            {
                case "Person":
                    new AddOrEditPersonWindow(Context, CurrentPerson).ShowDialog();
                    break;
                case "Occasion":
                    new AddOrEditOccasionWindow(Context, CurrentOccasion).ShowDialog();
                    break;
                case "Gift":
                    new AddOrEditGiftWindow(Context, CurrentGift).ShowDialog();
                    break;
                default:
                    break;
            }
        }
        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (CurrentPerson != null)
            {
                OccasionsRepository.DeleteRange(CurrentPerson.Occasions.Where(x => x.IsPersonal));
                GiftsRepository.DeleteRange(CurrentPerson.Gifts);
                PeopleRepository.Delete(CurrentPerson);
            }
            else if (CurrentOccasion != null)
            {
                GiftsRepository.DeleteRange(CurrentOccasion.Gifts);
                OccasionsRepository.Delete(CurrentOccasion);
            }
            else if (CurrentGift != null)
            {
                GiftsRepository.Delete(CurrentGift);
            }

            this.Close();
        }

        private void DetailsDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CurrentGift = (Gift)detailsDataGrid.SelectedItem;
            if (CurrentGift != null)
            {
                new AddOrEditGiftWindow(Context, CurrentGift).ShowDialog();
            }
        }
    }
}
