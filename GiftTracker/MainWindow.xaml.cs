using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using GiftTrackerClasses;
using System.Data.Entity;
using System.Threading;


namespace GiftTracker
{
    public partial class MainWindow : Window
    {
        private GTContext context;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private WindowState storedWindowState = WindowState.Normal;

        public MainWindow()
        {
            InitializeComponent();
            SetNotifyIcon();
            context = new GTContext();

            var people = new GTRepository<Person>(context).GetAll();
            var occasions = new GTRepository<Occasion>(context).GetAll();
            var gifts = new GTRepository<Gift>(context).GetAll();

            peopleDataGrid.ItemsSource = people;
            occasionsDataGrid.ItemsSource = occasions;

            //a way to filter an observable collection
            ICollectionView giftsView = CollectionViewSource.GetDefaultView(gifts);
            giftsView.Filter = (o) =>
            {
                Gift gift = (Gift)o;
                return gift.Owner == null && gift.Occasion == null;
            };

            giftsDataGrid.ItemsSource = giftsView;

            peopleDataGrid.MouseLeftButtonUp += DataGrid_MouseLeftButtonUp<Person>;
            occasionsDataGrid.MouseLeftButtonUp += DataGrid_MouseLeftButtonUp<Occasion>;
            giftsDataGrid.MouseLeftButtonUp += DataGrid_MouseLeftButtonUp<Gift>;
            context.SaveChanges();
        }

        void DataGrid_MouseLeftButtonUp<T>(object sender, MouseButtonEventArgs e)
        {
            T item = (T)((DataGrid)sender).SelectedItem;           
            if (item != null)
            {
                if (item is Person || item is Occasion)
                    new DetailsWindow(item, context).ShowDialog();
                else if (item is Gift)
                {
                    new AddOrEditGiftWindow(context, (Gift)((DataGrid)sender).SelectedItem).ShowDialog();
                }
            }
            ((DataGrid)sender).UnselectAll();
        }

        private void SetNotifyIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                BalloonTipText = "The app has been minimised. Click the tray icon to show.",
                BalloonTipTitle = "GiftTracker",
                Text = "Gift tracking app",
                Icon = new System.Drawing.Icon(@"..\..\Images\gift.ico")
            };

            notifyIcon.Click += (sender, e) =>
            {
                Show();
                WindowState = storedWindowState;
            };


        }

        private void OnClose(object sender, CancelEventArgs args)
        {
            notifyIcon.Dispose();
            notifyIcon = null;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            if (notifyIcon != null)
            {
                notifyIcon.Visible = !IsVisible;
            }
        }

        private void OnStateChanged(object sender, EventArgs args)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
                if (notifyIcon != null)
                {
                    notifyIcon.ShowBalloonTip(200);
                }
            }
            else
            {
                storedWindowState = WindowState;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag.ToString();
            switch (tag)
            {
                case "Person":
                    new AddOrEditPersonWindow(context).ShowDialog();
                    break;
                case "Occasion":
                    new AddOrEditOccasionWindow(context).ShowDialog();
                    break;
                case "Gift":
                    new AddOrEditGiftWindow(context).ShowDialog();
                    break;
                default:
                    break;
            }
        }
    }


}
