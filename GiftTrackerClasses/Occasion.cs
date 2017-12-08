using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftTrackerClasses
{
    public class Occasion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public byte[] Image { get; set; }
        public ICollection<Person> People { get; set; }
        public ICollection<Gift> Gifts { get; set; }
    }
}
