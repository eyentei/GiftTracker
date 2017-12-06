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
using System.Windows.Forms;
using System.ComponentModel;

namespace GiftTracker
{
    public partial class MainWindow : Window
    {
        private NotifyIcon notifyIcon;
        private WindowState storedWindowState = WindowState.Normal;

        public MainWindow()
        {
            InitializeComponent();
            notifyIcon = new NotifyIcon
            {
                BalloonTipText = "The app has been minimised. Click the tray icon to show.",
                BalloonTipTitle = "GiftTracker",
                Text = "Gift tracking app",
                Icon = new System.Drawing.Icon(@"..\..\gift.ico")
            };

            notifyIcon.Click += NotifyIcon_Click;



            List<Person> ppl = new List<Person>();
            List<Occasion> occ = new List<Occasion>();

            var Vasya = new Person
            {
                Name = "Vasya",
                Image = BitmapSourceToByteArray(@"cat.JPG"),
                Gifts = new List<Gift>()
            };
            var Petya = new Person
            {
                Name = "Petya",
                Image = BitmapSourceToByteArray(@"cat.JPG"),
                Gifts = new List<Gift>()
            };
            var NewYear = new Occasion
            {
                Image = BitmapSourceToByteArray(@"cat.JPG"),
                Name = "New Year"

            };
            var Birthday = new Occasion
            {
                Name = "Birthday"
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

        private byte[] BitmapSourceToByteArray(string image)
        {
            BitmapSource bSource = new BitmapImage(new Uri(image, UriKind.Relative));
            using (var stream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bSource));
                encoder.Save(stream);
                return stream.ToArray();
            }
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = storedWindowState;
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
    }

    
}
