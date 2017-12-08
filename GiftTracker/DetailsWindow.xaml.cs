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
        Context context;

        public DetailsWindow(object item, Context context)
        {
            InitializeComponent();
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.context = context;

            if (item is Person person)
            {
                CurrentPerson = person;
            } else if (item is Occasion occasion)
            {
                CurrentOccasion = occasion;
            }
            Load();

        }

        private void Load()
        {
            List<Gift> gifts = new List<Gift>();
            if (CurrentPerson != null)
            {
                this.DataContext = CurrentPerson;
                gifts = CurrentPerson.Gifts != null ? CurrentPerson.Gifts.ToList() : new List<Gift>();
                ListCollectionView lcv = new ListCollectionView(CurrentPerson.Gifts.ToList());
                lcv.GroupDescriptions.Add(new PropertyGroupDescription("Occasion.Name"));
                detailsDataGrid.ItemsSource = lcv;
                detailsDataGrid.UnselectAll();

            }

            else if (CurrentOccasion != null)
            {
                this.DataContext = CurrentOccasion;
                gifts = CurrentOccasion.Gifts != null ? CurrentOccasion.Gifts.ToList() : new List<Gift>();
                ListCollectionView lcv = new ListCollectionView(gifts);
                lcv.GroupDescriptions.Add(new PropertyGroupDescription("Person.Name"));
                detailsDataGrid.ItemsSource = lcv;
                detailsDataGrid.UnselectAll();
            }

            if (gifts.Count == 0)
            {
                detailsDataGrid.Height = 50;
            }
        }

        private void PlusButtonClicked(object sender, RoutedEventArgs e)
        {

            new AddOrEditGiftWindow(context).ShowDialog();
        
        }
    }
}
