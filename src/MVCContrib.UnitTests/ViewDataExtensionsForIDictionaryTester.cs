using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Policy;
using System.Security.Principal;
using System.Xml;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
    [TestFixture]
    public class ViewDataExtensionsForIDictionaryTester
    {
        [Test]
        public void ShouldRetrieveSingleObjectByType()
        {
            var bag = new Dictionary<string, object>();
            var url = new Url("/asdf"); //arbitrary object
            bag.Add(url);

            Assert.That(bag.Get<Url>(), Is.EqualTo(url));
            Assert.That(bag.Get(typeof(Url)), Is.EqualTo(url));
        }

        [Test, ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "No object exists that is of type 'System.Net.Mail.MailMessage'.")]
        public void ShouldGetObjectBasedOnType()
        {
            var url = new Url("/1");
            var identity = new GenericIdentity("name");

            var bag = new Dictionary<string, object>();
            bag.Add(identity).Add(url);

            bag.Get(typeof(MailMessage));
        }

        [Test, ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "You can only add one default object for type 'System.Security.Policy.Url'.")]
        public void AddingTwoDefaultObjectsOfSameTypeThrows()
        {
            var url1 = new Url("/1");
            var url2 = new Url("/2");

            var bag = new Dictionary<string, object>();
            bag.Add(url1);
            bag.Add(url2);
        }

        [Test, ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "No object exists with key 'System.Security.Policy.Url'.")]
        public void ShouldGetMeaningfulExceptionIfObjectDoesntExist()
        {
            var bag = new Dictionary<string, object>();
            var url = bag.Get<Url>();
        }

        [Test]
        public void ShouldReportContainsCorrectly()
        {
            var bag = new Dictionary<string, object>();
            bag.Add(new Url("/2"));

            Assert.That(bag.Contains<Url>());
            Assert.That(bag.Contains(typeof(Url)));
        }

        [Test]
        public void ShouldManageMoreThanOneObjectPerType()
        {
            var bag = new Dictionary<string, object> {{"key1", new Url("/1")}, {"key2", new Url("/2")}};

        	Assert.That(bag.Get<Url>("key1").Value, Is.EqualTo("/1"));
            Assert.That(bag.Get<Url>("key2").Value, Is.EqualTo("/2"));
        }

        [Test, ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "No object exists with key 'foobar'.")]
        public void ShouldGetMeaningfulExceptionIfObjectDoesntExistByKey()
        {
            var bag = new Dictionary<string, object>();
            var url = bag.Get<Url>("foobar");
        }

        [Test]
        public void ShouldCountNumberOfObjectsOfGivenType()
        {
            var bag = new Dictionary<string, object>();
            Assert.That(bag.GetCount(typeof(Url)), Is.EqualTo(0));

            bag.Add("1", new Url("/1"));
            bag.Add("2", new Url("/2"));
            bag.Add("3", new Url("/3"));

            Assert.That(bag.GetCount(typeof(Url)), Is.EqualTo(3));
        }

        [Test]
        public void ShouldBeAbleToInitializeBagWithSeveralObjects()
        {
            var url = new Url("/1");
            var identity = new GenericIdentity("name");

            var bag = new Dictionary<string, object>();
            bag.Add(identity).Add(url);
            Assert.That(bag.Get(typeof(GenericIdentity)), Is.EqualTo(identity));
            Assert.That(bag.Get(typeof(Url)), Is.EqualTo(url));
        }

        [Test]
        public void ShouldBeAbleToInitializeBagWithSeveralObjectsWithExplicitTypes()
        {
            var url = new Url("/1");
            var identity = new GenericIdentity("name");

            var bag = new Dictionary<string, object>();
            bag.Add<GenericIdentity>("first", identity);
            bag.Add<Url>("second", url);
            Assert.That(bag.Get<GenericIdentity>("first"), Is.EqualTo(identity));
            Assert.That(bag.Get<Url>("second"), Is.EqualTo(url));
        }

        [Test]
        public void ShouldBeAbleToGetADefaultValueIfTheKeyDoesntExist()
        {
            DateTime theDate = DateTime.Parse("April 04, 2005");
            DateTime defaultDate = DateTime.Parse("October 31, 2005");

            var bag = new Dictionary<string, object>();
            Assert.That(bag.GetOrDefault("some_date", defaultDate), Is.EqualTo(defaultDate));

            bag.Add("some_date", theDate);
            Assert.That(bag.GetOrDefault("some_date", defaultDate), Is.EqualTo(theDate));
        }

        [Test]
        public void ShouldHandleProxiedObjectsByType()
        {
            var mailMessageStub = MockRepository.GenerateStub<MailMessage>();
            var bag = new Dictionary<string, object>();
            bag.Add(mailMessageStub);
            var message = bag.Get<MailMessage>();

            Assert.That(message, Is.EqualTo(mailMessageStub));
        }

        [Test]
        public void ShouldInitializeWithProxiesAndResolveCorrectly()
        {
            var messageProxy = MockRepository.GenerateStub<MailMessage>();
            var xmlDocumentProxy = MockRepository.GenerateStub<XmlDocument>();

            var bag = new Dictionary<string, object>();
            bag.Add(messageProxy).Add(xmlDocumentProxy);

            Assert.That(bag.Get<MailMessage>(), Is.EqualTo(messageProxy));
            Assert.That(bag.Get<XmlDocument>(), Is.EqualTo(xmlDocumentProxy));
        }

        [Test]
        public void ShouldInitializeWithKeys()
        {
            var bag = new Dictionary<string, object> {{"key1", 2}, {"key2", 3}};
        	Assert.That(bag.ContainsKey("key1"));
            Assert.That(bag.ContainsKey("key2"));
        }
    }
}
