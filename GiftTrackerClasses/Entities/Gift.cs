using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftTrackerClasses
{
    public class Gift: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public byte[] Image { get; set; }
        [Required]
        public bool IsGiven { get; set; }
        public Person Owner { get; set; }
        public Occasion Occasion { get; set; }
    }
}
