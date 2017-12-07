using System;
using System.Collections.Generic;
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

namespace GiftTracker
{
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private WindowState storedWindowState = WindowState.Normal;

        public MainWindow()
        {
            InitializeComponent();
            SetNotifyIcon();

            List<Person> ppl = new List<Person>();
            List<Occasion> occ = new List<Occasion>();

            var Vasya = new Person
            {
                Name = "Vasya",
                Gifts = new List<Gift>(),
                Image = ImageHelper.BitmapSourceToByteArray(@"..\..\Images\gift.ico")
                
            };
            var Petya = new Person
            {
                Name = "Petya",
                Gifts = new List<Gift>(),
                Image = ImageHelper.BitmapSourceToByteArray(@"..\..\Images\gift.ico")
            };
            var NewYear = new Occasion
            {
                Name = "New Year",
                Image = ImageHelper.BitmapSourceToByteArray(@"..\..\Images\gift.ico")

            };
            var Birthday = new Occasion
            {
                Name = "Birthday",
                Image = ImageHelper.BitmapSourceToByteArray(@"..\..\Images\gift.ico")
            };
            var gfts = new List<Gift>
            {
                new Gift() { Occasion = NewYear, Owner = Vasya, Name="1"},
                new Gift() {Occasion = NewYear, Owner = Petya, Name="1"},
                new Gift() {Occasion = NewYear, Owner = Vasya, Name="2"},
                new Gift() {Occasion = Birthday, Owner = Vasya, Name="3"}
            };

            Petya.Gifts = gfts.Where(x => x.Owner == Petya).ToList();
            Petya.Occasions = occ;
            Vasya.Gifts = gfts.Where(x => x.Owner == Vasya).ToList(); ;
            Vasya.Occasions = occ;
            ppl.Add(Petya);
            ppl.Add(Vasya);
            occ.Add(Birthday);
            occ.Add(NewYear);
            peopleDataGrid.DataContext = ppl;
            peopleDataGrid.ItemsSource = ppl;
            occasionsDataGrid.DataContext = occ;
            occasionsDataGrid.ItemsSource = occ;
            peopleDataGrid.MouseLeftButtonUp += DataGrid_MouseLeftButtonUp<Person>;
            occasionsDataGrid.MouseLeftButtonUp += DataGrid_MouseLeftButtonUp<Occasion>;

        }

        void DataGrid_MouseLeftButtonUp<T>(object sender, MouseButtonEventArgs e)
        {
            T item = (T)((System.Windows.Controls.DataGrid)sender).SelectedItem;
            ((System.Windows.Controls.DataGrid)sender).UnselectAll();
            if (item != null)
            {
                new DetailsWindow(item).ShowDialog();
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
                    new AddOrEditPersonWindow().ShowDialog();
                    break;
                case "Occasion":
                    new AddOrEditOccasionWindow().ShowDialog();
                    break;
                default:
                    break;
            }
        }
    }

    
}
