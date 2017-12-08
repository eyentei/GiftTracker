using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace GiftTrackerClasses
{
    public class DataBaseCreation : DbMigrationsConfiguration<Context>
    {
        Context context;

        public DataBaseCreation(Context context)
        {
            AutomaticMigrationsEnabled = true;
            this.context = context;
        }

        public void CreatePerson(List<Person> ppl)
        {
            if (context.Person.Any())
                return;
            foreach (Person p in ppl)
            {
                context.Person.Add(p);
            }
            context.SaveChanges();
        }

        public void CreateOccasion(List<Occasion> occ)
        {
            if (context.Occasion.Any())
                return;
            foreach (Occasion o in occ)
            {
                context.Occasion.Add(o);
            }
            context.SaveChanges();
        }

        public void CreateGifts(List<Gift> gfts)
        {
            if (context.Gift.Any())
                return;
            foreach (Gift g in gfts)
            {
                context.Gift.Add(g);
            }
            context.SaveChanges();
        }
    }
}
