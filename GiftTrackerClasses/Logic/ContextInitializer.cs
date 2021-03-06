﻿using System;
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

            var newYear = new Occasion
            {
                Name = "New Year",
                Date = new DateTime(1753, 12, 31),
                Image = ImageHelper.BitmapSourceToByteArray(@"..\..\Images\DefaultOccasionImages\christmas.png"),

            };

            var ppl = new List<Person>() {
                new Person
            {
                Name = "Masha",
                Birthday = new DateTime(1995, 12, 20),
                Gifts = new ObservableCollection<Gift>(),
                Image = ImageHelper.BitmapSourceToByteArray(@"..\..\Images\DefaultUserImages\cat_icon.png"),
                Occasions = new ObservableCollection<Occasion>()

            },
            new Person
            {
                Name = "Petya Petin",
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
                });
            }
            
            peopleRepository.AddRange(ppl);
            peopleRepository.Save();
        }
    }
}
