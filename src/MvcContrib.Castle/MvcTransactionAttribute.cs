using System;
using System.Web.Mvc;
using Castle.Services.Transaction;
using MvcContrib.Services;

namespace MvcContrib.Castle
{
	/// <summary>
	/// Indicates the transaction support for a method.
	/// This attribute is modeled after Castle's ATM:
	/// http://www.castleproject.org/container/facilities/v1rc3/atm/index.html
	/// 
	/// Castle ATM used DynamicProxy to wrap the Transaction methods. This causes problems with Parameter Binders because DynamicProxy does
	/// not copy parameter attributes, a known bug (DYNPROXY-ISSUE-14) currently market as Won't Fix. (10/19/08)
	/// 
	/// There is no Controller attribute for using the MvcTransactionAttribute, 
	/// simply mark the methods that you want transactioned with MvcTransaction
	/// 
	/// [MvcTransaction]
	/// public void ActionResult AddItem
	/// {
	///   //do work
	/// }
	///
	/// Thrown Exceptions will cause a rollback. At minimum you'll need to configure an ITransactionManager with the DependencyResolver.
	/// For example with NHibernate and Rhino Tools this would go in your global.aspx.cs:
	/// 
	/// Container.AddFacility("rhino_transaction", new RhinoTransactionFacility());
	/// DependencyResolver.InitializeWith(new WindsorDependencyResolver(Container));
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class MvcTransactionAttribute : ActionFilterAttribute
	{
		private ITransaction transaction;
		private bool rolledback;

		/// <summary>
		/// Declares unspecified values for transaction and isolation, which
		/// means that the transaction manager will use the default values
		/// for them
		/// </summary>
		public MvcTransactionAttribute()
			: this(TransactionMode.Unspecified, IsolationMode.Unspecified)
		{
		}

		/// <summary>
		/// Declares the transaction mode, but omits the isolation, 
		/// which means that the transaction manager should use the
		/// default value for it.
		/// </summary>
		/// <param name="transactionMode"></param>
		public MvcTransactionAttribute(TransactionMode transactionMode)
			: this(transactionMode, IsolationMode.Unspecified)
		{
		}

		/// <summary>
		/// Declares both the transaction mode and isolation 
		/// desired for this method. The transaction manager should
		/// obey the declaration.
		/// </summary>
		/// <param name="transactionMode"></param>
		/// <param name="isolationMode"></param>
		public MvcTransactionAttribute(TransactionMode transactionMode, IsolationMode isolationMode)
		{
			TransactionMode = transactionMode;
			IsolationMode = isolationMode;
			Distributed = false;
		}

		/// <summary>
		/// Returns the <see cref="IsolationMode"/>
		/// </summary>
		public IsolationMode IsolationMode { get; set; }

		/// <summary>
		/// Returns the <see cref="TransactionMode"/>
		/// </summary>
		public TransactionMode TransactionMode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the transaction should be distributed.
		/// </summary>
		/// <value>
		/// <c>true</c> if a distributed transaction should be created; otherwise, <c>false</c>.
		/// </value>
		public bool Distributed { get; set; }

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			rolledback = false;
			
			var manager = DependencyResolver.Resolver.GetImplementationOf<ITransactionManager>();
			transaction = manager.CreateTransaction(TransactionMode, IsolationMode, Distributed);
			if (transaction != null)
			{
				transaction.Begin();
			}
			base.OnActionExecuting(filterContext);
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (transaction == null)
				throw new TransactionException("Transaction was never started.");

			Exception exception = filterContext.Exception;
			try
			{
				if (exception == null)
				{
					if (transaction.IsRollbackOnlySet)
					{
						rolledback = true;
						transaction.Rollback();
					}
					else
					{
						transaction.Commit();
					}
				}
				else
					throw exception;
			}
			catch (TransactionException)
			{
				throw;
			}
			catch (Exception)
			{
				if (!rolledback)
				{
					transaction.Rollback();
				}
				throw;
			}
			finally
			{
				var manager = DependencyResolver.Resolver.GetImplementationOf<ITransactionManager>();
				manager.Dispose(transaction);
				transaction = null;
			}
			base.OnActionExecuted(filterContext);
		}
	}
}
