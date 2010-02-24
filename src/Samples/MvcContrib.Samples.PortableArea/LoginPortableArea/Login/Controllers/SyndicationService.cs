using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;

namespace LoginPortableArea.Login.Controllers
{
    public class SyndicationService
    {

        /// <summary>
        /// Gets an RSS feed from a given URL and returns the resulting XML as a string.
        /// </summary>
        /// <param name="url">URL of the feed to load.</param>
        /// <param name="count">The count of postings to return.</param>
        /// <returns>The feed XML as a string.</returns>
        public SyndicationFeed GetFeed(string url, int count)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.CheckCharacters = true;
            settings.CloseInput = true;
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.ProhibitDtd = false;

            using (XmlReader reader = XmlReader.Create(url, settings))
            {
                SyndicationFeedFormatter formatter = null;
                Atom10FeedFormatter atom = new Atom10FeedFormatter();
                Rss20FeedFormatter rss = new Rss20FeedFormatter();
                SyndicationFeed feed;

                // Determine the format of the feed
                if (reader.ReadState == ReadState.Initial)
                {
                    reader.MoveToContent();
                }

                if (atom.CanRead(reader))
                {
                    formatter = atom;
                }

                if (rss.CanRead(reader))
                {
                    formatter = rss;
                }

                if (formatter == null)
                {
                    return null;
                }

                formatter.ReadFrom(reader);
                feed = formatter.Feed;

                // Remove unwanted items
                List<SyndicationItem> items = new List<SyndicationItem>();

                int added = 0;

                foreach (SyndicationItem i in feed.Items)
                {
                    items.Add(i);

                    if (added++ == count - 1) break;
                }

                feed.Items = items;
                return feed;
            }
        }

    }
}