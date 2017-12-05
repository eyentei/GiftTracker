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

namespace GiftTracker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //testing...
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

            Petya.Gifts = gfts.Where(x=> x.Owner == Petya).ToList();
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


        private void DataGrid_MouseLeftButtonUp<T>(object sender, MouseButtonEventArgs e)
        {
            T item = (T)((DataGrid)sender).SelectedItem;
            ((DataGrid)sender).UnselectAll();
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


    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public ICollection<Occasion> Occasions { get; set; }
        public ICollection<Gift> Gifts { get; set; }
    }

    public class Occasion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public ICollection<Person> People {get; set;}
        public ICollection<Gift> Gifts { get; set; }
    }

    public class Gift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public Person Owner { get; set; }
        public Occasion Occasion { get; set; }
    }
}
