using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.Services.Transaction;
using Castle.Windsor;
using MvcContrib.Castle;
using MvcContrib.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.Filters
{
	[TestFixture]
	public class MvcTransactionAttributeTester
	{
		private ITransactionManager manager;
		private MvcTransactionAttribute attribute;

		[SetUp]
		public void SetUp()
		{
			var container = new WindsorContainer();
			DependencyResolver.InitializeWith(new WindsorDependencyResolver(container));
			container.AddComponent("transaction.manager", typeof(ITransactionManager), typeof(TestITransactionManager));
			manager = DependencyResolver.Resolver.GetImplementationOf<ITransactionManager>();
            attribute = new MvcTransactionAttribute();
		}

		[Test]
		public void TransactionStarted_OnActionExecuting()
		{
			attribute.OnActionExecuting(GetActionExecutingContext());

			Assert.AreEqual(1, ((TestITransactionManager)manager).CreateTransactionCalled);
			Assert.AreEqual(1, ((TestITransaction)manager.CurrentTransaction).BeginCalled);
		}

		[Test, ExpectedException(typeof(TransactionException))]
		public void NonStartedTransactionThrows_OnActionOnActionExecuted()
		{
			attribute.OnActionExecuted(GetActionExecutedContext(null));
		}
        
		[Test]
		public void TransactionCommitted_AndDisposed_OnActionOnActionExecuted()
		{
			attribute.OnActionExecuting(GetActionExecutingContext());
			attribute.OnActionExecuted(GetActionExecutedContext(null));
			Assert.AreEqual(1, ((TestITransaction)manager.CurrentTransaction).CommitCalled);
			Assert.AreEqual(0, ((TestITransaction)manager.CurrentTransaction).RollbackCalled);
			Assert.AreEqual(1, ((TestITransactionManager)manager).DisposeTransactionCalled);
		}

		[Test]
		public void TransactionRolledback_WhenExection_OnActionOnActionExecuted()
		{
			attribute.OnActionExecuting(GetActionExecutingContext());
			Exception thrown = null;
			try
			{
				attribute.OnActionExecuted(GetActionExecutedContext(new Exception("Exception")));
			}
			catch(Exception e)
			{
				thrown = e;
			}
			Assert.IsNotNull(thrown);
			Assert.AreEqual(0, ((TestITransaction)manager.CurrentTransaction).CommitCalled);
			Assert.AreEqual(1, ((TestITransaction)manager.CurrentTransaction).RollbackCalled);
			Assert.AreEqual(1, ((TestITransactionManager)manager).DisposeTransactionCalled);
		}

		[Test]
		public void TransactionRolledback_WhenIsRollbackOnlySet_OnActionOnActionExecuted()
		{
			attribute.OnActionExecuting(GetActionExecutingContext());
			manager.CurrentTransaction.SetRollbackOnly();
			attribute.OnActionExecuted(GetActionExecutedContext(null));
			Assert.AreEqual(0, ((TestITransaction)manager.CurrentTransaction).CommitCalled);
			Assert.AreEqual(1, ((TestITransaction)manager.CurrentTransaction).RollbackCalled);
			Assert.AreEqual(1, ((TestITransactionManager)manager).DisposeTransactionCalled);
		}


		[Test]
		public void TransactionExceptionsRethrownWithNoRollback_WhenIsRollbackOnlySet_OnActionOnActionExecuted()
		{
			attribute.OnActionExecuting(GetActionExecutingContext());
			Exception thrown = null;
			try
			{
				attribute.OnActionExecuted(GetActionExecutedContext(new TransactionException("Exception")));
			}
			catch (Exception e)
			{
				thrown = e;
			}
			Assert.IsNotNull(thrown);
			Assert.AreEqual(0, ((TestITransaction)manager.CurrentTransaction).CommitCalled);
			Assert.AreEqual(0, ((TestITransaction)manager.CurrentTransaction).RollbackCalled);
			Assert.AreEqual(1, ((TestITransactionManager)manager).DisposeTransactionCalled);
		}

		[Test]
		public void CanCreateDifferentAttributesWithCorrectParameters()
		{
			var attribute1 = new MvcTransactionAttribute();
			
			var attribute2 = new MvcTransactionAttribute(TransactionMode.Supported);
			Assert.AreEqual(TransactionMode.Supported,attribute2.TransactionMode);
			var attribute3 = new MvcTransactionAttribute(TransactionMode.RequiresNew, IsolationMode.Chaos);
			Assert.AreEqual(TransactionMode.RequiresNew, attribute3.TransactionMode);
			Assert.AreEqual(IsolationMode.Chaos, attribute3.IsolationMode);
		}

		private static ActionExecutingContext GetActionExecutingContext()
		{
			var actionExecutingContext = new ActionExecutingContext(GetControllerContext(), MockRepository.GenerateStub<ActionDescriptor>(), new Dictionary<string, object>());
			return actionExecutingContext;
		}

		private static ActionExecutedContext GetActionExecutedContext(Exception e)
		{
			var actionExecutingContext = new ActionExecutedContext(GetControllerContext(), MockRepository.GenerateStub<ActionDescriptor>() ,false, e);
			return actionExecutingContext;
		}

		private static ControllerContext GetControllerContext()
		{
			var controller = new TestingController();
			var mockHttpContext = MockRepository.GenerateStub<HttpContextBase>();
			var controllerContext = new ControllerContext(mockHttpContext, new RouteData(), controller);
			controller.ControllerContext = controllerContext;
			return controllerContext;
		}


		internal class TestingController : Controller
		{
		}


		internal class TestITransactionManager : ITransactionManager, IFacility
		{
			public int CreateTransactionCalled { get; set; }
			public int DisposeTransactionCalled { get; set; }

			public ITransaction CreateTransaction(TransactionMode transactionMode, IsolationMode isolationMode)
			{
				return CreateTransaction(transactionMode, isolationMode, false);
			}

			public ITransaction CreateTransaction(TransactionMode transactionMode, IsolationMode isolationMode, bool distributedTransaction)
			{
				CreateTransactionCalled++;
				return (CurrentTransaction = new TestITransaction());
			}

			public void Dispose(ITransaction transaction)
			{
				DisposeTransactionCalled++;
			}

			public ITransaction CurrentTransaction { get; set; }

			public void Init(IKernel kernel, IConfiguration facilityConfig) { }
			public void Terminate() { }

			public event TransactionCreationInfoDelegate TransactionCreated;
			public event TransactionCreationInfoDelegate ChildTransactionCreated;
			public event TransactionDelegate TransactionCommitted;
			public event TransactionDelegate TransactionRolledback;
			public event TransactionDelegate TransactionDisposed;
			public event TransactionErrorDelegate TransactionFailed;

			//these invokers below are there to remove teh "Warning as Error" message. If you have a better way of removing this message, go for it.
			private void InvokeTransactionFailed(ITransaction transaction, TransactionException transactionError)
			{
				TransactionErrorDelegate Delegate = TransactionFailed;
			}
			private void InvokeTransactionCreated(ITransaction transaction, TransactionMode transactionMode, IsolationMode isolationMode, bool distributedTransaction)
			{
				TransactionCreationInfoDelegate Delegate = TransactionCreated;
			}
			private void InvokeChildTransactionCreated(ITransaction transaction, TransactionMode transactionMode, IsolationMode isolationMode, bool distributedTransaction)
			{
				TransactionCreationInfoDelegate Delegate = ChildTransactionCreated;
			}
			private void InvokeTransactionCommitted(ITransaction transaction)
			{
				TransactionDelegate Delegate = TransactionCommitted;
			}
			private void InvokeTransactionDisposed(ITransaction transaction)
			{
				TransactionDelegate Delegate = TransactionDisposed;
			}
			private void InvokeTransactionRolledback(ITransaction transaction)
			{
				TransactionDelegate Delegate = TransactionRolledback;
			}
		}

		internal class TestITransaction : ITransaction
		{
			public int BeginCalled { get; set; }
			public int	CommitCalled { get; set; }
			public int RollbackCalled { get; set; }

			public void Begin()
			{
				BeginCalled++;
			}

			public void Commit()
			{
				CommitCalled++;
			}

			public void Rollback()
			{
				RollbackCalled++;
			}

			public void SetRollbackOnly()
			{
				IsRollbackOnlySet = true;
			}

			public void Enlist(IResource resource)
			{
				throw new System.NotImplementedException();
			}

			public void RegisterSynchronization(ISynchronization synchronization)
			{
				throw new System.NotImplementedException();
			}

			public TransactionStatus Status
			{
				get { throw new System.NotImplementedException(); }
			}

			public IDictionary Context
			{
				get { throw new System.NotImplementedException(); }
			}

			public bool IsChildTransaction
			{
				get { throw new System.NotImplementedException(); }
			}

			public bool IsRollbackOnlySet
			{
				get; set;
			}

			public TransactionMode TransactionMode
			{
				get { throw new System.NotImplementedException(); }
			}

			public IsolationMode IsolationMode
			{
				get { throw new System.NotImplementedException(); }
			}

			public bool DistributedTransaction
			{
				get { throw new System.NotImplementedException(); }
			}

			public string Name
			{
				get { throw new System.NotImplementedException(); }
			}

			public IResource[] Resources
			{
				get { throw new System.NotImplementedException(); }
			}
		}
	}

}
