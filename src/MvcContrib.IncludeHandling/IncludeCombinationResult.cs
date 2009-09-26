using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcContrib.IncludeHandling.Configuration;

namespace MvcContrib.IncludeHandling
{
	public enum ResponseCompression
	{
		None = 0,
		Gzip = 1,
		Deflate = 2
	}

	public class IncludeCombinationResult : ActionResult
	{
		private static readonly IDictionary<IncludeType, string> _contentTypes = new Dictionary<IncludeType, string>
		{
			{ IncludeType.Css, MimeTypes.TextCss }
			, { IncludeType.Js, MimeTypes.ApplicationJavaScript }
		};

		private static readonly IDictionary<ResponseCompression, Func<string, bool>> _compressionOrder = new Dictionary<ResponseCompression, Func<string, bool>>
		{
			{ ResponseCompression.Gzip, header => header.Contains("gzip") },
			{ ResponseCompression.Deflate, header => header.Contains("deflate") },
			{ ResponseCompression.None, header => true }
		};

		private readonly string _key;
		private DateTime _now;
		private TimeSpan? _cacheFor;
		private readonly IList<ResponseCompression> _preferredCompressionOrder;

		public IncludeCombination Combination { get; private set; }

		public IncludeCombinationResult(IIncludeCombiner combiner, string key, DateTime now)
		{
			if (combiner == null)
			{
				throw new ArgumentNullException("combiner");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			_key = key;
			_now = now;
			Combination = combiner.GetCombination(_key);
		}

		public IncludeCombinationResult(IIncludeCombiner combiner, string key, DateTime now, IIncludeHandlingSettings settings)
			: this(combiner, key, now)
		{
			var typeSettings = settings.Types[Combination.Type];
			_cacheFor = typeSettings.CacheFor;
			if (typeSettings.Compress)
			{
				_preferredCompressionOrder = typeSettings.CompressionOrder;
			}
			else
			{
				_preferredCompressionOrder = new List<ResponseCompression> { ResponseCompression.None };
			}
		}

		public override void ExecuteResult(ControllerContext context)
		{
			var response = context.HttpContext.Response;
			response.ContentEncoding = Encoding.UTF8;
			if (Combination == null || Combination.Content == null)
			{
				response.StatusCode = (int) HttpStatusCode.NotFound;
				return;
			}
			response.ContentType = _contentTypes[Combination.Type];
			var cache = response.Cache;
			if (_cacheFor.HasValue)
			{
				cache.SetCacheability(HttpCacheability.Public);
				cache.SetExpires(_now.Add(_cacheFor.Value));
				cache.SetMaxAge(_cacheFor.Value);
				cache.SetValidUntilExpires(true);
				cache.SetLastModified(Combination.LastModifiedAt);
			}
			cache.SetETag(_key + "-" + Combination.LastModifiedAt.Ticks);
			var compressionAccepted = figureOutCompression(context.HttpContext.Request);
			var responseBodyBytes = Combination.Bytes[compressionAccepted];

			if (responseBodyBytes.Length <= 0)
			{
				response.StatusCode = (int) HttpStatusCode.NoContent;
				return;
			}
			switch (compressionAccepted)
			{
				case ResponseCompression.None:
					break;
				case ResponseCompression.Gzip:
				case ResponseCompression.Deflate:
					response.AppendHeader(HttpHeaders.ContentEncoding, compressionAccepted.ToString().ToLowerInvariant());
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			response.AddHeader(HttpHeaders.ContentLength, responseBodyBytes.Length.ToString());
			response.OutputStream.Write(responseBodyBytes, 0, responseBodyBytes.Length);
			response.OutputStream.Flush();
		}

		private ResponseCompression figureOutCompression(HttpRequestBase request)
		{
			var acceptEncoding = request.Headers[HttpHeaders.AcceptEncoding];
			if (string.IsNullOrEmpty(acceptEncoding) || isIe6OrLess(request.Browser))
			{
				return ResponseCompression.None;
			}
			acceptEncoding = acceptEncoding.Trim().ToLowerInvariant();

			foreach(var compression in _preferredCompressionOrder)
			{
				if (_compressionOrder[compression](acceptEncoding))
				{
					return compression;
				}
			}
			return ResponseCompression.None;
		}

		private static bool isIe6OrLess(HttpBrowserCapabilitiesBase browser)
		{
			return
				browser.Type.Contains("IE") && browser.MajorVersion <= 6;
		}
	}
}