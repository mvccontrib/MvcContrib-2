using System.Collections.Generic;
using MvcContrib.Samples.UI.Models;

namespace MvcContrib.Samples.UI.Services
{
    public class PersonService
    {
        private static Person cachedPerson = new Person
        {
            Gender = "M", 
            Name = "Jeremy", 
            Roles = new List<int> {1, 2},
            Father = new Parent {Name ="Jim"},
            Mother = new Parent { Name = "Joan" }
        };

        public static Person GetPerson()
        {
            return cachedPerson;
        }

        public static void SavePerson(Person person)
        {
            cachedPerson = person;
        }
    }
}