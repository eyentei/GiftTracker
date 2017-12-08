using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftTrackerClasses
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public byte[] Image { get; set; }
        public ICollection<Occasion> Occasions { get; set; }
        public ICollection<Gift> Gifts { get; set; }
    }

    

    
}
