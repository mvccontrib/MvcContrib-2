using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MvcContrib.IncludeHandling.Configuration
{
	public abstract class IncludeTypeElement : ConfigurationElement, IIncludeTypeSettings
	{
		private const string CACHEFOR = "cacheFor";
		private const string COMPRESSIONORDER = "compressionOrder";
		private const string DEFAULTCACHEFOR = "365.00:00:00.000";
		private const string DEFAULTCOMPRESSIONORDER = "gzip,deflate";
		private const string DEFAULTPATH = "~/include/{0}/{1}";
		private const string LINEBREAKAT = "lineBreakAt";
		private const string MINIFY = "minify";
		private const string COMPRESS = "compress";
		private const string PATH = "path";
		protected const string OPTIONS = "options";

		private IList<ResponseCompression> _compressionOrderList;
		private string _path;

		[ConfigurationProperty(COMPRESSIONORDER, DefaultValue = DEFAULTCOMPRESSIONORDER)]
		protected string compressionOrder
		{
			get
			{
				try
				{
					return this[COMPRESSIONORDER].ToString();
				}
				catch
				{
					return DEFAULTCOMPRESSIONORDER;
				}
			}
		}

		[ConfigurationProperty(CACHEFOR, DefaultValue = DEFAULTCACHEFOR)]
		protected string cacheFor
		{
			get
			{
				try
				{
					return this[CACHEFOR].ToString();
				}
				catch
				{
					return DEFAULTCACHEFOR;
				}
			}
		}

		#region IIncludeTypeSettings Members

		[ConfigurationProperty(LINEBREAKAT, DefaultValue = int.MaxValue)]
		public int LineBreakAt
		{
			get
			{
				int result;
				if (!int.TryParse(this[LINEBREAKAT].ToString(), out result))
				{
					result = int.MaxValue;
				}
				if (result <= 0)
				{
					result = int.MaxValue;
				}
				return result;
			}
		}

		[ConfigurationProperty(PATH, DefaultValue = DEFAULTPATH)]
		public string Path
		{
			get
			{
				if (_path == null)
				{
					_path = this[PATH].ToString();
					if (_path == null)
					{
						_path = DEFAULTPATH;
					}
					if (!_path.Contains("{0}") || !_path.Contains("{1}"))
					{
						throw new ConfigurationErrorsException("path must contain two format placeholders; {0} and {1}, for type and key respectively.");
					}
				}
				return _path;
			}
		}

		public IList<ResponseCompression> CompressionOrder
		{
			get
			{
				if (_compressionOrderList == null)
				{
					_compressionOrderList = compressionOrder.Split(',').CastToEnum<ResponseCompression>(true).ToList();
				}
				return _compressionOrderList;
			}
		}

		[ConfigurationProperty(MINIFY, DefaultValue = true)]
		public bool Minify
		{
			get { return (bool) this[MINIFY]; }
		}

		[ConfigurationProperty(COMPRESS, DefaultValue = true)]
		public bool Compress
		{
			get { return (bool) this[COMPRESS]; }
		}

		public TimeSpan? CacheFor
		{
			get
			{
				TimeSpan result;
				if (!TimeSpan.TryParse(cacheFor, out result))
				{
					result = TimeSpan.FromDays(365);
				}
				return result;
			}
		}

		#endregion
	}
}