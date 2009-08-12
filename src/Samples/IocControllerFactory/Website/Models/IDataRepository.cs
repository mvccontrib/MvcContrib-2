using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

namespace Website.Models
{
    public interface ILinkRepository
    {
        IEnumerable<Link> GetLinks();
    }

    public class  InMemoryLinkRepository:ILinkRepository
    {
        public IEnumerable<Link> GetLinks()
        {
            List<Link> links = new List<Link>(3);
            
            links.Add(new Link(){Url = "http://mvccontrig.org",Title = "MvcContrib Website"});
            links.Add(new Link(){Url = "http://groups.google.com/group/mvccontrib-discuss",Title = "MvcContrib Discussion Group"});
            links.Add(new Link(){Url = "http://build.mvccontrib.org",Title = "Continous Build Server"});
            links.Add(new Link(){Url = "http://mvccontrib.googlecode.com/svn/trunk/src",Title = "Source Code"});

            return links;
        }
    }

    public class Link
    {
        public string Url { get; set; }
        public string Title { get; set;} 
    }
}

