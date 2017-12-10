using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GiftTrackerClasses
{
    public class Context : DbContext
    {
        public DbSet<Gift> Gift { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Occasion> Occasion { get; set; }

        public Context() : base("GiftTracker_DB")
        {
            Database.SetInitializer<Context>(new DropCreateDatabaseIfModelChanges<Context>());
        }
    }
}
