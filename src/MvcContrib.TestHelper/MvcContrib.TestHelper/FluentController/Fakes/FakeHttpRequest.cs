using System;
using System.Collections.Specialized;
using System.Web;

namespace MvcContrib.TestHelper.FluentController.Fakes
{
    public class FakeHttpRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection _cookies;
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly NameValueCollection _serverVariables;
        private readonly string _relativeUrl;
        private readonly Uri _url;
        private readonly Uri _urlReferrer;

        public FakeHttpRequest(string relativeUrl, NameValueCollection formParams, NameValueCollection queryStringParams,
                               HttpCookieCollection cookies)
        {
            _relativeUrl = relativeUrl;
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
            _serverVariables = new NameValueCollection();
        }

        public FakeHttpRequest(string relativeUrl, Uri url, Uri urlReferrer, NameValueCollection formParams, NameValueCollection queryStringParams,
                               HttpCookieCollection cookies)
            : this(relativeUrl, formParams, queryStringParams, cookies)
        {
            _url = url;
            _urlReferrer = urlReferrer;
        }

        public FakeHttpRequest(string relativeUrl, Uri url, Uri urlReferrer)
            : this(relativeUrl, url, urlReferrer, null, null, null)
        {
        }

        public override NameValueCollection ServerVariables
        {
            get
            {
                return _serverVariables;
            }
        }

        public override NameValueCollection Form
        {
            get { return _formParams; }
        }

        public override NameValueCollection QueryString
        {
            get { return _queryStringParams; }
        }

        public override HttpCookieCollection Cookies
        {
            get { return _cookies; }
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get { return _relativeUrl; }
        }

        public override Uri Url
        {
            get
            {
                return _url;
            }
        }

        public override Uri UrlReferrer
        {
            get
            {
                return _urlReferrer;
            }
        }

        public override string PathInfo
        {
            get { return String.Empty; }
        }

        public override string ApplicationPath
        {
            get
            {
                return "";
            }
        }
    }
}