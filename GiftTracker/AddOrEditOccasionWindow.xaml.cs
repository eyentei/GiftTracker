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
using GiftTrackerClasses;

namespace GiftTracker
{
    /// <summary>
    /// Interaction logic for AddOrEditOccasionWindow.xaml
    /// </summary>
    public partial class AddOrEditOccasionWindow : Window
    {
        Context context;
        public AddOrEditOccasionWindow(Context context)
        {
            InitializeComponent();
            this.context = context;
        }
    }
}
