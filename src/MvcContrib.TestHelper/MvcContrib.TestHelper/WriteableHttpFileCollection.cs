using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcContrib.TestHelper
{
	/// <summary>
	/// An implementation of HttpFileCollectionBase to assit with testing.
	/// </summary>
	public class WriteableHttpFileCollection : HttpFileCollectionBase, IWriteableHttpFileCollection
	{
		private readonly Dictionary<string, HttpPostedFileBase> files;

		/// <summary>
		/// Initializes a new instance of the <see cref="WriteableHttpFileCollection"/> class.
		/// </summary>
		public WriteableHttpFileCollection()
		{
			files = new Dictionary<string, HttpPostedFileBase>();
		}

		/// <summary>
		/// Gets a string array containing the keys (names) of all members in the file collection.
		/// </summary>
		/// <value>An array of file names.</value>
		public override string[] AllKeys
		{
			get { return files.Keys.ToArray(); }
		}

		/// <summary>
		/// Gets the object with the specified name from the file collection.
		/// </summary>
		/// <param name="name">Name of item to be returned.</param>
		/// <returns>The System.Web.HttpPostedFile specified by name.</returns>
		public override HttpPostedFileBase this[string name]
		{
			get { return files[name]; }
		}

		/// <summary>
		/// Gets the object with the specified index from the file collection.
		/// </summary>
		/// <param name="index">Index of item to be returned.</param>
		/// <returns>The System.Web.HttpPostedFile specified by index.</returns>
		public override HttpPostedFileBase this[int index]
		{
			get { return files[AllKeys[index]]; }
		}

		/// <summary>
		/// Gets or sets the <see cref="HttpPostedFileBase"/> with the specified name.
		/// </summary>
		HttpPostedFileBase IWriteableHttpFileCollection.this[string name]
		{
			get { return files[name]; }
			set { files[name] = value; }
		}

		/// <summary>
		/// Gets the file count of the underlying <see cref="HttpFileCollectionBase"/>.
		/// </summary>
		public override int Count
		{
			get { return files.Count; }
		}
	}
}
