using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftTrackerClasses
{
    public class Person : INotifyPropertyChanged
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime? Birthday { get; set; }
        [Required]
        public byte[] Image { get; set; }
        public virtual ObservableCollection<Occasion> Occasions { get; set; } = new ObservableCollection<Occasion>();
        public virtual ObservableCollection<Gift> Gifts { get; set; } = new ObservableCollection<Gift>();

        public event PropertyChangedEventHandler PropertyChanged;
    }




}
