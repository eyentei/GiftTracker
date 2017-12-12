using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GiftTrackerClasses
{
    public class GTContext : DbContext
    {
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Occasion> Occasions { get; set; }

        public GTContext() : base("GiftTracker_DB")
        {
            Database.SetInitializer<GTContext>(new ContextInitializer());
        }
    }
}
