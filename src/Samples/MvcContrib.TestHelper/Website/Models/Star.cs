using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.TestHelper.Sample.Controllers;

namespace MvcContrib.TestHelper.Sample.Models
{
    public class Star
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public string NearbyLink { get; set; }
    }

    public static class StarDatabase
    {
        public static List<Star> GetStars()
        {
            List<Star> stars = new List<Star>();
            stars.Add(new Star { ID = 1, Name = "Beta Brahe", Distance = 44.2f });
            stars.Add(new Star { ID = 2, Name = "Alpha Brahe", Distance = 34.9f });
            stars.Add(new Star { ID = 3, Name = "Delta Brahe", Distance = 46.1f });
            stars.Add(new Star { ID = 4, Name = "Zetha Brahe", Distance = 47.4f });
            stars.Add(new Star { ID = 5, Name = "Theta Brahe", Distance = 48.6f });
            return stars;
        }

        public static List<Star> GetStarsAndLinks()
        {
            
            List<Star> stars = new List<Star>();
            stars.Add(new Star
                          {
                              ID = 1,
                              Name = "Beta Brahe",
                              Distance = 44.2f,
                              NearbyLink = ""
                          });

            stars.Add(new Star { ID = 2, Name = "Alpha Brahe", Distance = 34.9f, NearbyLink = "" });
            stars.Add(new Star { ID = 3, Name = "Delta Brahe", Distance = 46.1f, NearbyLink = "" });
            stars.Add(new Star { ID = 4, Name = "Zetha Brahe", Distance = 47.4f, NearbyLink = "" });
            stars.Add(new Star { ID = 5, Name = "Theta Brahe", Distance = 48.6f, NearbyLink = "" });
            return stars;
        }

    }

}
