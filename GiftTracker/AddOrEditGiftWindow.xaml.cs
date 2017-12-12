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
using System.Data.Entity;
using GiftTrackerClasses;

namespace GiftTracker
{
    /// <summary>
    /// Interaction logic for AddOrEditGiftWindow.xaml
    /// </summary>
    public partial class AddOrEditGiftWindow : Window
    {
        GTContext context;
        public AddOrEditGiftWindow(GTContext context)
        {
            InitializeComponent();
            this.context = context;
        }
    }
}
