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
    public class Occasion : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsPersonal { get; set; } // for one person or for all
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public byte[] Image { get; set; }
        public virtual ObservableCollection<Person> People { get; set; }
        public virtual ObservableCollection<Gift> Gifts { get; set; }
    }
}
