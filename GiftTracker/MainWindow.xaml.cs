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
            var c = new Person
            {
                Image = BitmapSourceToByteArray(@"cat.JPG")
            };
            var cf = new Occasion
            {
                Image = BitmapSourceToByteArray(@"cat.JPG")
            };
            ppl.Add(c);
            occ.Add(cf);
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
            new PersonWindow(item).ShowDialog();
            ((DataGrid)sender).UnselectAll();
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
        public string Name { get; set; } = "Vasya";
        public byte[] Image { get; set; }
    }

    public class Occasion
    {
        public string Name { get; set; } = "New Year";
        public byte[] Image { get; set; }
    }
}
