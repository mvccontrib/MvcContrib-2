using System;
using System.Net.Mail;
using System.Security.Policy;
using System.Security.Principal;
using System.Web.Mvc;
using System.Xml;
using NUnit.Framework;

using Rhino.Mocks;

namespace MvcContrib.UnitTests
{
    [TestFixture]
    public class ViewDataExtensionsForViewDataTester
    {
        [Test]
        public void ShouldRetrieveSingleObjectByType()
        {
            var viewData = new ViewDataDictionary();
            var url = new Url("/asdf"); //arbitrary object
            viewData.Add(url);

            Assert.That(viewData.Get<Url>(), Is.EqualTo(url));
            Assert.That(viewData.Get(typeof(Url)), Is.EqualTo(url));
        }

        [Test, ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "No object exists that is of type 'System.Net.Mail.MailMessage'.")]
        public void ShouldGetObjectBasedOnType()
        {
            var url = new Url("/1");
            var identity = new GenericIdentity("name");

        	var viewData = new ViewDataDictionary();
        	viewData.Add(identity).Add(url);

            viewData.Get(typeof(MailMessage));
        }

        [Test, ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "No object exists with key 'System.Security.Policy.Url'.")]
        public void ShouldGetMeaningfulExceptionIfObjectDoesntExist()
        {
        	var viewData = new ViewDataDictionary();
            var url = viewData.Get<Url>();
        }

        [Test]
        public void ShouldReportContainsCorrectly()
        {
            var viewData = new ViewDataDictionary();
            viewData.Add(new Url("/2"));

            Assert.That(viewData.Contains<Url>());
            Assert.That(viewData.Contains(typeof(Url)));
        }

        [Test]
        public void ShouldManageMoreThanOneObjectPerType()
        {
        	var viewData = new ViewDataDictionary {{"key1", new Url("/1")}, {"key2", new Url("/2")}};

        	Assert.That(viewData.Get<Url>("key1").Value, Is.EqualTo("/1"));
            Assert.That(viewData.Get<Url>("key2").Value, Is.EqualTo("/2"));
        }

        [Test, ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "No object exists with key 'foobar'.")]
        public void ShouldGetMeaningfulExceptionIfObjectDoesntExistByKey()
        {
            var viewData = new ViewDataDictionary();
            var url = viewData.Get<Url>("foobar");
        }

        [Test]
        public void ShouldBeAbleToGetADefaultValueIfTheKeyDoesntExist()
        {
            DateTime theDate = DateTime.Parse("April 04, 2005");
            DateTime defaultDate = DateTime.Parse("October 31, 2005");

            var viewData = new ViewDataDictionary();

            Assert.That(viewData.GetOrDefault("some_date", defaultDate), Is.EqualTo(defaultDate));

            var viewData2 = new ViewDataDictionary {{"some_date", theDate}};
        	Assert.That(viewData2.GetOrDefault("some_date", defaultDate), Is.EqualTo(theDate));
        }

        [Test]
        public void ShouldHandleProxiedObjectsByType()
        {
            var mailMessageStub = MockRepository.GenerateStub<MailMessage>();
            var viewData = new ViewDataDictionary();
			viewData.Add(mailMessageStub);
            var message = viewData.Get<MailMessage>();

            Assert.That(message, Is.EqualTo(mailMessageStub));
        }

        [Test]
        public void ShouldInitializeWithProxiesAndResolveCorrectly()
        {
            var messageProxy = MockRepository.GenerateStub<MailMessage>();
            var xmlDocumentProxy = MockRepository.GenerateStub<XmlDocument>();
			
			var viewData = new ViewDataDictionary();
			viewData.Add(messageProxy).Add(xmlDocumentProxy);

            Assert.That(viewData.Get<MailMessage>(), Is.EqualTo(messageProxy));
            Assert.That(viewData.Get<XmlDocument>(), Is.EqualTo(xmlDocumentProxy));
        }

        [Test]
        public void ShouldInitializeWithKeys()
        {
			var viewData = new ViewDataDictionary {{"key1", 2}, {"key2", 3}};
        	Assert.That(viewData.Contains("key1"));
            Assert.That(viewData.Contains("key2"));
        }
    }
}
