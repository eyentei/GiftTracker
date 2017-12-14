using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftTrackerClasses
{
    class ContextInitializer: DropCreateDatabaseIfModelChanges<GTContext>
    {
        protected override void Seed(GTContext context)
        {
            var peopleRepository = new GTRepository<Person>(context);
            var giftRepository = new GTRepository<Gift>(context);
            var occasionRepository = new GTRepository<Occasion>(context);
            var generator = new DataGenerator(context);

            var newYear = new Occasion
            {
                Name = "New Year",
                Date = new DateTime(9999, 01, 01),
                Image = ImageHelper.BitmapSourceToByteArray(@"..\..\Images\DefaultOccasionImages\christmas.png"),
                IsPersonal = false

            };

            var ppl = new List<Person>() {
                new Person
            {
                Name = "Vasya",
                Birthday = new DateTime(1995, 11, 04),
                Gifts = new ObservableCollection<Gift>(),
                Image = ImageHelper.BitmapSourceToByteArray(@"..\..\Images\DefaultUserImages\cat_icon.png"),
                Occasions = new ObservableCollection<Occasion>()

            },
            new Person
            {
                Name = "Petya",
                Birthday = new DateTime(1996, 04, 14),
                Gifts = new ObservableCollection<Gift>(),
                Image = ImageHelper.BitmapSourceToByteArray(@"..\..\Images\DefaultUserImages\male_icon.png"),
                Occasions = new ObservableCollection<Occasion>()
            }};

            foreach (var person in ppl)
            {
                person.Occasions.Add(newYear);
                person.Occasions.Add(new Occasion {
                    Date = person.Birthday,
                    Name = person.Name + "'s Birthday",
                    Image = ImageHelper.BitmapSourceToByteArray(@"..\..\Images\DefaultOccasionImages\gift.png"),
                    IsPersonal = true
                });
            }
            
            peopleRepository.AddRange(ppl);
            peopleRepository.Save();
        }
    }
}
