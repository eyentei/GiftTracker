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
            var peopleRepo = new GTRepository<Person>(context);
            var occasionsRepo = new GTRepository<Occasion>(context);
            var giftsRepo = new GTRepository<Gift>(context);

            var people = peopleRepo.GetAll();
            var occasions = occasionsRepo.GetAll();
            var gifts = giftsRepo.GetAll();

            peopleDataGrid.DataContext = people;
            peopleDataGrid.ItemsSource = people;
            occasionsDataGrid.DataContext = occasions;
            occasionsDataGrid.ItemsSource = occasions;
            giftsDataGrid.DataContext = gifts.Where(g => g.Owner == null && g.Occasion == null);
            giftsDataGrid.ItemsSource = gifts.Where(g => g.Owner == null && g.Occasion == null);

            peopleDataGrid.MouseLeftButtonUp += DataGrid_MouseLeftButtonUp<Person>;
            occasionsDataGrid.MouseLeftButtonUp += DataGrid_MouseLeftButtonUp<Occasion>;
            giftsDataGrid.MouseLeftButtonUp += DataGrid_MouseLeftButtonUp<Gift>;
            context.SaveChanges();
        }

        void DataGrid_MouseLeftButtonUp<T>(object sender, MouseButtonEventArgs e)
        {
            T item = (T)((DataGrid)sender).SelectedItem;
            ((DataGrid)sender).UnselectAll();
            if (item != null)
            {
                new DetailsWindow(item, context).ShowDialog();
            }
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
