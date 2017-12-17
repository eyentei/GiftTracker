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
using GiftTracker.Properties;
using System.Windows.Threading;

namespace GiftTracker
{
    public partial class MainWindow : Window
    {
        GTContext Context { get; set; }
        System.Windows.Forms.NotifyIcon NotifyIcon { get; set; }
        WindowState StoredWindowState { get; set; } = WindowState.Normal;
        ObservableCollection<Occasion> Occasions { get; set; }
        public GTContext Сontext { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            SetNotifyIcon();
            Context = new GTContext();
            Occasions = new GTRepository<Occasion>(Context).GetAll();
            Occasions.CollectionChanged += Occasions_CollectionChanged;
            occasionsDataGrid.ItemsSource = Occasions;

            peopleDataGrid.ItemsSource = new GTRepository<Person>(Context).GetAll();

            var gifts = new GTRepository<Gift>(Context).GetAll();

            //a way to filter an observable collection
            ICollectionView giftsView = CollectionViewSource.GetDefaultView(gifts);
            giftsView.Filter = (o) =>
            {
                Gift gift = (Gift)o;
                return gift.Owner == null && gift.Occasion == null;
            };

            giftsDataGrid.ItemsSource = giftsView;

            notificationsComboBox.ItemsSource = new List<string>() { "day", "week", "month" };

            peopleDataGrid.MouseLeftButtonUp += DataGrid_MouseLeftButtonUp<Person>;
            occasionsDataGrid.MouseLeftButtonUp += DataGrid_MouseLeftButtonUp<Occasion>;
            giftsDataGrid.MouseLeftButtonUp += DataGrid_MouseLeftButtonUp<Gift>;
        }

        private void Occasions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var occasion in e.NewItems)
                {
                    SetTimer((Occasion)occasion);
                }
            }
        }

        private void Occasion_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Date")
            {
                SetTimer((Occasion)sender);
            }
        }

        void DataGrid_MouseLeftButtonUp<T>(object sender, MouseButtonEventArgs e)
        {
            T item = (T)((DataGrid)sender).SelectedItem;           
            if (item != null)
            {
                if (item is Person || item is Occasion)
                    new DetailsWindow(item, Context).ShowDialog();
                else if (item is Gift)
                {
                    new AddOrEditGiftWindow(Context, item as Gift).ShowDialog();
                    ICollectionView giftsView = CollectionViewSource.GetDefaultView(new GTRepository<Gift>(Context).GetAll());
                    giftsView.Filter = (o) =>
                    {
                        Gift gift = (Gift)o;
                        return gift.Owner == null && gift.Occasion == null;
                    };

                    giftsDataGrid.ItemsSource = giftsView;
                }
            }
            ((DataGrid)sender).UnselectAll();
        }

        private void SetNotifyIcon()
        {
            NotifyIcon = new System.Windows.Forms.NotifyIcon
            {
                BalloonTipTitle = "GiftTracker",
                Text = "Gift tracking app",
                Icon = new System.Drawing.Icon(@"..\..\Images\gift.ico")
            };

            NotifyIcon.Click += (sender, e) =>
            {
                Show();
                WindowState = StoredWindowState;
            };
            NotifyIcon.Visible = true;
        }

        private void OnClose(object sender, CancelEventArgs args)
        {
            NotifyIcon.Visible = false;
            NotifyIcon.Dispose();
            NotifyIcon = null;
        }

        private void OnStateChanged(object sender, EventArgs args)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
                if (NotifyIcon != null)
                {
                    var occasion = GetClosestOccasion();
                    NotifyIcon.BalloonTipText = $"Closest occasion is {occasion.Name} in {(occasion.ClosestDate-DateTime.Now.Date).TotalDays} days";
                    NotifyIcon.ShowBalloonTip(200);
                }
            }
            else
            {
                StoredWindowState = WindowState;
            }
        }

        private Occasion GetClosestOccasion()
        {
            return Occasions.OrderBy(x => x.ClosestDate).FirstOrDefault(x => x.ClosestDate >= DateTime.Now.Date);
        }

        private void ShowNotification(Occasion occasion)
        {
            if (NotifyIcon != null)
            {
                if (DateTime.Now.Date == occasion.ClosestDate)
                {
                    NotifyIcon.BalloonTipText = $"{occasion.Name} is today!";
                    NotifyIcon.ShowBalloonTip(200);
                    occasion.Timer.Dispose();
                } else
                {
                    NotifyIcon.BalloonTipText = $"{occasion.Name} is in {(occasion.ClosestDate - DateTime.Now.Date).TotalDays} days";
                    NotifyIcon.ShowBalloonTip(200);
                }
                              
            } 
            
        }

        private void SetTimer(Occasion occasion)
        {
            var now = DateTime.Now.Date;
            var notifyOn = GetNotificationDate(occasion.ClosestDate);
            var dueTime = (notifyOn >= now) ? notifyOn - now : TimeSpan.Zero;
           if (occasion.ClosestDate >= now && dueTime < TimeSpan.FromDays(50))
            {
                occasion.Timer = new Timer(_ => ShowNotification(occasion), null, dueTime, TimeSpan.FromDays(1));
            }
        }
      

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (notificationsCheckBox.IsChecked ?? false)
            {
                Settings.Default.NotifyBefore = notificationsComboBox.SelectedIndex.ToString();
                foreach (var occasion in Occasions)
                {
                    SetTimer(occasion);
                }  
            }
            else
            {
                Settings.Default.NotifyBefore = "";
                foreach (var occasion in Occasions)
                {
                    occasion.Timer.Dispose();
                }

            }
            Settings.Default.Save();
            savedLabel.Visibility = Visibility.Visible;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag.ToString();
            switch (tag)
            {
                case "Person":
                    new AddOrEditPersonWindow(Context).ShowDialog();
                    break;
                case "Occasion":
                    new AddOrEditOccasionWindow(Context).ShowDialog();
                    break;
                case "Gift":
                    new AddOrEditGiftWindow(Context).ShowDialog();
                    break;
                default:
                    break;
            }
        }

        private DateTime GetNotificationDate(DateTime date)
        {
            var nb = Settings.Default.NotifyBefore;
            switch (nb)
            {
                case "0":
                    return date.AddDays(-1);
                case "1":
                    return date.AddDays(-7);
                case "2":
                    return date.AddMonths(-1);
                default:
                    return date;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var notifyBefore = Settings.Default.NotifyBefore;
            foreach (var occasion in Occasions)
            {
                occasion.PropertyChanged += Occasion_PropertyChanged;
            }
            if (!string.IsNullOrEmpty(notifyBefore))
            {
                foreach (var occasion in Occasions)
                {
                    SetTimer(occasion);
                }
                notificationsCheckBox.IsChecked = true;
                notificationsComboBox.SelectedIndex = int.Parse(notifyBefore);
            }
        }
    }


}
