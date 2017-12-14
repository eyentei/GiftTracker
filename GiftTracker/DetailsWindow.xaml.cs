using GiftTrackerClasses;
using System;
using System.Collections.Generic;
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
        GTContext Context { get; set; }
        GTRepository<Person> PeopleRepository { get; set; }
        GTRepository<Occasion> OccasionsRepository { get; set; }

        public DetailsWindow(object item, GTContext context)
        {
            InitializeComponent();
            this.SizeToContent = SizeToContent.WidthAndHeight;
            Context = context;
            PeopleRepository = new GTRepository<Person>(context);
            OccasionsRepository = new GTRepository<Occasion>(context);

            if (item is Person person)
            {
                CurrentPerson = person;
            }
            else if (item is Occasion occasion)
            {
                CurrentOccasion = occasion;
                deleteButton.Content = "Delete occasion";
                editButton.Content = "Edit occasion";
            }
            Load();

        }

        private void Load()
        {        
            if (CurrentPerson != null)
            {
                this.DataContext = CurrentPerson;
                var gifts = CurrentPerson.Gifts;
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
                var gifts = CurrentOccasion.Gifts;
                var people = CurrentOccasion.People;
                ListCollectionView lcv = new ListCollectionView(gifts);

                var groupDescription = new PropertyGroupDescription("Owner.Name");
                foreach (var person in people)
                    groupDescription.GroupNames.Add(person.Name);

                lcv.GroupDescriptions.Add(groupDescription);
                detailsDataGrid.ItemsSource = lcv;
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
            new AddOrEditPersonWindow(Context, CurrentPerson).ShowDialog();
        }
        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (CurrentPerson != null)
            {

                foreach (var occasion in CurrentPerson.Occasions.ToList())
                {
                    if (occasion.IsPersonal)
                    {
                        OccasionsRepository.Delete(occasion);
                    }
                }
                PeopleRepository.Delete(CurrentPerson);
            }
            else if (CurrentOccasion != null)
            {
                // примерно то же, но  для события
            }
            
               this.Close();
        }
    }
}
