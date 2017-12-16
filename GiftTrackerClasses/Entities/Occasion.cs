using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
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
        public DateTime? Date { get; set; }
        [Required]
        public byte[] Image { get; set; }
        public virtual ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();
        public virtual ObservableCollection<Gift> Gifts { get; set; } = new ObservableCollection<Gift>();
        [NotMapped]
        public DateTime ClosestDate
        {
            get { return new DateTime( Date.Value.Year > DateTime.Now.Year ? Date.Value.Year : DateTime.Now.Year, Date.Value.Month, Date.Value.Day); }
        }
        [NotMapped]
        public Timer Timer { get; set; }
    }
}
